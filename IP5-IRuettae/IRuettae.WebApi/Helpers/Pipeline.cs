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
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;
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
            routeCalculation.StateText = "";
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
                try
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

                    //var eventTextWriter = new EventTextWriter();
                    //var lastUpdate = DateTime.Now;
                    //eventTextWriter.CharWritten += (o, c) =>
                    //{
                    //    if (null == routeCalculation.StateText)
                    //    {
                    //        routeCalculation.StateText = string.Empty;
                    //    }

                    //    routeCalculation.StateText += c;
                    //    if (DateTime.Now - lastUpdate > TimeSpan.FromMinutes(1))
                    //    {
                    //        lastUpdate = DateTime.Now;
                    //        dbSession.Update(routeCalculation);
                    //        dbSession.Flush();
                    //    }
                    //};

                    //Console.SetOut(eventTextWriter);
                    //Console.SetError(eventTextWriter);


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
                    var serialPath =
                        HostingEnvironment.MapPath(
                            $"~/App_Data/ClusteringSolverInput{routeCalculation.ClusteringOptimisationFunction}_{routeCalculation.NumberOfVisits}Visits.serial");
                    if (serialPath != null)
                    {
                        using (var stream = File.Open(serialPath, FileMode.Create))
                        {
                            new BinaryFormatter().Serialize(stream, clusteringSolverInputData);
                        }
                    }

                    var mpsPath =
                        HostingEnvironment.MapPath($"~/App_Data/Clustering_{routeCalculation.ClusteringOptimisationFunction}_{routeCalculation.NumberOfVisits}.mps");
                    Starter.SaveMps(mpsPath, clusteringSolverInputData, targetType);
#endif

                    var phase1Result = Starter.Optimise(clusteringSolverInputData, targetType,
                        routeCalculation.ClustringMipGap);

                    routeCalculation.StateText += $"{DateTime.Now}: Clustering done{Environment.NewLine}";


                    var clusteredRoutesSb = new StringBuilder();
                    for (int santa = 0; santa < phase1Result.Waypoints.GetLength(0); santa++)
                    {
                        for (int day = 0; day < phase1Result.Waypoints.GetLength(1); day++)
                        {
                            var wp = phase1Result.Waypoints[santa, day].Aggregate(string.Empty,
                                (carry, n) =>
                                    carry + Environment.NewLine +
                                    $"[{n.RealVisitId} {clusteringSolverInputData.VisitNames[n.Visit]}]");
                            clusteredRoutesSb.Append($"Route Santa {santa} on {phase1Result.StartingTime[day]}");
                            clusteredRoutesSb.AppendLine(wp);
                            clusteredRoutesSb.AppendLine(new string('-', 20));
                        }
                    }
                    //var clusteredRoutes = phase1Result.Waypoints
                    //    .Cast<List<Waypoint>>()
                    //    .Select(wp =>  wp.Aggregate("", (carry, n) => carry + Environment.NewLine + $"[{n.RealVisitId} {clusteringSolverInputData.VisitNames[n.Visit]}]"));



                    routeCalculation.ClusteringResult = clusteredRoutesSb.ToString();

                    #endregion Clustering

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
                    var inputData = schedulingSovlerVariableBuilders.Where(vb => vb.Visits.Count > 1)
                        .Select(vb => vb.Build()).ToList();

                    int ctr = 0;
                    var routeResults = inputData
                        .AsParallel()
                        .Select(schedulingInputdata =>
                        {
                            var mpsPathScheduling =
                                HostingEnvironment.MapPath($"~/App_Data/Scheduling_{routeCalculation.Id}_{ctr++}.mps");
                            Starter.SaveMps(mpsPathScheduling, schedulingInputdata, TargetBuilderType.Default);
                            var retVal = new SchedulingResult
                            {
                                Route = Starter.Optimise(schedulingInputdata, TargetBuilderType.Default,
                                    routeCalculation.SchedulingMipGap),
                                StartingTime = schedulingInputdata.DayStartingTimes[0]
                            };
                            retVal.Route.StartingTime = new[] {  retVal.StartingTime };
                            return retVal;

                        }).ToList();

                    routeCalculation.SchedulingResult = JsonConvert.SerializeObject(routeResults);
                    // gets captured by eventwriter
                    routeCalculation.StateText += $"{DateTime.Now}: Scheduling done{Environment.NewLine}";

                    #endregion Scheduling

                    #region metrics

                    foreach (var routeResult in routeResults)
                    {
                        for (int day = 0; day < routeResult.Route.StartingTime.Length; day++)
                        {
                            for (int santa = 0; santa < routeResult.Route.Waypoints.GetLength(0); santa++)
                            {
                                var waypoints = routeResult.Route.Waypoints[santa, day];
                                foreach (var waypoint in waypoints)
                                {
                                    var visit = visits.Where(v => v.Id == waypoint.RealVisitId).First();
                                    var visitStart = routeResult.Route.StartingTime[day].AddSeconds(waypoint.StartTime * routeCalculation.TimeSliceDuration);
                                    var visitEnd = visitStart.AddSeconds(visit.Duration);

                                    foreach (var desired in visit.Desired)
                                    {
                                        // inside
                                        if (visitStart <= desired.Start && visitEnd >= desired.End)
                                        {
                                            routeCalculation.DesiredSeconds += (desired.End - desired.Start).Value.TotalSeconds;
                                        }

                                        // outside
                                        else if (visitStart > desired.Start && visitEnd < desired.End)
                                        {
                                            routeCalculation.DesiredSeconds += (visitEnd - visitStart).TotalSeconds;
                                        }

                                        // left
                                        else if (visitStart > desired.Start && visitEnd >= desired.End)
                                        {
                                            routeCalculation.DesiredSeconds += (desired.End - visitStart).Value.TotalSeconds;
                                        }

                                        // right
                                        else if (visitStart <= desired.Start && visitEnd < desired.End)
                                        {
                                            routeCalculation.DesiredSeconds += (visitEnd - desired.Start).Value.TotalSeconds;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (var routeResult in routeResults)
                    {
                        for (int day = 0; day < routeResult.Route.StartingTime.Length; day++)
                        {
                            for (int santa = 0; santa < routeResult.Route.Waypoints.GetLength(0); santa++)
                            {
                                var waypoints = routeResult.Route.Waypoints[santa, day];
                                var latestVisit = new DateTime().Add(routeResult.Route.StartingTime[day].AddSeconds(waypoints.Max(wp => wp.StartTime) * routeCalculation.TimeSliceDuration).TimeOfDay);
                                if (latestVisit > routeCalculation.LatestVisit)
                                {
                                    routeCalculation.LatestVisit = latestVisit;
                                }
                            }
                        }
                    }

                    routeCalculation.LongestRouteDistance = routeResults.Max(rr =>
                        rr.Route.Waypoints.Cast<List<Waypoint>>().Max(wpl =>
                        {
                            var totalDistance = 0;
                            var lastwp = wpl.First();
                            foreach (var wp in wpl)
                            {
                                totalDistance += dbSession
                                    .Query<Way>()
                                    .Single(w => w.From.Id.Equals(lastwp.RealVisitId) && w.To.Id.Equals(wp.RealVisitId)).Distance;
                            }
                            return totalDistance;
                        }));

                    routeCalculation.LongestRouteTime = routeResults.Max(rr =>
                        rr.Route.Waypoints.Cast<List<Waypoint>>().Max(wpl =>
                        {
                            var totalDuration = 0;
                            var lastwp = wpl.First();
                            foreach (var wp in wpl)
                            {
                                totalDuration += dbSession
                                    .Query<Way>()
                                    .Single(w => w.From.Id.Equals(lastwp.RealVisitId) && w.To.Id.Equals(wp.RealVisitId)).Duration;
                            }
                            return totalDuration;
                        }));

                    routeCalculation.TotalWaytime = routeResults.Sum(rr =>
                        rr.Route.Waypoints.Cast<List<Waypoint>>().Max(wpl =>
                        {
                            var totalDuration = 0;
                            var lastwp = wpl.First();
                            foreach (var wp in wpl)
                            {
                                totalDuration += dbSession
                                    .Query<Way>()
                                    .Single(w => w.From.Id.Equals(lastwp.RealVisitId) && w.To.Id.Equals(wp.RealVisitId)).Duration;
                            }
                            return totalDuration;
                        }));

                    routeCalculation.WaytimePerSanta = routeCalculation.TotalWaytime / routeResults.Sum(rr => rr.Route.Waypoints.Length);
                    #endregion metrics

                    dbSession.Update(routeCalculation);
                    dbSession.Flush();


                    routeCalculation.Result = routeCalculation.SchedulingResult;
                    routeCalculation.State = RouteCalculationState.Finished;

                    routeCalculation.EndTime = DateTime.Now;
                    dbSession.Update(routeCalculation);
                    dbSession.Flush();
                }
                catch (Exception e)
                {
                    routeCalculation.State = RouteCalculationState.Cancelled;
                    routeCalculation.StateText += "Error: " + e.Message;
                    dbSession.Update(routeCalculation);
                    dbSession.Flush();
                }
            };
        }

        public void StartWorker()
        {
            bgWorker.RunWorkerAsync();
        }
    }
}