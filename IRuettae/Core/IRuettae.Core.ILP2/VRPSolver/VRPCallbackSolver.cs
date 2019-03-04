using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;
using IRuettae.Core.Models;

namespace IRuettae.Core.ILP2.VRPSolver
{
    public partial class VRPCallbackSolver
    {
        private OptimizationInput input;
        private int[,] distances;
        private int[] visitDurations;

        public VRPCallbackSolver(OptimizationInput input)
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

        private T[,] ConvertBack<T>(T[] orig)
        {
            var retVal = new T[distances.GetLength(0), distances.GetLength(0)];
            for (int i = 0; i < distances.GetLength(0); i++)
            {
                for (int j = 0; j < distances.GetLength(0); j++)
                {
                    retVal[i, j] = orig[i * distances.GetLength(0) + j];
                }
            }

            return retVal;
        }

        public List<int[]> SolveVRP(int timeLimitMilliseconds)
        {
            using (var env = new GRBEnv($"{DateTime.Now:yy-MM-dd-HH-mm-ss}_vrp_gurobi.log"))
            using (var vrpModel = new GRBModel(env))
            {
                #region initialize Variables
                var numberOfRoutes = input.Santas.Length * input.Days.Length;
                var v = new GRBVar[numberOfRoutes][]; // [santa] visits [visit]
                var w = new GRBVar[numberOfRoutes][]; // [santa] uses [way]


                for (int s = 0; s < numberOfRoutes; s++)
                {
                    v[s] = new GRBVar[visitDurations.Length];
                    for (int i = 0; i < v[s].Length; i++)
                    {
                        v[s][i] = vrpModel.AddVar(0, 1, 0.0, GRB.BINARY, $"v[{s}][{i}]");
                    }

                    w[s] = vrpModel.AddVars(distances.GetLength(0) * distances.GetLength(1), GRB.BINARY);
                }
                #endregion initialize Variables

                #region add constraints
                SelfieConstraint(vrpModel, numberOfRoutes, w);
                VisitVisitedOnce(vrpModel, numberOfRoutes, v);
                IncomingOutgoingWaysConstraints(vrpModel, numberOfRoutes, w, v);
                IncomingOutgoingSanta(vrpModel, numberOfRoutes, v, w);
                IncomingOutgoingSantaHome(vrpModel, numberOfRoutes, w, v);
                NumberOfWaysMatchForSanta(vrpModel, numberOfRoutes, v, w);
                BreakHandling(vrpModel, numberOfRoutes, v);
                #endregion add constraints

                var totalWayTime = new GRBLinExpr(0);
                var longestRoute = vrpModel.AddVar(0, input.Days.Max(d => d.to - d.from), 0, GRB.CONTINUOUS,
                    "longestRoute");
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    var routeTime = new GRBLinExpr(0);
                    for (int i = 0; i < visitDurations.Length; i++)
                    {
                        routeTime += v[s][i] * visitDurations[i];
                        for (int j = 0; j < visitDurations.Length; j++)
                        {
                            routeTime += AccessW(w[s], i, j) * distances[i, j];
                        }
                    }

                    totalWayTime += routeTime;
                    vrpModel.AddConstr(longestRoute >= routeTime, $"longesRouteConstr{s}");
                }

                vrpModel.SetObjective(
                    +(40d / 3600d) * totalWayTime
                    + (30d / 3600d) * longestRoute
                    , GRB.MINIMIZE);

                vrpModel.Parameters.LazyConstraints = 1;
                vrpModel.SetCallback(new VRPCallbackSolverCallback(w, AccessW, ConvertBack));
                vrpModel.Parameters.TimeLimit = timeLimitMilliseconds / 1000;
                InitializeModel(w, v, numberOfRoutes, input.Visits, input.Santas.Length);
                vrpModel.Optimize();
                if (vrpModel.SolCount == 0)
                {
                    return null;
                }

                var routes = new List<int[]>();
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    var route = new List<int>();
                    var currVisit = 0;
                    do
                    {
                        route.Add(currVisit);
                        for (int i = 0; i < visitDurations.Length; i++)
                        {
                            if (AccessW(w[s], currVisit, i).X > 0.5)
                            {
                                currVisit = i;
                                break;
                            }
                        }
                    } while (currVisit != 0);
                    routes.Add(route.ToArray());
                }

                vrpModel.Reset();
                return routes;
            }
        }

        private void InitializeModel(GRBVar[][] w, GRBVar[][] v, int numberOfRoutes, Visit[] inputVisits, int numberOfSantas)
        {
            var rest = inputVisits.Count(visit => !visit.IsBreak) % numberOfRoutes;
            var visitsPerRoute = inputVisits.Count(visit => !visit.IsBreak) / numberOfRoutes;
            var visitIndex = 1;
            for (var day = 0; day < input.Days.Length; day++)
            {
                for (var santa = 0; santa < numberOfSantas; santa++)
                {
                    var visitSequence = new List<int> { 0 };
                    var s = numberOfSantas * day + santa;
                    foreach (var breakVisit in inputVisits.Where(visit => visit.IsBreak && (visit.SantaId == santa)))
                    {
                        v[s][breakVisit.Id + 1].Start = 1;
                        visitSequence.Add(breakVisit.Id + 1);
                    }

                    for (var i = 0; i < visitsPerRoute; i++)
                    {
                        v[s][visitIndex].Start = 1;
                        visitSequence.Add(visitIndex);
                        visitIndex++;
                    }

                    if (day + 1 == input.Days.Length && santa + 1 == numberOfSantas)
                    {
                        for (int i = 0; i < rest; i++)
                        {
                            v[s][visitIndex].Start = 1;
                            visitSequence.Add(visitIndex);
                            visitIndex++;
                        }
                    }

                    visitSequence.Add(0);

                    for (int i = 1; i < visitSequence.Count; i++)
                    {
                        AccessW(w[s], visitSequence[i - 1], visitSequence[i]).Start = 1;
                    }
                }
            }
        }
    }
}
