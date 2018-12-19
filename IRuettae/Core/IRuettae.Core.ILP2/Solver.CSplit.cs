using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;
using IRuettae.Core.Models;

namespace IRuettae.Core.ILP2
{
    public partial class Solver
    {
        private void DesiredOverlap(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w, GRBVar[][,] c, GRBVar[][][] desiredDuration)//, GRBVar[][][] desiredOverlapPenalty)
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
                        //model.AddConstr(desiredDuration[s][i][d] <= to - from, $"desired[{s}][{i}][{d}] <= to-from");

                        var desiredStart = model.AddVar(from, to, 0, GRB.CONTINUOUS, $"desiredStart[{s}][{i}][{d}]");


                        var tempSumStart = new GRBLinExpr(0);
                        for (int k = 0; k < visitDurations.Length; k++)
                        {
                            tempSumStart += c[s][k, i];
                        }

                        tempSumStart += dayStart;

                        model.AddConstr(desiredStart >= tempSumStart, $"desiredstart[{s}[{i}][{d}] >= tempsum");

                        var desiredEnd = model.AddVar(dayStart, to, 0, GRB.CONTINUOUS, $"desiredEnd[{s}][{i}][{d}]");
                        //model.AddConstr(desiredEnd <= to, $"desiredEnd[{s}][{i}][{d}] <= to");
                        var tempSumEnd = tempSumStart + visitDurations[i];
                        //new GRBLinExpr(0);
                        //for (int k = 0; k < visitDurations.Length; k++)
                        //{
                        //    tempSumEnd += c[s][i, k] - w[s][i, k] * distances[i, k];
                        //}

                        //tempSumEnd += dayStart;
                        model.AddConstr(desiredEnd <= tempSumEnd, $"desiredEnd[{s}[{i}][{d}] <= tempsum");
                        //model.AddConstr(desiredEnd >= desiredStart, $"desiredend>=desiredstart [{s}][{i}][{d}]");
                        //model.AddConstr(desiredDuration[s][i][d] <= desiredEnd - desiredStart + desiredOverlapPenalty[s][i][d],$"desired overlap[{s}][{i}][{d}]");
                        var y = model.AddVar(0, 1, 0, GRB.BINARY, null);



                        // if positive, overlappenalty = 0
                        model.AddGenConstrIndicator(y, 0, desiredEnd - desiredStart >= 0, null);
                        //model.AddGenConstrIndicator(y, 0, 0 == desiredOverlapPenalty[s][i][d], null);
                        model.AddGenConstrIndicator(y, 0, desiredDuration[s][i][d] == desiredEnd - desiredStart, null);
                        // if negative, overlap possible
                        model.AddGenConstrIndicator(y, 1, desiredEnd - desiredStart <= 0, null);
                        model.AddGenConstrIndicator(y, 1, desiredDuration[s][i][d] == 0, null);

                        //model.AddConstr(desiredOverlapPenalty[s][i][d] <= desiredStart, $"desiredPenalty[{s}][{i}][{d}] upperbound");
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
        private void UnavailableOverlap(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w, GRBVar[][,] c, GRBVar[][][] unavailableDuration, GRBVar[][][] unavailableOverlapPenalty)
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
                            model.AddConstr(unavailableDuration[s][i][d] == 0,
                                $"unavailalbe[{s}][{i}][{d}] == 0, outside of day");
                            model.AddConstr(unavailableOverlapPenalty[s][i][d] == 0,
                                $"unavailableOverlapPenalty[{s}][{i}][{d}] == 0 outside of day");
                            continue;
                        }

                        var maxUnavailableDuration = Math.Min(visit.Duration, to - from);
                        model.AddConstr(unavailableDuration[s][i][d] <= maxUnavailableDuration * v[s][i], $"unavailable[{s}][{i}][{d}] only possible if v[{s}][{i}]");
                        //model.AddConstr(unavailableDuration[s][i][d] <= to - from, $"unavailable[{s}][{i}][{d}] <= to-from");

                        var unavailableStart = model.AddVar(from, dayEnd, 0, GRB.CONTINUOUS, $"unavailableStart[{s}][{i}][{d}]");
                        var binHelperStart = model.AddVar(0, 1, 0, GRB.BINARY, $"binHelperUnavailableStart[{s}][{i}][{d}]");
                        //model.AddConstr(unavailableStart >= from, $"unavailableStart[{s}[{i}][{d}] >= from");
                        var tempSumStart = new GRBLinExpr(0);
                        for (int k = 0; k < visitDurations.Length; k++)
                        {
                            tempSumStart += c[s][k, i];
                        }

                        tempSumStart += dayStart;
                        model.AddConstr(unavailableStart >= tempSumStart, $"unavailableStart[{s}[{i}][{d}] >= tempsum");
                        model.AddGenConstrIndicator(binHelperStart, 0, unavailableStart <= @from, null);
                        model.AddGenConstrIndicator(binHelperStart, 1, unavailableStart <= tempSumStart, null);


                        //model.AddConstr(unavailableStart <= @from + binHelperStart * bigM, "unavailableStart[{s}[{i}][{d}] <= from + bigM*binHelperStart");
                        //model.AddConstr(unavailableStart <= tempSumStart + (1 - binHelperStart) * bigM, "unavailableStart[{s}[{i}][{d}] <= tempsum + bigM*(1-binHelperStart)");

                        var unavailableEnd = model.AddVar(dayStart, to, 0, GRB.CONTINUOUS, $"unavailableEnd[{s}][{i}][{d}]");
                        var binHelperEnd = model.AddVar(0, 1, 0, GRB.BINARY, $"binHelperUnavailableEnd[{s}][{i}][{d}]");
                        //model.AddConstr(unavailableEnd <= to, $"unavailableEnd[{s}][{i}][{d}] <= to");
                        var tempSumEnd = new GRBLinExpr(0);
                        for (int k = 0; k < visitDurations.Length; k++)
                        {
                            tempSumEnd += c[s][i, k] - AccessW(w[s], i, k) * distances[i, k];
                        }
                        tempSumEnd += dayStart;

                        model.AddConstr(unavailableEnd <= tempSumEnd, $"unavailableEnd[{s}[{i}][{d}] <= tempsum");
                        model.AddGenConstrIndicator(binHelperEnd, 0, unavailableEnd >= to, null);
                        model.AddGenConstrIndicator(binHelperEnd, 1, unavailableEnd >= tempSumEnd, null);
                        //model.AddConstr(unavailableEnd >= to - binHelperEnd * bigM, "unavailableEnd[{s}[{i}][{d}] <= to - bigM*binHelperStart");
                        //model.AddConstr(unavailableEnd >= tempSumEnd - (1 - binHelperEnd) * bigM, "unavailableEnd[{s}[{i}][{d}] <= tempsum - bigM*(1-binHelperStart)");

                        model.AddConstr(
                            unavailableDuration[s][i][d] >= unavailableEnd - unavailableStart,
                            $"unavailable overlap[{s}][{i}][{d}]");
                    }
                }
            }
        }



        private void BuildResult(OptimizationResult output, int numberOfRoutes, GRBVar[][] w, GRBVar[][,] c)
        {
            var routes = new List<Route>();

            for (int s = 0; s < numberOfRoutes; s++)
            {
                Console.WriteLine($"Santa {s} uses way");
                var route = new Route();
                var wpList = new List<Waypoint>();
                route.SantaId = s % input.Days.Length;
                var lastId = 0;
                var day = s / input.Santas.Length;

                do
                {
                    var (id, startingTime) = GetNextVisit(lastId, w[s], c[s]);
                    wpList.Add(new Waypoint { StartTime = startingTime + input.Days[day].from, VisitId = id - 1 });
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
                            Console.WriteLine($"[{i},{j}]=\t{c[s][i, j].X}");
                        }
                    }
                }
            }

            output.Routes = routes.ToArray();
        }

        private (int id, int startingTime) GetNextVisit(int lastVisit, GRBVar[] w, GRBVar[,] c)
        {
            if (lastVisit == -1)
            {
                return (-1, -1);
            }

            for (int j = 0; j < w.GetLength(1); j++)
            {
                if (Math.Round(AccessW(w, lastVisit, j).X, 0) > 0)
                {
                    return (id: j, startingTime: (int)Math.Ceiling(c[lastVisit, j].X));
                }
            }

            return (-1, -1);
        }


        private void COnlyOnVisitedWays(GRBModel model, int numberOfRoutes, GRBVar[][] w, GRBVar[][,] c)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                (var dayStart, var dayEnd) = input.Days[s / input.Santas.Length];
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        model.AddConstr(c[s][i, j] <= (dayEnd - dayStart) * AccessW(w[s], i, j), $"cost only on visited ways c[{s}][{i},{j}]");
                        model.AddConstr(AccessW(w[s], i, j) <= c[s][i, j], $"visited ways only on cost w[{s}][{i},{j}]");
                    }
                }
            }
        }

        private void IncreasingC(GRBModel model, int numberOfRoutes, GRBVar[][] w, GRBVar[][,] c)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var day = s / input.Santas.Length;
                (var daystart, var dayend) = input.Days[day];
                var dayDuration = dayend - daystart;
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
                            c[s][i, j] >= cki + visitDurations[i] + distances[i, j] - dayDuration * (1 - AccessW(w[s], i, j)),
                            $"c[{s}][{i},{j}] bigger than incomming + duration v[{i}] + routecost[{i},{j}]");
                    }

                    // starting times
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        model.AddConstr(c[s][0, j] >= distances[0, j] - dayDuration * (1 - AccessW(w[s], i, j)),
                            $"Starting time for c[{s}][0,{j}]");
                    }
                }
            }
        }

        private void FillMaxRoute(GRBModel model, GRBVar[] maxRoutes, GRBVar[][,] c)
        {
            for (int s = 0; s < maxRoutes.Length; s++)
            {
                var day = s / input.Santas.Length;
                (var dayStart, var dayEnd) = input.Days[day];
                maxRoutes[s] = model.AddVar(0, dayEnd, 0, GRB.CONTINUOUS, $"santa{s} maxRoute");
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
                minRoutes[s] = model.AddVar(0, input.Days.Max(d => d.to - d.from), 0, GRB.CONTINUOUS, $"santa{s} minRoute");
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        model.AddConstr(minRoutes[s] <= c[s][i, j], $"minRoutes[{s}] <= c[{s}][{i},{j}]");
                    }
                }
            }
        }
    }
}
