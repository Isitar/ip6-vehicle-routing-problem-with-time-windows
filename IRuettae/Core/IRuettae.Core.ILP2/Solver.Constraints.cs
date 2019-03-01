using System;
using System.CodeDom;
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
        private void IncreasingC(GRBModel model, int numberOfRoutes, GRBVar[][] w, GRBVar[][] c, GRBVar[][] v)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                for (int i = 1; i < distances.GetLength(0); i++)
                {
                    model.AddGenConstrIndicator(v[s][i], 0, c[s][i] == 0, GurobiVarName($"if v[{s}][{i}] == 0 c[{s}][{i}] == 0"));
                    for (int k = 0; k < distances.GetLength(1); k++)
                    {
                        model.AddGenConstrIndicator(AccessW(w[s], k, i), 1, c[s][i] >= c[s][k] + distances[k, i] + visitDurations[k], GurobiVarName($"if w[{s}][{k},{i}] c[{s}][{i}] >= c[{s}][{k}]+ dist+duration"));
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
                var santaUsed = model.AddVar(0, 1, 0, GRB.BINARY, GurobiVarName($"Santa{s} used"));
                var santaUsedSum = new GRBLinExpr(0);
                for (int i = 1; i < distances.GetLength(0); i++)
                {
                    w0i += AccessW(w[s], 0, i);
                    wi0 += AccessW(w[s], i, 0);
                    model.AddConstr(santaUsed >= v[s][i], GurobiVarName($"santa_used[{s}]_>=_v[{s}][{i}]"));
                    santaUsedSum += v[s][i];
                }

                model.AddConstr(santaUsed <= santaUsedSum, GurobiVarName($"santa_used[{s}]_<=_sum(v[{s}][i])"));
                model.AddConstr(w0i == santaUsed, GurobiVarName($"way_from_home_santa_{s}"));
                model.AddConstr(wi0 == santaUsed, GurobiVarName($"way_to_home_santa_{s}"));
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

                    model.AddConstr(wki == wik, GurobiVarName($"w[{s}][k,{i}] == w[{s}][{i},k]"));
                    model.AddConstr(wki == v[s][i], GurobiVarName($"if visit {i} is visited by santa {s}, incoming way has to be used"));
                    model.AddConstr(wik == v[s][i], GurobiVarName($"if visit {i} is visited by santa {s}, outgoing way has to be used"));
                }
            }
        }

        private void FillMaxRoute(GRBModel model, GRBVar[] maxRoutes, GRBVar[][] c, GRBVar[][] v)
        {
            for (int s = 0; s < maxRoutes.Length; s++)
            {
                for (int i = 0; i < visitDurations.Length; i++)
                {
                    model.AddConstr(maxRoutes[s] >= c[s][i] + (visitDurations[i] + distances[i, 0]) * v[s][i], GurobiVarName($"maxRoute[{s}] >= visit {i} +duration+dist home"));
                }
            }
        }

        private void FillMinRoutes(GRBModel model, GRBVar[] minRoutes, GRBVar[][] c, GRBVar[][] v)
        {
            for (int s = 0; s < minRoutes.Length; s++)
            {
                var (dayStart, dayEnd) = input.Days[s / input.Santas.Length];

                for (int i = 0; i < visitDurations.Length; i++)
                {
                    model.AddConstr(minRoutes[s] <= c[s][i] - (distances[0, i]) * v[s][i] + (1 - v[s][i]) * (dayEnd - dayStart), GurobiVarName($"minRoute[{s}] <= visit {i} - dist from home bigM"));
                }
            }
        }

        private void MinRouteSmallerThanMaxRoute(GRBModel model, GRBVar[] minRoutes, GRBVar[] maxRoutes)
        {
            for (int s = 0; s < minRoutes.Length; s++)
            {
                model.AddConstr(minRoutes[s] <= maxRoutes[s], GurobiVarName($"minRoutesSmallerThanMaxRoutesForSanta{s}"));
            }
        }

        private void IncomingOutgoingGlobal(GRBModel model, int numberOfRoutes, GRBVar[][] w)
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
                        wki += AccessW(w[s], k, i);
                        wik += AccessW(w[s], i, k);
                    }
                }

                model.AddConstr(wki == 1, GurobiVarName($"v{i} incoming ways == 1"));
                model.AddConstr(wik == 1, GurobiVarName($"v{i} outgoing ways == 1"));
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

                model.AddConstr(sumVisitsVisited == sumWaysUsed, GurobiVarName($"Santa {s} visits visited + home == ways used"));
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

                model.AddConstr(sum == 1, GurobiVarName($"visit {i} visited once"));
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
                    model.AddGenConstrOr(v[s][i], v[s].Take(i - 1).Skip(1).ToArray(), GurobiVarName($"break v[{s}]{i}] ")); // assignment if used
                    breakRoutes.Add(s);
                }

                // break cannot be visited by another santa
                foreach (var nonBreakRoute in Enumerable.Range(0, numberOfRoutes).Except(breakRoutes))
                {
                    model.AddConstr(v[nonBreakRoute][i] == 0, GurobiVarName($"v[{nonBreakRoute}][{i}]==0,nonbreakroute"));
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
                    model.AddConstr(AccessW(w[s], i, i) == 0, GurobiVarName($"no selfie for {s} {i}"));
                }
            }
        }

        private void DesiredOverlap(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w, GRBVar[][] c, GRBVar[][][] desiredDuration)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var day = s / input.Santas.Length;
                var (dayStart, dayEnd) = input.Days[day];
                var dayDuration = dayEnd - dayStart;
                for (int i = 1; i < visitDurations.Length; i++)
                {
                    var visit = input.Visits[i - 1];
                    for (int d = 0; d < visit.Desired.Length; d++)
                    {
                        var (desiredFrom, desiredTo) = visit.Desired[d];
                        // check if desired on day
                        if (desiredTo < dayStart || desiredFrom > dayEnd)
                        {
                            model.Remove(desiredDuration[s][i][d]);
                            desiredDuration[s][i][d] = null; //free up memory
                            //model.AddConstr(desiredDuration[s][i][d] == 0, GurobiVarName($"desiredDuration[{s}][{i}][{d}] == 0, outside of day"));
                            continue;
                        }

                        var maxDesiredDuration = Math.Min(visit.Duration, desiredTo - desiredFrom);
                        model.AddConstr(desiredDuration[s][i][d] <= maxDesiredDuration * v[s][i], GurobiVarName($"desired[{s}][{i}][{d}] only possible if v[{s}][{i}]"));

                        var desiredStart = model.AddVar(Math.Max(desiredFrom - dayStart, 0), dayDuration, 0, GRB.CONTINUOUS, GurobiVarName($"desiredStart[{s}][{i}][{d}]"));

                        model.AddConstr(desiredStart >= c[s][i], GurobiVarName($"desiredStart[{s}[{i}][{d}] >= visitStart"));

                        var desiredEnd = model.AddVar(0, desiredTo - dayStart, 0, GRB.CONTINUOUS, GurobiVarName($"desiredEnd[{s}][{i}][{d}]"));

                        model.AddConstr(desiredEnd <= c[s][i] + visit.Duration * v[s][i], GurobiVarName($"desiredEnd[{s}[{i}][{d}] <= visitEnd"));
                        var binDecisionVariable = model.AddVar(0, 1, 0, GRB.BINARY, GurobiVarName($"binDesiredDecisionVar[{s}][{i}][{d}]"));

                        // if positive, duration = end -start
                        model.AddGenConstrIndicator(binDecisionVariable, 0, desiredEnd - desiredStart >= 0, null);
                        model.AddGenConstrIndicator(binDecisionVariable, 0, desiredDuration[s][i][d] == desiredEnd - desiredStart, null);
                        // if negative, duration = 0
                        model.AddGenConstrIndicator(binDecisionVariable, 1, desiredEnd - desiredStart <= 0, null);
                        model.AddGenConstrIndicator(binDecisionVariable, 1, desiredDuration[s][i][d] == 0, null);
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
        private void UnavailableOverlap(GRBModel model, int numberOfRoutes, GRBVar[][] v, GRBVar[][] w, GRBVar[][] c, GRBVar[][][] unavailableDuration, bool hardConstraint = false)
        {
            for (int s = 0; s < numberOfRoutes; s++)
            {
                var day = s / input.Santas.Length;
                var (dayStart, dayEnd) = input.Days[day];
                var dayDuration = dayEnd - dayStart;
                for (int i = 1; i < visitDurations.Length; i++)
                {
                    var visit = input.Visits[i - 1];
                    for (int d = 0; d < visit.Unavailable.Length; d++)
                    {
                        // check if unavailable is on this day
                        var (unavailableFrom, unavailableTo) = visit.Unavailable[d];

                        if (unavailableTo < dayStart || unavailableFrom > dayEnd)
                        {
                            model.Remove(unavailableDuration[s][i][d]);
                            unavailableDuration[s][i][d] = null;
                            //model.AddConstr(unavailableDuration[s][i][d] == 0, GurobiVarName($"unavailalbe[{s}][{i}][{d}] == 0, outside of day"));
                            continue;
                        }

                        // temp
                        if (hardConstraint)
                        {
                            model.AddConstr(unavailableDuration[s][i][d] == 0, GurobiVarName($"unavailalbe[{s}][{i}][{d}] == 0, hard constraint"));
                        }

                        var maxUnavailableDuration = Math.Min(visit.Duration, unavailableTo - unavailableFrom);
                        model.AddConstr(unavailableDuration[s][i][d] <= maxUnavailableDuration * v[s][i], GurobiVarName($"unavailable[{s}][{i}][{d}] only possible if v[{s}][{i}]"));

                        var unavailableStart = model.AddVar(unavailableFrom - dayStart, dayDuration, 0, GRB.CONTINUOUS, GurobiVarName($"unavailableStart[{s}][{i}][{d}]"));
                        var binHelperStart = model.AddVar(0, 1, 0, GRB.BINARY, GurobiVarName($"binHelperUnavailableStart[{s}][{i}][{d}]"));

                        var visitStart = c[s][i];

                        model.AddConstr(unavailableStart >= visitStart, null);
                        model.AddGenConstrIndicator(binHelperStart, 0, unavailableStart <= unavailableFrom - dayStart, null);
                        model.AddGenConstrIndicator(binHelperStart, 1, unavailableStart <= visitStart, null);

                        var unavailableEnd = model.AddVar(0, unavailableTo - dayStart, 0, GRB.CONTINUOUS, GurobiVarName($"unavailableEnd[{s}][{i}][{d}]"));
                        var binHelperEnd = model.AddVar(0, 1, 0, GRB.BINARY, GurobiVarName($"binHelperUnavailableEnd[{s}][{i}][{d}]"));

                        var visitEnd = visitStart + visit.Duration * v[s][i];

                        model.AddConstr(unavailableEnd <= visitEnd, GurobiVarName($"unavailableEnd[{s}[{i}][{d}] <= visitEnd"));
                        model.AddGenConstrIndicator(binHelperEnd, 0, unavailableEnd >= unavailableTo - dayStart, null);
                        model.AddGenConstrIndicator(binHelperEnd, 1, unavailableEnd >= visitEnd, null);

                        model.AddConstr(
                            unavailableDuration[s][i][d] >= unavailableEnd - unavailableStart,
                            GurobiVarName($"unavailable overlap[{s}][{i}][{d}]"));
                    }
                }
            }
        }

        private void LowerBoundTotalWaytime(GRBModel model, GRBLinExpr totalWayTime)
        {
            // lower boudn totalWayTime
            model.AddConstr(totalWayTime >= visitDurations.Sum(), null);
        }

        private string GurobiVarName(string varname)
        {
            return varname
                .Replace(";", "")
                .Replace(">=", "gt")
                .Replace("<=", "st")
                .Replace("==", "eq")
                .Replace(" ", "_");
        }
    }
}
