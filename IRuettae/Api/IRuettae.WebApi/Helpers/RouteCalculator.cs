using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.Models;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;
using NHibernate;
using Santa = IRuettae.Persistence.Entities.Santa;
using Visit = IRuettae.Persistence.Entities.Visit;

namespace IRuettae.WebApi.Helpers
{
    public class RouteCalculator
    {
        public static ConcurrentBag<BackgroundWorker> BackgroundWorkers = new ConcurrentBag<BackgroundWorker>();

        private readonly long routeCalculationId;

        private BackgroundWorker bgWorker;


        public RouteCalculator(RouteCalculation routeCalculation)
        {
            this.routeCalculationId = routeCalculation.Id;
            SetupBgWorker();
        }

        private void SetupBgWorker()
        {
            bgWorker = new BackgroundWorker();
            BackgroundWorkers.Add(bgWorker);
            bgWorker.Disposed += (sender, args) =>
            {
                var dbSession = SessionFactory.Instance.OpenSession();
                var routeCalculation = dbSession.Get<RouteCalculation>(routeCalculationId);
                if (string.IsNullOrEmpty(routeCalculation.Result))
                {
                    routeCalculation.State = RouteCalculationState.Cancelled;
                    routeCalculation.StateText.Append(new RouteCalculationLog()
                    {
                        Log = $"{Environment.NewLine} {DateTime.Now} Background worker stopped"
                    });
                }

                dbSession.Update(routeCalculation);
                dbSession.Flush();
            };

            bgWorker.DoWork += BackgroundWorkerDoWork;
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs args)
        {
            try
            {
                #region Prepare
                var dbSession = SessionFactory.Instance.OpenSession();
                var routeCalculation = dbSession.Get<RouteCalculation>(routeCalculationId);

                var santas = dbSession.Query<Santa>().ToList();

                var visits = dbSession.Query<Visit>()
                    .Where(v => v.Year == routeCalculation.Year && v.Id != routeCalculation.StarterVisitId)
                    .ToList();

                visits.ForEach(v =>
                    v.Duration = 60 * (v.NumberOfChildren * routeCalculation.TimePerChild +
                                       routeCalculation.TimePerChildOffset));

                var startVisit = dbSession.Query<Visit>().First(v => v.Id == routeCalculation.StarterVisitId);

                routeCalculation.NumberOfSantas = santas.Count;
                routeCalculation.NumberOfVisits = visits.Count;

                var converter = new Converter.PersistenceToCoreConverter();
                var optimizationInput = converter.Convert(routeCalculation.Days, startVisit, visits, santas);

                routeCalculation.State = RouteCalculationState.Ready;
                dbSession.Update(routeCalculation);
                dbSession.Flush();
                #endregion Prepare

                #region Run

                routeCalculation.StartTime = DateTime.Now;

                var ilpData = JsonConvert.DeserializeObject<ILPStarterData>(routeCalculation.AlgorithmData);
                var solver = new ILPSolver(optimizationInput, ilpData);

                // note: Progress<> is not suitable here as it may use multiple threads
                var consoleProgress = new EventHandler<string>(OnConsoleProgressOnProgressChanged);
                var progress = new EventHandler<ProgressReport>(OnProgressOnProgressChanged);

                routeCalculation.State = RouteCalculationState.Running;
                dbSession.Update(routeCalculation);
                dbSession.Flush();

                var optimizationResult = solver.Solve((int)(ilpData.ClusteringTimeLimit + ilpData.SchedulingTimeLimit),
                    progress, consoleProgress);

                // refresh session as changes where made in other sessions (progress)
                dbSession.Clear();
                routeCalculation = dbSession.Get<RouteCalculation>(routeCalculation.Id);
                var routeCalculationResult = new RouteCalculationResult
                {
                    OptimizationResult = optimizationResult,
                    VisitMap = converter.VisitMap,
                    SantaMap = converter.SantaMap,
                    ZeroTime = converter.ZeroTime,
                };

                routeCalculation.Result = JsonConvert.SerializeObject(routeCalculationResult);
                routeCalculation.State = RouteCalculationState.Finished;

                #endregion Run


                #region metrics


                #endregion metrics

                routeCalculation.EndTime = DateTime.Now;
                dbSession.Update(routeCalculation);
                dbSession.Flush();

            }
            catch (Exception e)
            {
                var dbSession = SessionFactory.Instance.OpenSession();
                var routeCalculation = dbSession.Get<RouteCalculation>(routeCalculationId);
                routeCalculation.State = RouteCalculationState.Cancelled;
                routeCalculation.StateText.Add(new RouteCalculationLog() { Log = $"Error: {e.Message}"});
                dbSession.Update(routeCalculation);
                dbSession.Flush();
            }
        }

        public void StartWorker()
        {
            bgWorker.RunWorkerAsync();
        }

        private void OnConsoleProgressOnProgressChanged(object s, string message)
        {
            try
            {
                var dbSession = SessionFactory.Instance.OpenSession();
                var routeCalculation = dbSession.Get<RouteCalculation>(routeCalculationId);
                routeCalculation.StateText.Add(new RouteCalculationLog { Log = message });
                dbSession.Update(routeCalculation);
                dbSession.Flush();
            }
            catch
            {
                // ignored
            }
        }

        private void OnProgressOnProgressChanged(object s, ProgressReport report)
        {
            try
            {
                var dbSession = SessionFactory.Instance.OpenSession();
                var routeCalculation = dbSession.Get<RouteCalculation>(routeCalculationId);
                routeCalculation.Progress = report.Progress;
                dbSession.Update(routeCalculation);
                dbSession.Flush();

                OnConsoleProgressOnProgressChanged(s, $"Progress: {report.Progress:P2}");
            }
            catch
            {
                // ignored
            }
        }
    }

}