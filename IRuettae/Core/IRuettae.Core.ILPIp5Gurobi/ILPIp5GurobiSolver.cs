using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using IRuettae.Core.ILPIp5Gurobi.Algorithm.Models;
using IRuettae.Core.ILPIp5Gurobi.VariableBuilder;
using IRuettae.Core.Models;
using ResultState = IRuettae.Core.ILPIp5Gurobi.Algorithm.ResultState;
using Route = IRuettae.Core.Models.Route;
using Waypoint = IRuettae.Core.Models.Waypoint;

namespace IRuettae.Core.ILPIp5Gurobi
{
    public class ILPIp5GurobiSolver : ISolver
    {
        private readonly OptimizationInput input;
        private readonly ILPIp5GurobiConfig ip5GurobiConfig;

        public ILPIp5GurobiSolver(OptimizationInput input, ILPIp5GurobiConfig ip5GurobiConfig)
        {
            this.input = input;
            this.ip5GurobiConfig = ip5GurobiConfig;
        }

        public OptimizationResult Solve(long timeLimitMilliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            if (timeLimitMilliseconds < ip5GurobiConfig.ClusteringTimeLimitMiliseconds + ip5GurobiConfig.SchedulingTimeLimitMiliseconds)
            {
                throw new ArgumentException("must be at least the sum of ClusteringTimeLimit and SchedulingTimeLimit", "timeLimitMilliseconds");
            }

            consoleProgress?.Invoke(this, "Solving started");

            var sw = Stopwatch.StartNew();

            var clusteringSolverVariableBuilder = new ClusteringSolverVariableBuilder(input, ip5GurobiConfig.TimeSliceDuration);
            var clusteringSolverInputData = clusteringSolverVariableBuilder.Build();
            var clusteringSolver =
                new Algorithm.Clustering.ClusteringILPSolver(clusteringSolverInputData);


#if WriteMPS && DEBUG
            System.IO.File.WriteAllText($@"C:\Temp\iRuettae\ILP\Clustering\{new Guid()}.mps", clusterinSolver.ExportMPS());
#endif
            var clusteringTimeLimitMiliseconds = ip5GurobiConfig.ClusteringTimeLimitMiliseconds;
            if (clusteringTimeLimitMiliseconds == 0)
            {
                // avoid surpassing timelimit
                clusteringTimeLimitMiliseconds = timeLimitMilliseconds;
            }

            var phase1ResultState = clusteringSolver.Solve(ip5GurobiConfig.ClusteringMIPGap, clusteringTimeLimitMiliseconds);
            if (!(new[] { ResultState.Feasible, ResultState.Optimal }).Contains(phase1ResultState))
            {
                return new OptimizationResult()
                {
                    OptimizationInput = input,
                    Routes = new Route[] { },
                    TimeElapsed = sw.ElapsedMilliseconds / 1000,
                };

            }

            var phase1Result = clusteringSolver.GetResult();
            progress?.Invoke(this, new ProgressReport(0.5));
            consoleProgress?.Invoke(this, "Clustering done");
            consoleProgress?.Invoke(this, $"Clustering Result: {phase1Result}");


            var schedulingSovlerVariableBuilders = new List<SchedulingSolverVariableBuilder>();
            foreach (var santa in Enumerable.Range(0, phase1Result.Waypoints.GetLength(0)))
            {
                foreach (var day in Enumerable.Range(0, phase1Result.Waypoints.GetLength(1)))
                {
                    var cluster = phase1Result.Waypoints[santa, day];
                    var schedulingOptimizationInput = new OptimizationInput
                    {
                        Visits = input.Visits.Where(v => cluster.Select(w => w.Visit - 1).Contains(v.Id)).ToArray(),
                        Santas = new[] { input.Santas[santa] },
                        Days = new[] { input.Days[day] },
                        RouteCosts = input.RouteCosts,
                    };

                    schedulingSovlerVariableBuilders.Add(new SchedulingSolverVariableBuilder(ip5GurobiConfig.TimeSliceDuration, schedulingOptimizationInput, cluster.OrderBy(wp => wp.StartTime).Select(wp => wp.Visit).ToArray()));
                }
            }

            var schedulingInputVariables = schedulingSovlerVariableBuilders
                .Where(vb => vb.Visits != null && vb.Visits.Count > 1)
                .Select(vb => vb.Build());


            var routeResults = schedulingInputVariables
                .AsParallel()
                .Select(schedulingInputVariable =>
                {

                    var schedulingSolver = new SchedulingILPSolver(schedulingInputVariable);

#if WriteMPS && DEBUG
                    System.IO.File.WriteAllText($@"C:\Temp\iRuettae\ILP\Scheduling\{new Guid()}.mps", schedulingSolver.ExportMPS());
#endif


                    var clusteringExtraTime = Math.Max(0, clusteringTimeLimitMiliseconds - sw.ElapsedMilliseconds);
                    var schedulingTimelimitMiliseconds = ip5GurobiConfig.SchedulingTimeLimitMiliseconds + clusteringExtraTime;
                    if (schedulingTimelimitMiliseconds == 0 && timeLimitMilliseconds != 0)
                    {
                        // avoid surpassing timelimit
                        schedulingTimelimitMiliseconds = Math.Max(1, timeLimitMilliseconds - sw.ElapsedMilliseconds);
                    }

                    var schedulingResultState = schedulingSolver.Solve(ip5GurobiConfig.SchedulingMIPGap, schedulingTimelimitMiliseconds);
                    if (!(new[] { ResultState.Feasible, ResultState.Optimal }).Contains(schedulingResultState))
                    {

                        var realWaypointList = new List<Algorithm.Waypoint>();

                        // take presolved and return it
                        for (int i = 0; i < schedulingInputVariable.Presolved.Length; i++)
                        {
                            var i1 = i;
                            var currVisit = input.Visits.FirstOrDefault(v => v.Id == schedulingInputVariable.Presolved[i1] - 1);

                            var timeStamp = schedulingInputVariable.DayStarts[0];
                            if (i > 0)
                            {
                                var lastVisit = input.Visits.FirstOrDefault(v => v.Id == schedulingInputVariable.Presolved[i - 1] - 1);

                                timeStamp = realWaypointList.Last().StartTime + lastVisit.Duration;
                                timeStamp += i > 1
                                    ? input.RouteCosts[lastVisit.Id, currVisit.Id]
                                    : currVisit.WayCostFromHome;
                            }

                            realWaypointList.Add(new Algorithm.Waypoint(currVisit.Equals(default(Visit)) ? Constants.VisitIdHome : currVisit.Id,
                                timeStamp));
                        }
                        var absolutlyLastVisit = input.Visits.FirstOrDefault(v => v.Id == schedulingInputVariable.Presolved[schedulingInputVariable.Presolved.Length - 1] - 1);
                        realWaypointList.Add(new Algorithm.Waypoint(Constants.VisitIdHome, realWaypointList.Last().StartTime + absolutlyLastVisit.Duration + absolutlyLastVisit.WayCostToHome));

                        return new Algorithm.Route(1, 1)
                        {
                            SantaIds = schedulingInputVariable.SantaIds,
                            Waypoints = new[,]
                            {
                                {realWaypointList}
                            }
                        };
                    }

                    var route = schedulingSolver.GetResult();

                    for (int i = 0; i < route.Waypoints.GetLength(0); i++)
                    {
                        for (int j = 0; j < route.Waypoints.GetLength(1); j++)
                        {
                            var realWaypointList = new List<Algorithm.Waypoint>();

                            var waypointList = route.Waypoints[i, j];
                            // copy for later lambda expression
                            var jCopy = j;
                            waypointList.ForEach(wp =>
                            {
                                wp.Visit = wp.Visit == 0
                                    ? Constants.VisitIdHome
                                    : schedulingInputVariable.VisitIds[wp.Visit - 1];
                                wp.StartTime = Math.Max(wp.StartTime, 0);
                                wp.StartTime *= ip5GurobiConfig.TimeSliceDuration;
                                wp.StartTime += schedulingInputVariable.DayStarts[jCopy];
                                realWaypointList.Add(wp);
                            });
                            route.Waypoints[i, j] = realWaypointList;
                        }
                    }
                    return route;
                })
                .ToList();

            progress?.Invoke(this, new ProgressReport(0.99));
            consoleProgress?.Invoke(this, "Scheduling done");
            consoleProgress?.Invoke(this, $"Scheduling Result:{Environment.NewLine}" +
                routeResults.Where(r => r != null).Select(r => r.ToString()).Aggregate((acc, c) => acc + Environment.NewLine + c));

            // construct new output elem
            var optimizationResult = new OptimizationResult()
            {
                OptimizationInput = input,
                Routes = routeResults.Select(r => r != null ? new Route
                {
                    SantaId = r.SantaIds[0],
                    Waypoints = r.Waypoints[0, 0].Select(origWp => new Waypoint
                    {
                        VisitId = origWp.Visit,
                        StartTime = origWp.StartTime
                    }).ToArray(),

                } : new Route()).ToArray(),
            };

            progress?.Invoke(this, new ProgressReport(1));

            // assign total elapsed time
            sw.Stop();
            optimizationResult.TimeElapsed = sw.ElapsedMilliseconds / 1000;
            return optimizationResult;
        }
    }
}
