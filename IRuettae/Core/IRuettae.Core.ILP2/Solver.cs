using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;
using IRuettae.Core.ILP2.VRPSolver;
using IRuettae.Core.Models;

namespace IRuettae.Core.ILP2
{
    public partial class Solver : ISolver
    {
        private readonly OptimizationInput input;
        private double vrpTimeLimitFactor;
        private readonly int[,] distances;
        private readonly int[] visitDurations;

        /// <summary>
        /// Creates a new solver
        /// </summary>
        /// <param name="input"></param>
        /// <param name="vrpTimeLimitFactor"></param>
        public Solver(OptimizationInput input, double vrpTimeLimitFactor)
        {
            this.input = input;
            this.vrpTimeLimitFactor = vrpTimeLimitFactor;

            visitDurations = input.Visits.Select(v => v.Duration).Prepend(0).ToArray();
            distances = new int[input.Visits.Length + 1, input.Visits.Length + 1];
            for (int i = 1; i < distances.GetLength(0); i++)
            {
                for (int j = 1; j < distances.GetLength(1); j++)
                {
                    distances[i, j] = input.RouteCosts[i - 1, j - 1];
                }
            }

            for (int i = 1; i < distances.GetLength(0); i++)
            {
                distances[i, 0] = input.Visits[i - 1].WayCostToHome;
                distances[0, i] = input.Visits[i - 1].WayCostFromHome;
            }

            distances[0, 0] = 0;
        }


        /// <summary>
        /// 2 dimensional array access for one dimensional arr
        /// </summary>
        /// <param name="w">the arr</param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>grbvar at w[i*dist + j]</returns>
        private GRBVar AccessW(GRBVar[] w, int i, int j)
        {
            return w[i * distances.GetLength(0) + j];
        }

        public OptimizationResult Solve(long timelimitMiliseconds, EventHandler<ProgressReport> progress,
            EventHandler<string> consoleProgress)
        {
            var sw = Stopwatch.StartNew();
            var output = new OptimizationResult
            {
                OptimizationInput = input
            };

            var timeWindowIsRelevant = !input.Visits.All(visit =>
            {
                if (visit.Desired.Length > 0) return false;

                return !visit.Unavailable.Any(unavailable =>
                {
                    var (unavailableFrom, unavailableTo) = unavailable;
                    foreach (var (dayFrom, dayTo) in input.Days)
                    {
                        if (IntersectionLength(unavailableFrom, unavailableTo, dayFrom, dayTo) > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                });
            });

            // first solve vrp, take result as initial solution.
            if (!timeWindowIsRelevant)
            {
                vrpTimeLimitFactor = 1;
            }

            var vrpSolution = input.Visits.Length > 75 ? FakeVRPSolution(input.Santas.Length * input.Days.Length) : new VRPCallbackSolver(input).SolveVRP((int)(timelimitMiliseconds * vrpTimeLimitFactor));

            consoleProgress?.Invoke(this, $"vrp needed {sw.ElapsedMilliseconds}ms, remaining {timelimitMiliseconds - sw.ElapsedMilliseconds}");

            if (!timeWindowIsRelevant)
            {
                BuildResultFromVRP(output, vrpSolution);
                output.TimeElapsed = sw.ElapsedMilliseconds / 1000;
                return output;
            }

            timelimitMiliseconds -= sw.ElapsedMilliseconds;
            using (var env = new GRBEnv($"{DateTime.Now:yy-MM-dd-HH-mm-ss}_gurobi.log"))
            using (var model = new GRBModel(env))
            {
                #region initialize Variables
                var numberOfRoutes = input.Santas.Length * input.Days.Length;
                var v = new GRBVar[numberOfRoutes][]; // [santa] visits [visit]
                var w = new GRBVar[numberOfRoutes][]; // [santa] uses [way]
                var c = new GRBVar[numberOfRoutes][]; // [santa] visits visit at the end of [way]

                var desiredDuration = new GRBVar[numberOfRoutes][][];
                var unavailableDuration = new GRBVar[numberOfRoutes][][];

                var maxRoutes = new GRBVar[numberOfRoutes];
                var minRoutes = new GRBVar[numberOfRoutes];

                for (int s = 0; s < numberOfRoutes; s++)
                {
                    v[s] = new GRBVar[visitDurations.Length];
                    c[s] = new GRBVar[visitDurations.Length];
                    desiredDuration[s] = new GRBVar[visitDurations.Length][];
                    unavailableDuration[s] = new GRBVar[visitDurations.Length][];
                    var (dayStart, dayEnd) = input.Days[s / input.Santas.Length];
                    var dayDuration = dayEnd - dayStart;
                    for (int i = 0; i < v[s].Length; i++)
                    {
                        v[s][i] = model.AddVar(0, 1, 0.0, GRB.BINARY, GurobiVarName($"v[{s}][{i}]"));
                        c[s][i] = model.AddVar(0, dayDuration, 0, GRB.CONTINUOUS, GurobiVarName($"c[{s}][{i}]"));

                        if (i > 0)
                        {
                            var visit = input.Visits[i - 1];
                            desiredDuration[s][i] = new GRBVar[visit.Desired.Length];
                            unavailableDuration[s][i] = new GRBVar[visit.Unavailable.Length];

                            for (int d = 0; d < visit.Desired.Length; d++)
                            {
                                var ub = Math.Max(0, Math.Min(visit.Duration, visit.Desired[d].to - visit.Desired[d].from));
                                desiredDuration[s][i][d] = model.AddVar(0, ub, 0, GRB.CONTINUOUS, GurobiVarName($"desiredDuration[{s}][{i}][{d}]"));
                            }

                            for (int u = 0; u < visit.Unavailable.Length; u++)
                            {
                                var ub = Math.Max(0, Math.Min(visit.Duration, visit.Unavailable[u].to - visit.Unavailable[u].from));
                                unavailableDuration[s][i][u] = model.AddVar(0, ub, 0, GRB.CONTINUOUS, GurobiVarName($"unavailableDuration[{s}][{i}][{u}]"));
                            }
                        }
                    }

                    w[s] = model.AddVars(distances.GetLength(0) * distances.GetLength(1), GRB.BINARY);
                    maxRoutes[s] = model.AddVar(0, dayEnd - dayStart, 0, GRB.CONTINUOUS, GurobiVarName($"santa{s} maxRoute"));
                    minRoutes[s] = model.AddVar(0, dayEnd - dayStart, 0, GRB.CONTINUOUS, GurobiVarName($"santa{s} minRoute"));
                }

                #endregion initialize Variables


                #region add constraints
                SelfieConstraint(model, numberOfRoutes, w);

                // visit visited once
                VisitVisitedOnce(model, numberOfRoutes, v);
                // breaks
                BreakHandling(model, numberOfRoutes, v);

                // number of ways = number of visits + home
                NumberOfWaysMatchForSanta(model, numberOfRoutes, v, w);

                // incoming & outgoing constraint
                IncomingOutgoingGlobal(model, numberOfRoutes, w);
                IncomingOutgoingSanta(model, numberOfRoutes, v, w);

                IncomingOutgoingSantaHome(model, numberOfRoutes, w, v);

                IncreasingC(model, numberOfRoutes, w, c, v);

                DesiredOverlap(model, numberOfRoutes, v, w, c, desiredDuration);

                UnavailableOverlap(model, numberOfRoutes, v, w, c, unavailableDuration, true);

                FillMaxRoute(model, maxRoutes, c, v);
                FillMinRoutes(model, minRoutes, c, v);
                MinRouteSmallerThanMaxRoute(model, minRoutes, maxRoutes);

                // Symmetry breaking constraint if no breaks
                if (!input.Visits.Any(visit => visit.IsBreak))
                {
                    for (int d = 0; d < input.Days.Length; d++)
                    {
                        var dayOffset = d * input.Santas.Length;
                        for (int s = 1; s < input.Santas.Length; s++)
                        {
                            model.AddConstr(maxRoutes[dayOffset + s] - minRoutes[dayOffset + s] <= maxRoutes[dayOffset + s - 1] - minRoutes[dayOffset + s - 1], null);
                        }
                    }
                }

                #endregion add constraints

                // TARGET FUNCTION
                var totalWayTime = new GRBLinExpr(0);
                var longestRoute = model.AddVar(0, input.Days.Max(d => d.to - d.from), 0, GRB.CONTINUOUS, "longestRoute");
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    totalWayTime += (maxRoutes[s] - minRoutes[s]);
                    model.AddConstr(longestRoute >= maxRoutes[s] - minRoutes[s], $"longesRouteConstr{s}");
                }

                var desiredSum = new GRBLinExpr(0);
                var unavailableSum = new GRBLinExpr(0);
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    for (int i = 1; i < visitDurations.Length; i++)
                    {
                        var visit = input.Visits[i - 1];
                        for (int d = 0; d < visit.Desired.Length; d++)
                        {
                            if (!(desiredDuration[s][i][d] is null))
                            { desiredSum += desiredDuration[s][i][d]; }
                        }

                        for (int u = 0; u < visit.Unavailable.Length; u++)
                        {
                            if (!(unavailableDuration[s][i][u] is null))
                            { unavailableSum += unavailableDuration[s][i][u]; }
                        }
                    }
                }

                LowerBoundTotalWaytime(model, totalWayTime);

                model.SetObjective(
                    +(12d) * unavailableSum
                    + (4d) * totalWayTime
                    - (2d) * desiredSum
                    + (3d) * longestRoute
                    , GRB.MINIMIZE);
                model.Parameters.TimeLimit = Math.Max(0, timelimitMiliseconds / 1000);
                InitializeWithVRPSolution(vrpSolution, numberOfRoutes, model, v, w, c);
                model.Optimize();
                output.TimeElapsed = sw.ElapsedMilliseconds / 1000;
                try
                {
                    if (model.SolCount == 0 && vrpSolution != null)
                    {
                        consoleProgress?.Invoke(this, "No solution found -> try with vrpsolution");
                        BuildResultFromVRP(output, vrpSolution);
                        return output;
                    }
                    BuildResult(output, numberOfRoutes, w, c);
                    consoleProgress?.Invoke(this, $"longestRoute: {longestRoute.X}");
                    consoleProgress?.Invoke(this, $"totalWayTime: {totalWayTime.Value}");
                    consoleProgress?.Invoke(this, $"DesiredDuration: {desiredSum.Value}");
                    consoleProgress?.Invoke(this, $"UnavailableDuration: {unavailableSum.Value}");
                    for (int s = 0; s < numberOfRoutes; s++)
                    {
                        consoleProgress?.Invoke(this, $"maxRoutes[{s}]: {maxRoutes[s].X}, {minRoutes[s].X} , ->{maxRoutes[s].X - minRoutes[s].X}");
                        for (int visitIndex = 0; visitIndex < visitDurations.Length; visitIndex++)
                        {
                            consoleProgress?.Invoke(this, $"c[{s}][{visitIndex}] ({v[s][visitIndex].X}): {c[s][visitIndex].X}");
                        }
                    }
                }
                catch (Exception e)
                {
                    consoleProgress?.Invoke(this, $"ERROR: {e.Message}");
                    output.Routes = new Route[0];
                }
            }

            return output;
        }

        private List<int[]> FakeVRPSolution(int numberOfRoutes)
        {
            var visitSequences = new List<int[]>();
            var rest = input.Visits.Count(visit => !visit.IsBreak) % numberOfRoutes;
            var visitsPerRoute = input.Visits.Count(visit => !visit.IsBreak) / numberOfRoutes;
            var visitIndex = 1;
            for (var day = 0; day < input.Days.Length; day++)
            {
                for (var santa = 0; santa < input.Santas.Length; santa++)
                {
                    var visitSequence = new List<int> { 0 };
                    var s = input.Santas.Length * day + santa;
                    foreach (var breakVisit in input.Visits.Where(visit => visit.IsBreak && (visit.SantaId == santa)))
                    {
                        visitSequence.Add(breakVisit.Id + 1);
                    }

                    for (var i = 0; i < visitsPerRoute; i++)
                    {
                        visitSequence.Add(visitIndex);
                        visitIndex++;
                    }

                    if (day + 1 == input.Days.Length && santa + 1 == input.Santas.Length)
                    {
                        for (int i = 0; i < rest; i++)
                        {
                            visitSequence.Add(visitIndex);
                            visitIndex++;
                        }
                    }

                    visitSequence.Add(0);

                    visitSequences.Add(visitSequence.ToArray());
                }
            }

            return visitSequences;
        }

        private void BuildResultFromVRP(OptimizationResult output, List<int[]> vrpSolution)
        {
            var routes = new List<Route>();
            for (int s = 0; s < vrpSolution.Count; s++)
            {
                var route = new Route { SantaId = s % input.Santas.Length };
                var wpList = new List<Waypoint>();

                var day = s / input.Santas.Length;
                var lastVisitId = -1;
                foreach (var visitId in vrpSolution[s])
                {
                    if (visitId == 0)
                    {
                        wpList.Add(new Waypoint { StartTime = input.Days[day].from, VisitId = -1 });
                    }
                    else
                    {
                        wpList.Add(new Waypoint
                        {
                            StartTime = wpList.Last().StartTime + visitDurations[lastVisitId] + distances[lastVisitId, visitId],
                            VisitId = visitId - 1
                        });
                    }
                    lastVisitId = visitId;
                }

                if (wpList.Count == 0)
                {
                    continue;
                }
                wpList.Add(new Waypoint
                {
                    StartTime = wpList.Last().StartTime + visitDurations[lastVisitId] + distances[lastVisitId, 0],
                    VisitId = -1
                });
                route.Waypoints = wpList.ToArray();

                routes.Add(route);
            }

            output.Routes = routes.ToArray();
        }

        private void InitializeWithVRPSolution(List<int[]> vrpSolution, int numberOfRoutes, GRBModel model, GRBVar[][] v, GRBVar[][] w, GRBVar[][] c)
        {

            if (vrpSolution == null)
            {
                return;
            }
            for (int s = 0; s < numberOfRoutes; s++)
            {
                if (vrpSolution[s].Length == 1)
                {
                    // initialize with 0
                    for (int visitIndex = 0; visitIndex < visitDurations.Length; visitIndex++)
                    {
                        v[s][visitIndex].Start = 0;
                        c[s][visitIndex].Start = 0;
                    }
                    for (int i = 0; i < distances.GetLength(0); i++)
                    {
                        for (int j = 0; j < distances.GetLength(0); j++)
                        {
                            AccessW(w[s], i, j).Start = 0;
                        }
                    }
                    continue;
                }

                // initialize v[s]
                for (int visitIndex = 0; visitIndex < visitDurations.Length; visitIndex++)
                {
                    v[s][visitIndex].Start = vrpSolution[s].Contains(visitIndex) ? 1 : 0;
                }

                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    for (int j = 0; j < distances.GetLength(0); j++)
                    {
                        AccessW(w[s], i, j).Start = 0;
                    }
                }

                var lastVisit = 0;
                var lastTimeStamp = 0;
                for (int i = 0; i < vrpSolution[s].Length; i++)
                {
                    var currVisit = vrpSolution[s][i];
                    if (lastVisit != currVisit)
                    {
                        AccessW(w[s], lastVisit, currVisit).Start = 1;

                        lastTimeStamp = lastTimeStamp + visitDurations[lastVisit] + distances[lastVisit, currVisit];
                        c[s][currVisit].Start = lastTimeStamp;
                    }

                    lastVisit = currVisit;
                }

                if (lastVisit != 0)
                {
                    AccessW(w[s], lastVisit, 0).Start = 1;
                }
            }
        }

        private void BuildResult(OptimizationResult output, int numberOfRoutes, GRBVar[][] w, GRBVar[][] c)
        {
            var routes = new List<Route>();

            for (int s = 0; s < numberOfRoutes; s++)
            {
                Debug.WriteLine($"Santa {s} uses way");
                var route = new Route();
                var wpList = new List<Waypoint>();
                route.SantaId = s % input.Santas.Length;
                var lastId = 0;
                var day = s / input.Santas.Length;

                do
                {
                    var (id, startingTime) = GetNextVisit(lastId, w[s], c[s]);
                    if (id == 0)
                    {
                        // if last visit
                        var lastVisit = input.Visits[lastId - 1];
                        wpList.Add(new Waypoint { StartTime = wpList.Last().StartTime + lastVisit.Duration + lastVisit.WayCostToHome, VisitId = id - 1 });
                    }
                    else
                    {
                        wpList.Add(new Waypoint { StartTime = startingTime + input.Days[day].from, VisitId = id - 1 });
                    }

                    lastId = id;
                } while (lastId != 0 && lastId != -1);

                if (wpList.Count == 0 || lastId == -1)
                {
                    continue;
                }

                var firstWaypoint = wpList.First();
                var firstVisit = input.Visits[firstWaypoint.VisitId];
                route.Waypoints = wpList
                    .Prepend(new Waypoint
                    {
                        StartTime = firstWaypoint.StartTime - firstVisit.WayCostFromHome,
                        VisitId = Constants.VisitIdHome
                    })
                    .ToArray();

                routes.Add(route);
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        if (Math.Round(AccessW(w[s], i, j).X, 0) > 0)
                        {
                            Debug.WriteLine($"[{i},{j}]=\t{c[s][j].X}");
                        }
                    }
                }
            }

            output.Routes = routes.ToArray();
        }

        private (int id, int startingTime) GetNextVisit(int lastVisit, GRBVar[] w, GRBVar[] c)
        {
            if (lastVisit == -1)
            {
                return (-1, -1);
            }

            for (int j = 0; j < distances.GetLength(1); j++)
            {
                if (Math.Round(AccessW(w, lastVisit, j).X, 0) > 0)
                {
                    return (id: j, startingTime: (int)Math.Round(c[j].X, 0));
                }
            }

            return (-1, -1);
        }


        /// <summary>
        /// Returns how much the two intervals overlap
        /// </summary>
        /// <param name="start1">start of first interval</param>
        /// <param name="end1">end of first interval</param>
        /// <param name="start2">start of second interval</param>
        /// <param name="end2">end of second interval</param>
        /// <returns></returns>
        private static int IntersectionLength(int start1, int end1, int start2, int end2)
        {
            int startIntersection = Math.Max(start1, start2);
            int endIntersection = Math.Min(end1, end2);
            if (startIntersection < endIntersection)
            {
                return endIntersection - startIntersection;
            }
            return 0;
        }
    }
}