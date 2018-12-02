﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;
using IRuettae.Core.Models;

namespace IRuettae.Core.ILP2
{
    public class Solver : ISolver
    {
        private readonly OptimizationInput input;
        private readonly int[,] distances;
        private readonly int[] visitDurations;
        private readonly int bigM = 100000;

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
            bigM = 100 * input.Days.Max(d => d.to);
        }

        public OptimizationResult Solve(long timelimitMiliseconds, EventHandler<ProgressReport> progress,
            EventHandler<string> consoleProgress)
        {
            var output = new OptimizationResult
            {
                OptimizationInput = input
            };

            using (var env = new GRBEnv("ilp-solver2.log"))
            using (var model = new GRBModel(env))
            {
                var numberOfRoutes = input.Santas.Length * input.Days.Length;
                var v = new GRBVar[numberOfRoutes][];
                var w = new GRBVar[numberOfRoutes][,];
                var c = new GRBVar[numberOfRoutes][,];
                var desiredOverlapPenalty = new GRBVar[numberOfRoutes][][];
                var desiredDuration = new GRBVar[numberOfRoutes][][];
                var unavailableOverlapPenalty = new GRBVar[numberOfRoutes][][];
                var unavailableDuration = new GRBVar[numberOfRoutes][][];

                for (int s = 0; s < numberOfRoutes; s++)
                {
                    v[s] = new GRBVar[visitDurations.Length];
                    w[s] = new GRBVar[distances.GetLength(0), distances.GetLength(1)];
                    c[s] = new GRBVar[distances.GetLength(0), distances.GetLength(1)];
                    desiredOverlapPenalty[s] = new GRBVar[visitDurations.Length][];
                    desiredDuration[s] = new GRBVar[visitDurations.Length][];
                    unavailableDuration[s] = new GRBVar[visitDurations.Length][];
                    unavailableOverlapPenalty[s] = new GRBVar[visitDurations.Length][];

                    for (int i = 0; i < v[s].Length; i++)
                    {
                        v[s][i] = model.AddVar(0, 1, 0.0, GRB.BINARY, $"v[{s}][{i}]");


                        if (i > 0)
                        {
                            var visit = input.Visits[i - 1];
                            desiredOverlapPenalty[s][i] = new GRBVar[visit.Desired.Length];
                            desiredDuration[s][i] = new GRBVar[visit.Desired.Length];
                            unavailableDuration[s][i] = new GRBVar[visit.Unavailable.Length];
                            unavailableOverlapPenalty[s][i] = new GRBVar[visit.Unavailable.Length];

                            for (int d = 0; d < visit.Desired.Length; d++)
                            {
                                desiredOverlapPenalty[s][i][d] = model.AddVar(0, int.MaxValue, 0, GRB.CONTINUOUS,
                                    $"desiredOverlapPenalty[{s}][{v}][{d}]");
                                desiredDuration[s][i][d] = model.AddVar(0, int.MaxValue, 0, GRB.CONTINUOUS,
                                    $"desiredDuration[{s}][{v}][{d}]");
                            }

                            for (int u = 0; u < visit.Unavailable.Length; u++)
                            {
                                unavailableOverlapPenalty[s][i][u] = model.AddVar(0, int.MaxValue, 0, GRB.CONTINUOUS,
                                    $"unavailableOverlapPenalty[{s}][{v}][{u}]");
                                unavailableDuration[s][i][u] = model.AddVar(0, int.MaxValue, 0, GRB.CONTINUOUS,
                                    $"unavailableDuration[{s}][{v}][{u}]");
                            }
                        }
                    }

                    for (int i = 0; i < distances.GetLength(0); i++)
                    {
                        for (int j = 0; j < distances.GetLength(1); j++)
                        {
                            w[s][i, j] = model.AddVar(0, 1, 0, GRB.BINARY, $"w[{s}][{i},{j}]");
                            c[s][i, j] = model.AddVar(0, bigM, 0, GRB.CONTINUOUS, $"c[{s}][{i},{j}]");
                        }
                    }
                }

                COnlyOnVisitedWays(model, numberOfRoutes, w, c);
                SelfieConstraint(model, numberOfRoutes, w);

                // visit visited once
                VisitVisitedOnce(model, numberOfRoutes, v);

                // number of ways = number of visits + home
                NumberOfWaysMatchForSanta(model, numberOfRoutes, v, w);

                // incoming & outgoing constraint
                IncomingOutgoingGlobal(model, numberOfRoutes, w);
                IncomingOutgoingSanta(model, numberOfRoutes, v, w);

                IncomingOutgoingSantaHome(model, numberOfRoutes, w, v);

                IncreasingC(model, numberOfRoutes, w, c);

                DesiredOverlap(model, numberOfRoutes, v, w, c, desiredDuration, desiredOverlapPenalty);
                UnavailableOverlap(model, numberOfRoutes, v, w, c, unavailableDuration, unavailableOverlapPenalty);

                // TARGET FUNCTION

                var maxRoutes = new GRBVar[numberOfRoutes];
                FillMaxRoute(model, maxRoutes, c);

                var minRoutes = new GRBVar[numberOfRoutes];
                FillMinRoutes(model, minRoutes, c);
                var totalWayTime = new GRBLinExpr(0);
                var longestRoute = model.AddVar(0, bigM, 0, GRB.CONTINUOUS, "longestRoute");
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    totalWayTime += maxRoutes[s] - minRoutes[s];
                    model.AddConstr(longestRoute >= maxRoutes[s] - minRoutes[s], $"longesRouteConstr{s}");
                }

                var desiredOverlapPenaltySum = new GRBLinExpr(0);
                var unavailableOverlapPenaltySum = new GRBLinExpr(0);
                var desiredSum = new GRBLinExpr(0);
                var unavailableSum = new GRBLinExpr(0);
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    for (int i = 1; i < visitDurations.Length; i++)
                    {
                        var visit = input.Visits[i - 1];
                        for (int d = 0; d < visit.Desired.Length; d++)
                        {
                            desiredOverlapPenaltySum += desiredOverlapPenalty[s][i][d];
                            desiredSum += desiredDuration[s][i][d];
                        }

                        for (int u = 0; u < visit.Unavailable.Length; u++)
                        {
                            unavailableOverlapPenaltySum += unavailableOverlapPenalty[s][i][u];
                            unavailableSum += unavailableDuration[s][i][u];
                        }
                    }
                }


                model.SetObjective(0
                    + 1000000 * desiredOverlapPenaltySum
                    //+ 1000000 * unavailableOverlapPenaltySum
                    + 120 * unavailableSum
                    + 40 * totalWayTime
                    - 20 * desiredSum
                    + 30 * longestRoute
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
                    consoleProgress?.Invoke(this, $"desiredOverlapPenaltySum: {desiredOverlapPenaltySum.Value}");
                    consoleProgress?.Invoke(this,
                        $"unavailableOverlapPenaltySum: {unavailableOverlapPenaltySum.Value}");
                }
                catch (Exception e)
                {
                    output.Routes = new Route[0];
                }
            }


            return output;
        }

        private void DesiredOverlap(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][,] w, GRBVar[][,] c, GRBVar[][][] desiredDuration, GRBVar[][][] desiredOverlapPenalty)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var day = s / input.Santas.Length;
                for (int i = 1; i < visitDurations.Length; i++)
                {
                    var visit = input.Visits[i - 1];
                    for (int d = 0; d < visit.Desired.Length; d++)
                    {
                        var (from, to) = visit.Desired[d];
                        // check if desired on day
                        if (to < input.Days[day].from || from > input.Days[day].to)
                        {
                            model.AddConstr(desiredDuration[s][i][d] == 0,
                                $"desiredDuration[{s}][{i}][{d}] == 0, outside of day");
                            model.AddConstr(desiredOverlapPenalty[s][i][d] == 0,
                                $"desiredOverlapPenalty[{s}][{i}][{d}] == 0 outside of day");
                            continue;
                        }

                        model.AddConstr(desiredDuration[s][i][d] <= bigM * v[s][i], $"desired[{s}][{i}][{d}] only possible if v[{s}][{i}]");
                        var desiredStart = model.AddVar(0, bigM, 0, GRB.CONTINUOUS, $"desiredStart[{s}][{i}][{d}]");

                        model.AddConstr(desiredStart >= from, $"desiredstart[{s}[{i}][{d}] >= from");
                        var tempSumStart = new GRBLinExpr(0);
                        for (int k = 0; k < visitDurations.Length; k++)
                        {
                            tempSumStart += c[s][k, i];
                            //model.AddConstr(desiredStart >= c[s][k, i], $"desiredstart[{s}[{i}][{d}] >=c[{s}][{k},{i}]");
                        }
                        model.AddConstr(desiredStart >= tempSumStart, $"desiredstart[{s}[{i}][{d}] >= tempsum");

                        var desiredEnd = model.AddVar(0, bigM, 0, GRB.CONTINUOUS, $"desiredEnd[{s}][{i}][{d}]");
                        model.AddConstr(desiredEnd <= to, $"desiredEnd[{s}][{i}][{d}] <= to");
                        var tempSumEnd = new GRBLinExpr(0);
                        for (int k = 0; k < visitDurations.Length; k++)
                        {
                            tempSumEnd += c[s][i, k];
                            //model.AddConstr(desiredEnd <= (c[s][i, k] - visit.Duration) + bigM * (1 - w[s][i, k]), $"desiredEnd[{s}[{i}][{d}] <=c[{s}][{k},{i}]");
                        }
                        model.AddConstr(desiredEnd <= tempSumEnd - visit.Duration, $"desiredEnd[{s}[{i}][{d}] <= tempsum");

                        model.AddConstr(
                            desiredDuration[s][i][d] <= desiredEnd - desiredStart + desiredOverlapPenalty[s][i][d],
                            $"desired overlap[{s}][{i}][{d}]");
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
        /// <param name="unavailableOverlapPenalty"></param>
        private void UnavailableOverlap(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][,] w, GRBVar[][,] c, GRBVar[][][] unavailableDuration, GRBVar[][][] unavailableOverlapPenalty)
        {

            for (int s = 0; s < numberOfRoutes; s++)
            {

                var day = s / input.Santas.Length;

                for (int i = 1; i < visitDurations.Length; i++)
                {
                    var visit = input.Visits[i - 1];
                    for (int d = 0; d < visit.Unavailable.Length; d++)
                    {
                        // check if unavailable is on this day
                        var (from, to) = visit.Unavailable[d];

                        if (to < input.Days[day].from || from > input.Days[day].to)
                        {
                            model.AddConstr(unavailableDuration[s][i][d] == 0,
                                $"unavailalbe[{s}][{i}][{d}] == 0, outside of day");
                            model.AddConstr(unavailableOverlapPenalty[s][i][d] == 0,
                                $"unavailableOverlapPenalty[{s}][{i}][{d}] == 0 outside of day");
                            continue;
                        }


                        model.AddConstr(unavailableDuration[s][i][d] <= bigM * v[s][i], $"unavailable[{s}][{i}][{d}] only possible if v[{s}][{i}]");
                        var unavailableStart = model.AddVar(0, bigM, 0, GRB.CONTINUOUS, $"unavailableStart[{s}][{i}][{d}]");
                        var binHelperStart = model.AddVar(0, 1, 0, GRB.BINARY, $"binHelperUnavailableStart[{s}][{i}][{d}]");
                        model.AddConstr(unavailableStart >= from, $"unavailableStart[{s}[{i}][{d}] >= from");
                        var tempSumStart = new GRBLinExpr(0);
                        for (int k = 0; k < visitDurations.Length; k++)
                        {
                            tempSumStart += c[s][k, i];
                        }
                        model.AddConstr(unavailableStart >= tempSumStart, $"unavailableStart[{s}[{i}][{d}] >= tempsum");
                        model.AddConstr(unavailableStart <= @from + binHelperStart * bigM, "unavailableStart[{s}[{i}][{d}] <= from + bigM*binHelperStart");
                        model.AddConstr(unavailableStart <= tempSumStart + (1-binHelperStart) * bigM, "unavailableStart[{s}[{i}][{d}] <= tempsum + bigM*(1-binHelperStart)");

                        var unavailableEnd = model.AddVar(0, bigM, 0, GRB.CONTINUOUS, $"unavailableEnd[{s}][{i}][{d}]");
                        var binHelperEnd = model.AddVar(0, 1, 0, GRB.BINARY, $"binHelperUnavailableEnd[{s}][{i}][{d}]");
                        model.AddConstr(unavailableEnd <= to, $"unavailableEnd[{s}][{i}][{d}] <= to");
                        var tempSumEnd = new GRBLinExpr(0);
                        for (int k = 0; k < visitDurations.Length; k++)
                        {
                            tempSumEnd += c[s][i, k];
                        }
                        model.AddConstr(unavailableEnd <= tempSumEnd - visit.Duration, $"unavailableEnd[{s}[{i}][{d}] <= tempsum");
                        model.AddConstr(unavailableEnd >= to + binHelperEnd * bigM, "unavailableEnd[{s}[{i}][{d}] <= to + bigM*binHelperStart");
                        model.AddConstr(unavailableEnd >= tempSumStart + (1 - binHelperEnd) * bigM, "unavailableEnd[{s}[{i}][{d}] <= tempsum + bigM*(1-binHelperStart)");

                        model.AddConstr(
                            unavailableDuration[s][i][d] <= unavailableEnd - unavailableStart + unavailableOverlapPenalty[s][i][d],
                            $"unavailable overlap[{s}][{i}][{d}]");
                    }
                }
            }
        }

        private void BuildResult(OptimizationResult output, int numberOfRoutes, GRBVar[][,] w, GRBVar[][,] c)
        {
            var routes = new List<Route>();

            for (int s = 0; s < numberOfRoutes; s++)
            {
                Console.WriteLine($"Santa {s} uses way");
                var route = new Route();
                var wpList = new List<Waypoint>();
                route.SantaId = numberOfRoutes % input.Days.Length;
                var lastId = 0;

                do
                {
                    var (id, startingTime) = GetNextVisit(lastId, w[s], c[s]);
                    wpList.Add(new Waypoint { StartTime = startingTime, VisitId = id - 1 });
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
                        if (Math.Round(w[s][i, j].X, 0) > 0)
                        {
                            Console.WriteLine($"[{i},{j}]=\t{c[s][i, j].X}");
                        }
                    }
                }
            }

            output.Routes = routes.ToArray();
        }

        private (int id, int startingTime) GetNextVisit(int lastVisit, GRBVar[,] w, GRBVar[,] c)
        {
            if (lastVisit == -1)
            {
                return (-1, -1);
            }

            for (int j = 0; j < w.GetLength(1); j++)
            {
                if (Math.Round(w[lastVisit, j].X, 0) > 0)
                {
                    return (id: j, startingTime: (int)Math.Ceiling(c[lastVisit, j].X));
                }
            }

            return (-1, -1);
        }

        private void COnlyOnVisitedWays(GRBModel model, int numberOfRoutes, GRBVar[][,] w, GRBVar[][,] c)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        model.AddConstr(c[s][i, j] <= bigM * w[s][i, j],
                            $"cost only on visited ways c[{s}][{i},{j}]");
                        model.AddConstr(w[s][i, j] <= c[s][i, j], $"visited ways only on cost w[{s}][{i},{j}]");
                    }
                }
            }
        }

        private void IncreasingC(GRBModel model, int numberOfRoutes, GRBVar[][,] w, GRBVar[][,] c)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    var cki = new GRBLinExpr(0);
                    if (i != 0)
                    {
                        for (int k = 0; k < distances.GetLength(1); k++)
                        {
                            cki += c[s][k, i];
                        }
                    }

                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        model.AddConstr(
                            c[s][i, j] >= cki + visitDurations[i] + distances[i, j] -
                            bigM * (1 - w[s][i, j]),
                            $"c[{s}][{i},{j}] bigger than incomming + duration v[{i}] + routecost[{i},{j}]");
                    }

                    // starting times
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        model.AddConstr(c[s][0, j] >= distances[0, j] - bigM * (1 - w[s][0, j]),
                            $"Starting time for c[{s}][0,{j}]");
                    }
                }
            }
        }

        private void IncomingOutgoingSantaHome(GRBModel model, int numberOfRoutes, GRBVar[][,] w, GRBVar[][] v)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var w0i = new GRBLinExpr(0);
                var wi0 = new GRBLinExpr(0);
                var santaUsed = model.AddVar(0, 1, 0, GRB.BINARY, $"Santa{s} used");
                var santaUsedSum = new GRBLinExpr(0);
                for (int i = 1; i < distances.GetLength(0); i++)
                {
                    w0i += w[s][0, i];
                    wi0 += w[s][i, 0];
                    model.AddConstr(santaUsed >= v[s][i], $"santa_used[{s}] >= v[{s}][{i}]");
                    santaUsedSum += v[s][i];
                }

                model.AddConstr(santaUsed <= santaUsedSum, $"santa_used[{s}] <= sum(v[{s}][i])");
                model.AddConstr(w0i == santaUsed, $"way from home santa {s}");
                model.AddConstr(wi0 == santaUsed, $"way to home santa {s}");
            }
        }

        private void IncomingOutgoingSanta(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][,] w)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    var wki = new GRBLinExpr(0);
                    var wik = new GRBLinExpr(0);


                    for (int k = 0; k < distances.GetLength(1); k++)
                    {
                        wki += w[s][k, i];
                        wik += w[s][i, k];
                    }

                    model.AddConstr(wki == v[s][i],
                        $"if visit {i} is visited by santa {s}, incoming way has to be used");
                    model.AddConstr(wik == v[s][i],
                        $"if visit {i} is visited by santa {s}, outgoing way has to be used");
                }
            }
        }

        private void FillMaxRoute(GRBModel model, GRBVar[] maxRoutes, GRBVar[][,] c)
        {
            for (int s = 0; s < maxRoutes.Length; s++)
            {
                maxRoutes[s] = model.AddVar(0, bigM, 0, GRB.CONTINUOUS, $"santa{s} maxRoute");
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        model.AddConstr(maxRoutes[s] >= c[s][i, j], $"maxRoute[{s}] >= c[{s}][{i},{j}]");
                    }
                }

                model.AddConstr(maxRoutes[s] <= input.Days[0].to, $"maxroute{s} day limit");
            }
        }

        private void FillMinRoutes(GRBModel model, GRBVar[] minRoutes, GRBVar[][,] c)
        {
            for (int s = 0; s < minRoutes.Length; s++)
            {
                minRoutes[s] = model.AddVar(0, bigM, 0, GRB.CONTINUOUS, $"santa{s} minRoute");
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        model.AddConstr(minRoutes[s] <= c[s][i, j], $"minRoutes[{s}] <= c[{s}][{i},{j}]");
                    }
                }
            }
        }

        private void IncomingOutgoingGlobal(GRBModel model, int numberOfRoutes, GRBVar[][,] w)
        {
            for (int i = 1; i < distances.GetLength(0); i++)
            {
                var wki = new GRBLinExpr(0);
                var wik = new GRBLinExpr(0);


                for (int s = 0; s < numberOfRoutes; s++)
                {
                    for (int k = 0; k < distances.GetLength(1); k++)
                    {
                        wki += w[s][k, i];
                        wik += w[s][i, k];
                    }
                }

                model.AddConstr(wki == 1, $"v{i} incoming ways == 1");
                model.AddConstr(wik == 1, $"v{i} outgoing ways == 1");
            }
        }

        private void NumberOfWaysMatchForSanta(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][,] w)
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
                        sumWaysUsed += w[s][i, j];
                    }
                }

                model.AddConstr(sumVisitsVisited == sumWaysUsed, $"Santa {s} visits visited + home == ways used");
            }
        }

        private void VisitVisitedOnce(GRBModel model, int numberOfRoutes, GRBVar[][] v)
        {
            for (int i = 1; i < visitDurations.Length; i++)
            {
                var sum = new GRBLinExpr(0);
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    sum += v[s][i];
                }

                model.AddConstr(sum == 1, $"visit {i} visited once");
            }
        }

        private void SelfieConstraint(GRBModel model, int numberOfRoutes, GRBVar[][,] w)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                // selfies
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    model.AddConstr(w[s][i, i] == 0, $"no selfie for {s} {i}");
                }
            }
        }
    }
}