using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using IRuettae.Core.Algorithm;
using IRuettae.Persistence.Entities;
using IRuettae.Preprocessing.Mapping;
using IRuettae.WebApi.Persistence;
using NHibernate;

namespace IRuettae.WebApi.Helpers
{
    public class Pipeline
    {
        private RouteCalculation routeCalculation;
        private readonly ISession dbSession;
        private BackgroundWorker bgWorker;


        public Pipeline(RouteCalculation routeCalculation)
        {
            this.routeCalculation = routeCalculation;
            this.dbSession = SessionFactory.Instance.OpenSession();
            dbSession.FlushMode = FlushMode.Always;

            SetupBgWorker();

        }

        private void SetupBgWorker()
        {
            bgWorker = new BackgroundWorker();
            routeCalculation = dbSession.Merge(routeCalculation);
            bgWorker.DoWork += (sender, args) =>
            {

                var santas = dbSession.Query<Santa>().ToList();

                var visits = dbSession.Query<Visit>()
                    .Where(v => v.Year == routeCalculation.Year || v.Id == routeCalculation.StarterVisitId)
                    .ToList();


                visits.ForEach(v => v.Duration = 60 * (v.NumberOfChildren * routeCalculation.TimePerChild +
                                       routeCalculation.TimePerChildOffset));

                // set starterId to front
                visits.Sort((a, b) =>
                {
                    if (a.Id == routeCalculation.StarterVisitId)
                    {
                        return -1;
                    }

                    if (b.Id == routeCalculation.StarterVisitId)
                    {
                        return 1;
                    }

                    return a.Id.CompareTo(b.Id);
                });


                routeCalculation.NumberOfSantas = santas.Count;
                routeCalculation.NumberOfVisits = visits.Count;
                routeCalculation.State = RouteCalculationState.Ready;
                dbSession.Update(routeCalculation);
                dbSession.Flush();
                
                var eventTextWriter = new EventTextWriter();
                var lastUpdate = DateTime.Now;
                eventTextWriter.CharWritten += (o, c) =>
                {
                    if (null == routeCalculation.StateText)
                    {
                        routeCalculation.StateText = string.Empty;
                    }

                    routeCalculation.StateText += c;
                    if (DateTime.Now - lastUpdate > TimeSpan.FromMinutes(1))
                    {
                        lastUpdate = DateTime.Now;
                        dbSession.Update(routeCalculation);
                        dbSession.Flush();
                    }
                };

                Console.SetOut(eventTextWriter);
                Console.SetError(eventTextWriter);
                

                // ******************************
                #region Clustering
                // ******************************

                var clusteringSolverVariableBuilder = new ClusteringSolverVariableBuilder
                {
                    Visits = visits,
                    Santas = santas,
                    Days = routeCalculation.Days
                };

                var clusteringSolverInputData = clusteringSolverVariableBuilder.Build();

                routeCalculation.StartTime = DateTime.Now;

                routeCalculation.State = RouteCalculationState.RunningPhase1;
                dbSession.Update(routeCalculation);
                dbSession.Flush();

                var targetType =
                    routeCalculation.ClusteringOptimisationFunction == ClusteringOptimisationGoals.MinTimePerSanta
                        ? TargetBuilderType.Default
                        : TargetBuilderType.MinTimeOnly;
#if DEBUG
                var serialPath = HostingEnvironment.MapPath($"~/App_Data/ClusteringSolverInput{routeCalculation.NumberOfVisits}Visits.serial");
                if (serialPath != null)
                {
                    using (var stream = File.Open(serialPath, FileMode.Create))
                    {
                        new BinaryFormatter().Serialize(stream, clusteringSolverInputData);
                    }
                }

                var mpsPath = HostingEnvironment.MapPath($"~/App_Data/Clustering_{routeCalculation.NumberOfVisits}.mps");
                Starter.SaveMps(mpsPath, clusteringSolverInputData, targetType);
#endif

                var phase1Result = Starter.Optimise(clusteringSolverInputData, targetType, routeCalculation.ClustringMipGap);

                // gets captured by eventwriter
                Console.WriteLine($"{DateTime.Now}: Clustering done");

                var clusteredRoutes = phase1Result.Waypoints
                    .Cast<List<Waypoint>>()
                    .Select(wp => wp.Aggregate("", (carry, n) => carry + Environment.NewLine + $"[{n.RealVisitId} {clusteringSolverInputData.VisitNames[n.Visit]}]"));

                routeCalculation.ClusteringResult = "";
                var sb = new StringBuilder();
                var ctr = 0;
                foreach (var route in clusteredRoutes)
                {
                    sb.AppendLine($"Route {ctr.ToString()}:");
                    sb.AppendLine(route);
                    ctr++;
                }

                routeCalculation.ClusteringResult = sb.ToString();
#endregion

                dbSession.Update(routeCalculation);
                dbSession.Flush();

                // ******************************
#region Scheduling
                // ******************************

                var schedulingSovlerVariableBuilders = new List<SchedulingSolverVariableBuilder>();
                foreach (var santa in Enumerable.Range(0, phase1Result.Waypoints.GetLength(0)))
                {
                    foreach (var day in Enumerable.Range(0, phase1Result.Waypoints.GetLength(1)))
                    {
                        var cluster = phase1Result.Waypoints[santa, day];
                        schedulingSovlerVariableBuilders.Add(
                            new SchedulingSolverVariableBuilder(routeCalculation.TimeSliceDuration,
                                new List<Santa> { santas[santa] },
                                visits.Where(v => cluster.Select(w => w.RealVisitId).Contains(v.Id)).ToList(),
                                new List<(DateTime, DateTime)> { routeCalculation.Days[day] }
                            )
                        );

                    }
                }


                routeCalculation.State = RouteCalculationState.RunningPhase2;
                dbSession.Update(routeCalculation);
                dbSession.Flush();
                var inputData = schedulingSovlerVariableBuilders.Where(vb => vb.Visits.Count > 1).Select(vb => vb.Build()).ToList();

                var routeResults = inputData
                    .Select(id => Starter.Optimise(id, TargetBuilderType.Default, routeCalculation.SchedulingMipGap)).ToList();

                routeCalculation.SchedulingResult = Json.Encode(routeResults);
                // gets captured by eventwriter
                Console.WriteLine($"{DateTime.Now}: Scheduling done");
                #endregion
                dbSession.Update(routeCalculation);
                dbSession.Flush();


                routeCalculation.EndTime = DateTime.Now;
                dbSession.Update(routeCalculation);
                dbSession.Flush();
            };
        }

        public void StartWorker()
        {
            bgWorker.RunWorkerAsync();
        }
    }
}