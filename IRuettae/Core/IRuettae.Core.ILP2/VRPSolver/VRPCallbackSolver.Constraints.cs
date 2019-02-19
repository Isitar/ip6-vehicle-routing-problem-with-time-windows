using System.Collections.Generic;
using System.Linq;
using Gurobi;

namespace IRuettae.Core.ILP2.VRPSolver
{
    public partial class VRPCallbackSolver
    {
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

        private void IncomingOutgoingWaysConstraints(GRBModel model, int numberOfRoutes, GRBVar[][] w, GRBVar[][] v)
        {
            for (int i = 1; i < distances.GetLength(0); i++)
            {
                if (input.Visits[i - 1].IsBreak)
                {
                    continue;
                }

                var wki = new GRBLinExpr(0);
                var wik = new GRBLinExpr(0);

                for (int s = 0; s < numberOfRoutes; s++)
                {
                    for (int k = 0; k < distances.GetLength(1); k++)
                    {
                        var swki = AccessW(w[s], k, i);
                        var swik = AccessW(w[s], i, k);

                        wki += swki;
                        wik += swik;
                    }
                }

                model.AddConstr(wki == 1, $"v{i} incoming ways == 1");
                model.AddConstr(wik == 1, $"v{i} outgoing ways == 1");
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

                    model.AddConstr(wki == wik, null);
                    model.AddConstr(wki == v[s][i], $"if visit {i} is visited by santa {s}, incoming way has to be used");
                    model.AddConstr(wik == v[s][i], $"if visit {i} is visited by santa {s}, outgoing way has to be used");
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
                model.AddConstr(w0i == wi0, null);
                model.AddConstr(w0i == santaUsed, $"way from home santa {s}");
                model.AddConstr(wi0 == santaUsed, $"way to home santa {s}");
            }
        }

        private void SelfieConstraint(GRBModel model, int numberOfRoutes, GRBVar[][] w)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                // selfies
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    AccessW(w[s], i, i).UB = 0;
                }
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

                // break cannot be visited by another santa
                foreach (var nonBreakRoute in Enumerable.Range(0, numberOfRoutes).Except(breakRoutes))
                {
                    model.AddConstr(v[nonBreakRoute][i] == 0, null);
                }
            }
        }

        private void NumberOfWaysMatchForSanta(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var sumOutgoingWaysUsed = new GRBLinExpr(0);
                var sumIncomingWaysUsed = new GRBLinExpr(0);
                var sumVisitsVisited = new GRBLinExpr(0);
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    sumVisitsVisited += v[s][i];
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        sumOutgoingWaysUsed += AccessW(w[s], i, j);
                        sumIncomingWaysUsed += AccessW(w[s], j, i);
                    }
                }

                model.AddConstr(sumVisitsVisited == sumOutgoingWaysUsed, $"Santa {s} visits visited + home == ways outgoing used");
                model.AddConstr(sumVisitsVisited == sumIncomingWaysUsed, $"Santa {s} visits visited + home == ways incoming used");

                model.AddConstr(sumIncomingWaysUsed == sumOutgoingWaysUsed, $"Santa {s} sum incoming == sum outgoing");
            }
        }
    }
}
