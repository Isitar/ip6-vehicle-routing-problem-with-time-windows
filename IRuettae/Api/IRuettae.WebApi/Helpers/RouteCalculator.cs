using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IRuettae.Converter;
using IRuettae.Core;
using IRuettae.Core.Models;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;
using Santa = IRuettae.Persistence.Entities.Santa;
using Visit = IRuettae.Persistence.Entities.Visit;

namespace IRuettae.WebApi.Helpers
{
    public class RouteCalculator
    {
        
        private static BlockingCollection<long> work = new BlockingCollection<long>(new ConcurrentQueue<long>());


        public static void EnqueueRouteCalculation(long routeCalculationId)
        {
            work.Add(routeCalculationId);
        }
        

        private static void CalculateRoute(long routeCalculationId)
        {
            try
            {
                #region Prepare

                var dbSession = SessionFactory.Instance.OpenSession();
                var routeCalculation = dbSession.Get<RouteCalculation>(routeCalculationId);

                var santas = dbSession.Query<Santa>().OrderBy(s => s.Id).ToList();

                var visits = dbSession.Query<Visit>()
                    .Where(v => v.Year == routeCalculation.Year && v.Id != routeCalculation.StarterVisitId)
                    .OrderBy(v => v.Id)
                    .ToList();

                visits.ForEach(v =>
                    v.Duration = 60 * (v.NumberOfChildren * routeCalculation.TimePerChildMinutes +
                                       routeCalculation.TimePerChildOffsetMinutes));

                var startVisit = dbSession.Query<Visit>().First(v => v.Id == routeCalculation.StarterVisitId);

                routeCalculation.NumberOfSantas = santas.Count;
                routeCalculation.NumberOfVisits = visits.Count;

                var converter = new PersistenceToCoreConverter();
                var optimizationInput = converter.Convert(routeCalculation.Days, startVisit, visits, santas);

                // remove unnecessary santas
                {
                    // maximum number of additional santas to reach the theoretical optimum
                    var maxAdditional = optimizationInput.NumberOfVisits() - optimizationInput.NumberOfSantas();
                    maxAdditional = Math.Max(0, maxAdditional);
                    routeCalculation.MaxNumberOfAdditionalSantas = Math.Min(routeCalculation.MaxNumberOfAdditionalSantas, maxAdditional);
                }

                routeCalculation.State = RouteCalculationState.Ready;

                var solverConfig = SolverConfigFactory.CreateSolverConfig(routeCalculation, optimizationInput);
                routeCalculation.AlgorithmData = JsonConvert.SerializeObject(solverConfig);
                dbSession.Update(routeCalculation);
                dbSession.Flush();
                #endregion Prepare

                #region Run
                routeCalculation.StartTime = DateTime.Now;
               
                var solver = SolverFactory.CreateSolver(optimizationInput, solverConfig);

                // note: Progress<> is not suitable here as it may use multiple threads
                var consoleProgress = new EventHandler<string>((s,m) =>
                {
                    OnConsoleProgressOnProgressChanged(s,m,routeCalculationId);
                });
                
                var progress = new EventHandler<ProgressReport>((s, report) =>
                {
                    OnProgressOnProgressChanged(s, report, routeCalculationId);
                });

                routeCalculation.State = RouteCalculationState.Running;
                dbSession.Update(routeCalculation);
                dbSession.Flush();

                var optimizationResult = solver.Solve(routeCalculation.TimeLimitMiliseconds,
                    progress, consoleProgress);

                // refresh session as changes where made in other sessions (progress)
                dbSession.Clear();
                routeCalculation = dbSession.Get<RouteCalculation>(routeCalculation.Id);
                var routeCalculationResult = new RouteCalculationResult
                {
                    OptimizationResult = optimizationResult,
                    VisitMap = converter.VisitMap,
                    SantaMap = converter.SantaMap,
                    ZeroTime = converter.ZeroTime
                };
                routeCalculation.Result = JsonConvert.SerializeObject(routeCalculationResult);
                routeCalculation.State = RouteCalculationState.Finished;

                #endregion Run

                routeCalculation.EndTime = DateTime.Now;
                dbSession.Update(routeCalculation);
                dbSession.Flush();
            }
            catch (Exception e)
            {
                var dbSession = SessionFactory.Instance.OpenSession();
                var routeCalculation = dbSession.Get<RouteCalculation>(routeCalculationId);
                routeCalculation.State = RouteCalculationState.Cancelled;
                routeCalculation.StateText.Add(new RouteCalculationLog { Log = $"Error: {e.Message}" });
                dbSession.Update(routeCalculation);
                dbSession.Flush();
            }
        }

        public static void StartWorker()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var id = work.Take();
                    CalculateRoute(id);
                }
            });
        }

        private static void OnConsoleProgressOnProgressChanged(object s, string message, long routeCalculationId)
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

        private static void OnProgressOnProgressChanged(object s, ProgressReport report, long routeCalculationId)
        {
            try
            {
                var dbSession = SessionFactory.Instance.OpenSession();
                var routeCalculation = dbSession.Get<RouteCalculation>(routeCalculationId);
                routeCalculation.Progress = report.Progress;
                dbSession.Update(routeCalculation);
                dbSession.Flush();

                OnConsoleProgressOnProgressChanged(s, $"Progress: {report.Progress:P2}", routeCalculationId);
            }
            catch
            {
                // ignored
            }
        }
    }
}