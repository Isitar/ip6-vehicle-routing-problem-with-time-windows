using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;
using IRuettae.Core.Models;

namespace IRuettae.Core.ILP2
{
    public partial class Solver : ISolver
    {
        private readonly OptimizationInput input;
        private readonly int[,] distances;
        private readonly int[] visitDurations;


        public Solver(OptimizationInput input)
        {
            this.input = input;

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

        private GRBVar AccessW(GRBVar[] w, int i, int j)
        {
            return w[i * distances.GetLength(0) + j];
        }

        public OptimizationResult Solve(long timelimitMiliseconds, EventHandler<ProgressReport> progress,
            EventHandler<string> consoleProgress)
        {
            var output = new OptimizationResult
            {
                OptimizationInput = input
            };

            using (var env = new GRBEnv($"{DateTime.Now:yy-MM-dd-HH-mm-ss}_gurobi.log"))
            using (var model = new GRBModel(env))
            {
                var numberOfRoutes = input.Santas.Length * input.Days.Length;
                var v = new GRBVar[numberOfRoutes][];
                var w = new GRBVar[numberOfRoutes][];
                //var c = new GRBVar[numberOfRoutes][,];
                var c = new GRBVar[numberOfRoutes][]; //c_v

                var desiredDuration = new GRBVar[numberOfRoutes][][];
                //var unavailableOverlapPenalty = new GRBVar[numberOfRoutes][][];
                var unavailableDuration = new GRBVar[numberOfRoutes][][];

                for (int s = 0; s < numberOfRoutes; s++)
                {
                    v[s] = new GRBVar[visitDurations.Length];
                    //w[s] = new GRBVar[distances.GetLength(0), distances.GetLength(1)];
                    //c[s] = new GRBVar[distances.GetLength(0), distances.GetLength(1)];
                    c[s] = new GRBVar[visitDurations.Length];
                    //desiredOverlapPenalty[s] = new GRBVar[visitDurations.Length][];
                    desiredDuration[s] = new GRBVar[visitDurations.Length][];
                    unavailableDuration[s] = new GRBVar[visitDurations.Length][];
                    // unavailableOverlapPenalty[s] = new GRBVar[visitDurations.Length][];
                    var (dayStart, dayEnd) = input.Days[s / input.Santas.Length];
                    var dayDuration = dayEnd - dayStart;
                    for (int i = 0; i < v[s].Length; i++)
                    {
                        v[s][i] = model.AddVar(0, 1, 0.0, GRB.BINARY, $"v[{s}][{i}]");
                        c[s][i] = model.AddVar(0, dayDuration, 0, GRB.CONTINUOUS, $"c[{s}][{i}]");

                        if (i > 0)
                        {
                            var visit = input.Visits[i - 1];
                            //desiredOverlapPenalty[s][i] = new GRBVar[visit.Desired.Length];
                            desiredDuration[s][i] = new GRBVar[visit.Desired.Length];
                            unavailableDuration[s][i] = new GRBVar[visit.Unavailable.Length];
                            //unavailableOverlapPenalty[s][i] = new GRBVar[visit.Unavailable.Length];

                            for (int d = 0; d < visit.Desired.Length; d++)
                            {
                                //desiredOverlapPenalty[s][i][d] = model.AddVar(0, dayDuration, 0, GRB.CONTINUOUS,$"desiredOverlapPenalty[{s}][{v}][{d}]");
                                var ub = Math.Max(0, Math.Min(visit.Duration, visit.Desired[d].to - visit.Desired[d].from));
                                desiredDuration[s][i][d] = model.AddVar(0, ub, 0, GRB.CONTINUOUS, $"desiredDuration[{s}][{i}][{d}]");
                            }

                            for (int u = 0; u < visit.Unavailable.Length; u++)
                            {
                                //unavailableOverlapPenalty[s][i][u] = model.AddVar(0, dayDuration, 0, GRB.CONTINUOUS, $"unavailableOverlapPenalty[{s}][{v}][{u}]");
                                var ub = Math.Max(0, Math.Min(visit.Duration, visit.Unavailable[u].to - visit.Unavailable[u].from));
                                unavailableDuration[s][i][u] = model.AddVar(0, ub, 0, GRB.CONTINUOUS, $"unavailableDuration[{s}][{i}][{u}]");
                            }
                        }
                    }

                    w[s] = model.AddVars(distances.GetLength(0) * distances.GetLength(1), GRB.BINARY);
                    //Buffer.BlockCopy(tempWHolder, 0, w[s], 0, tempWHolder.Length * sizeof(Int32));
                    //for (int i = 0; i < distances.GetLength(0); i++)
                    //{
                    //    for (int j = 0; j < distances.GetLength(1); j++)
                    //    {

                    //        w[s][i, j] = tempWHolder[i * distances.GetLength(1) + j];//  model.AddVar(0, 1, 0, GRB.BINARY, null);//$"w[{s}][{i},{j}]");
                    //        //c[s][i, j] = model.AddVar(0, dayDuration, 0, GRB.CONTINUOUS, $"c[{s}][{i},{j}]");

                    //    }
                    //}
                }

                //COnlyOnVisitedWays(model, numberOfRoutes, w, c);
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

                DesiredOverlap(model, numberOfRoutes, v, w, c, desiredDuration);//, desiredOverlapPenalty);
                UnavailableOverlap(model, numberOfRoutes, v, w, c, unavailableDuration);//, unavailableOverlapPenalty);

                
                var maxRoutes = new GRBVar[numberOfRoutes];
                var minRoutes = new GRBVar[numberOfRoutes];
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    var day = s / input.Santas.Length;
                    var (dayStart, dayEnd) = input.Days[day];
                    maxRoutes[s] = model.AddVar(0, dayEnd - dayStart, 0, GRB.CONTINUOUS, $"santa{s} maxRoute");
                    minRoutes[s] = model.AddVar(0, dayEnd - dayStart, 0, GRB.CONTINUOUS, $"santa{s} minRoute");
                }

                FillMaxRoute(model, maxRoutes, c, v);
                FillMinRoutes(model, minRoutes, c);

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


                // TARGET FUNCTION
                var totalWayTime = new GRBLinExpr(0);
                var longestRoute = model.AddVar(0, input.Days.Max(d => d.to - d.from), 0, GRB.CONTINUOUS, "longestRoute");
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    totalWayTime += maxRoutes[s] - minRoutes[s];
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
                            desiredSum += desiredDuration[s][i][d];
                        }

                        for (int u = 0; u < visit.Unavailable.Length; u++)
                        {
                            unavailableSum += unavailableDuration[s][i][u];
                        }
                    }
                }

                model.AddConstr(totalWayTime >= visitDurations.Sum(), null);

                model.SetObjective(
                    +(120d / 3600d) * unavailableSum
                    + (40d / 3600d) * totalWayTime
                    - (20d / 3600d) * desiredSum
                    + (30d / 3600d) * longestRoute
                    , GRB.MINIMIZE);
                model.Parameters.TimeLimit = timelimitMiliseconds / 1000;

                model.Optimize();

                try
                {
                    BuildResult(output, numberOfRoutes, w, c);
                    consoleProgress?.Invoke(this, $"longestRoute: {longestRoute.X}");
                    consoleProgress?.Invoke(this, $"totalWayTime: {totalWayTime.Value}");
                    consoleProgress?.Invoke(this, $"DesiredDuration: {desiredSum.Value}");
                    consoleProgress?.Invoke(this, $"UnavailableDuration: {unavailableSum.Value}");
                }
                catch (Exception e)
                {
                    consoleProgress?.Invoke(this, $"ERROR: {e.Message}");
                    output.Routes = new Route[0];
                }
            }


            return output;
        }


        private void DesiredOverlap(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w, GRBVar[][] c, GRBVar[][][] desiredDuration)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var day = s / input.Santas.Length;
                var (dayStart, dayEnd) = input.Days[day];
                for (int i = 1; i < visitDurations.Length; i++)
                {
                    var visit = input.Visits[i - 1];
                    for (int d = 0; d < visit.Desired.Length; d++)
                    {
                        var (from, to) = visit.Desired[d];
                        // check if desired on day
                        if (to < dayStart || from > dayEnd)
                        {
                            model.AddConstr(desiredDuration[s][i][d] == 0,
                                $"desiredDuration[{s}][{i}][{d}] == 0, outside of day");
                            //model.AddConstr(desiredOverlapPenalty[s][i][d] == 0,$"desiredOverlapPenalty[{s}][{i}][{d}] == 0 outside of day");
                            continue;
                        }

                        var maxDesiredDuration = Math.Min(visit.Duration, to - from);
                        model.AddConstr(desiredDuration[s][i][d] <= maxDesiredDuration * v[s][i], $"desired[{s}][{i}][{d}] only possible if v[{s}][{i}]");

                        var desiredStart = model.AddVar(Math.Max(from, dayStart), dayEnd, 0, GRB.CONTINUOUS, $"desiredStart[{s}][{i}][{d}]");

                        model.AddConstr(desiredStart >= c[s][i] + dayStart, $"desiredStart[{s}[{i}][{d}] >= visitStart");

                        var desiredEnd = model.AddVar(0, to, 0, GRB.CONTINUOUS, $"desiredEnd[{s}][{i}][{d}]");


                        model.AddConstr(desiredEnd <= c[s][i] + visit.Duration * v[s][i] + dayStart, $"desiredEnd[{s}[{i}][{d}] <= visitEnd");
                        var y = model.AddVar(0, 1, 0, GRB.BINARY, null);


                        // if positive, duration = end -start
                        model.AddGenConstrIndicator(y, 0, desiredEnd - desiredStart >= 0, null);
                        model.AddGenConstrIndicator(y, 0, desiredDuration[s][i][d] == desiredEnd - desiredStart, null);
                        // if negative, duration = 0
                        model.AddGenConstrIndicator(y, 1, desiredEnd - desiredStart <= 0, null);
                        model.AddGenConstrIndicator(y, 1, desiredDuration[s][i][d] == 0, null);
                    }
                }
            }
        }

        /// <summary>
        /// Basicly the same as DesiredOverlap but with unavailable
        /// </summary>
        /// <param name="model"></param>
        /// <param name="numberOfRoutes"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <param name="c"></param>
        /// <param name="unavailableDuration"></param>
        private void UnavailableOverlap(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w, GRBVar[][] c, GRBVar[][][] unavailableDuration)//, GRBVar[][][] unavailableOverlapPenalty)
        {

            for (int s = 0; s < numberOfRoutes; s++)
            {
                var day = s / input.Santas.Length;
                var (dayStart, dayEnd) = input.Days[day];

                for (int i = 1; i < visitDurations.Length; i++)
                {
                    var visit = input.Visits[i - 1];
                    for (int d = 0; d < visit.Unavailable.Length; d++)
                    {
                        // check if unavailable is on this day
                        var (from, to) = visit.Unavailable[d];

                        if (to < dayStart || from > dayEnd)
                        {
                            model.AddConstr(unavailableDuration[s][i][d] == 0, $"unavailalbe[{s}][{i}][{d}] == 0, outside of day");
                            //model.AddConstr(unavailableOverlapPenalty[s][i][d] == 0, $"unavailableOverlapPenalty[{s}][{i}][{d}] == 0 outside of day");
                            continue;
                        }

                        var maxUnavailableDuration = Math.Min(visit.Duration, to - from);
                        model.AddConstr(unavailableDuration[s][i][d] <= maxUnavailableDuration * v[s][i], $"unavailable[{s}][{i}][{d}] only possible if v[{s}][{i}]");

                        var unavailableStart = model.AddVar(from, dayEnd, 0, GRB.CONTINUOUS, $"unavailableStart[{s}][{i}][{d}]");
                        var binHelperStart = model.AddVar(0, 1, 0, GRB.BINARY, $"binHelperUnavailableStart[{s}][{i}][{d}]");

                        var visitStart = c[s][i] + dayStart;

                        model.AddConstr(unavailableStart >= visitStart - dayEnd * (1 - v[s][i]), null);
                        model.AddGenConstrIndicator(binHelperStart, 0, unavailableStart <= @from, null);
                        model.AddGenConstrIndicator(binHelperStart, 1, unavailableStart <= visitStart, null);


                        var unavailableEnd = model.AddVar(0, to, 0, GRB.CONTINUOUS, $"unavailableEnd[{s}][{i}][{d}]");
                        var binHelperEnd = model.AddVar(0, 1, 0, GRB.BINARY, $"binHelperUnavailableEnd[{s}][{i}][{d}]");

                        var visitEnd = c[s][i] + dayStart + visit.Duration * v[s][i];

                        model.AddConstr(unavailableEnd <= visitEnd, $"unavailableEnd[{s}[{i}][{d}] <= visitEnd");
                        model.AddGenConstrIndicator(binHelperEnd, 0, unavailableEnd >= to, null);
                        model.AddGenConstrIndicator(binHelperEnd, 1, unavailableEnd >= visitEnd, null);

                        model.AddConstr(
                            unavailableDuration[s][i][d] >= unavailableEnd - unavailableStart,
                            $"unavailable overlap[{s}][{i}][{d}]");
                    }
                }
            }
        }

        private void BuildResult(OptimizationResult output, int numberOfRoutes, GRBVar[][] w, GRBVar[][] c)
        {
            var routes = new List<Route>();

            for (int s = 0; s < numberOfRoutes; s++)
            {
                Console.WriteLine($"Santa {s} uses way");
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
                            Console.WriteLine($"[{i},{j}]=\t{c[s][j].X}");
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
                    return (id: j, startingTime: (int)Math.Ceiling(c[j].X));
                }
            }

            return (-1, -1);
        }

        private void COnlyOnVisitedWays(GRBModel model, int numberOfRoutes, GRBVar[][] w, GRBVar[][] c)
        {
            return; // not needed
        }

        private void IncreasingC(GRBModel model, int numberOfRoutes, GRBVar[][] w, GRBVar[][] c, GRBVar[][] v)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {

                for (int i = 1; i < distances.GetLength(0); i++)
                {
                    model.AddGenConstrIndicator(v[s][i], 0, c[s][i] == 0, null);
                    for (int k = 0; k < distances.GetLength(1); k++)
                    {
                        model.AddGenConstrIndicator(AccessW(w[s], k, i), 1, c[s][i] >= c[s][k] + distances[k, i] + visitDurations[k] + 1, null);
                    }
                }
            }
        }

        private void IncomingOutgoingSantaHome(GRBModel model, int numberOfRoutes, GRBVar[][] w, GRBVar[][] v)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var w0i = new GRBLinExpr(0);
                var wi0 = new GRBLinExpr(0);
                var santaUsed = model.AddVar(0, 1, 0, GRB.BINARY, $"Santa{s} used");
                var santaUsedSum = new GRBLinExpr(0);
                for (int i = 1; i < distances.GetLength(0); i++)
                {
                    w0i += AccessW(w[s], 0, i);
                    wi0 += AccessW(w[s], i, 0);
                    model.AddConstr(santaUsed >= v[s][i], $"santa_used[{s}] >= v[{s}][{i}]");
                    santaUsedSum += v[s][i];
                }

                model.AddConstr(santaUsed <= santaUsedSum, $"santa_used[{s}] <= sum(v[{s}][i])");
                model.AddConstr(w0i == santaUsed, $"way from home santa {s}");
                model.AddConstr(wi0 == santaUsed, $"way to home santa {s}");
            }
        }

        private void IncomingOutgoingSanta(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    var wki = new GRBLinExpr(0);
                    var wik = new GRBLinExpr(0);


                    for (int k = 0; k < distances.GetLength(1); k++)
                    {
                        wki += AccessW(w[s], k, i);
                        wik += AccessW(w[s], i, k);
                    }

                    model.AddConstr(wki == v[s][i], $"if visit {i} is visited by santa {s}, incoming way has to be used");
                    model.AddConstr(wik == v[s][i], $"if visit {i} is visited by santa {s}, outgoing way has to be used");
                }
            }
        }

        private void FillMaxRoute(GRBModel model, GRBVar[] maxRoutes, GRBVar[][] c, GRBVar[][] v)
        {
            for (int s = 0; s < maxRoutes.Length; s++)
            {
                for (int i = 0; i < visitDurations.Length; i++)
                {
                    model.AddConstr(maxRoutes[s] >= c[s][i] + (visitDurations[i] + distances[i, 0]) * v[s][i], null);
                }
            }
        }

        private void FillMinRoutes(GRBModel model, GRBVar[] minRoutes, GRBVar[][] c)
        {
            for (int s = 0; s < minRoutes.Length; s++)
            {
                model.AddGenConstrMin(minRoutes[s], c[s], 0, null);
            }
        }

        private void IncomingOutgoingGlobal(GRBModel model, int numberOfRoutes, GRBVar[][] w)
        {
            for (int i = 1; i < distances.GetLength(0); i++)
            {
                if (input.Visits[i - 1].IsBreak) { continue; }
                var wki = new GRBLinExpr(0);
                var wik = new GRBLinExpr(0);


                for (int s = 0; s < numberOfRoutes; s++)
                {
                    for (int k = 0; k < distances.GetLength(1); k++)
                    {
                        wki += AccessW(w[s], k, i);
                        wik += AccessW(w[s], i, k);
                    }
                }

                model.AddConstr(wki == 1, $"v{i} incoming ways == 1");
                model.AddConstr(wik == 1, $"v{i} outgoing ways == 1");
            }
        }

        private void NumberOfWaysMatchForSanta(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var sumWaysUsed = new GRBLinExpr(0);
                var sumVisitsVisited = new GRBLinExpr(0);
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    sumVisitsVisited += v[s][i];
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        sumWaysUsed += AccessW(w[s], i, j);
                    }
                }

                model.AddConstr(sumVisitsVisited == sumWaysUsed, $"Santa {s} visits visited + home == ways used");
            }
        }

        private void VisitVisitedOnce(GRBModel model, int numberOfRoutes, GRBVar[][] v)
        {
            for (int i = 1; i < visitDurations.Length; i++)
            {
                if (input.Visits[i - 1].IsBreak)
                {
                    continue;
                }

                var sum = new GRBLinExpr(0);
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    sum += v[s][i];
                }

                model.AddConstr(sum == 1, $"visit {i} visited once");

            }
        }

        private void BreakHandling(GRBModel model, int numberOfRoutes, GRBVar[][] v)
        {
            for (int i = 1; i < visitDurations.Length; i++)
            {
                if (!input.Visits[i - 1].IsBreak)
                {
                    continue;
                }

                var visit = input.Visits[i - 1];
                var breakRoutes = new List<int>();
                for (var day = 0; day < input.Days.Length; day++)
                {
                    var s = visit.SantaId + day * input.Santas.Length;
                    model.AddGenConstrOr(v[s][i], v[s].Take(i - 1).Skip(1).ToArray(), null); // assignment if used
                    breakRoutes.Add(s);
                }

                foreach (var nonBreakRoute in Enumerable.Range(0, numberOfRoutes).Except(breakRoutes))
                {
                    model.AddConstr(v[nonBreakRoute][i] == 0, null);
                }
            }
        }

        private void SelfieConstraint(GRBModel model, int numberOfRoutes, GRBVar[][] w)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                // selfies
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    model.AddConstr(AccessW(w[s], i, i) == 0, $"no selfie for {s} {i}");
                }
            }
        }
    }
}