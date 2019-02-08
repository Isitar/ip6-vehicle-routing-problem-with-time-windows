using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;

namespace IRuettae.Core.ILP2.VRPSolver
{
    internal class VRPCallbackSolverCallback : GRBCallback
    {
        private GRBVar[][] vars;
        private readonly Func<GRBVar[], int, int, GRBVar> accessVar;
        private readonly Func<double[], double[,]> convertBack;

        public VRPCallbackSolverCallback(GRBVar[][] vars, Func<GRBVar[], int, int, GRBVar> accessVar, Func<double[], double[,]> convertBack)
        {
            this.vars = vars;
            this.accessVar = accessVar;
            this.convertBack = convertBack;
        }
        protected override void Callback()
        {
            try
            {
                if (where == GRB.Callback.MIPSOL)
                {
                    var invalidSols = new List<(int cnt, GRBLinExpr expr)>();
                    for (int s = 0; s < vars.Length; s++)
                    {
                        var solutionVars = convertBack(GetSolution(vars[s]));
                        var visitedVisits = new List<int>();
                        var tspToVRPVisitMap = new int[solutionVars.GetLength(0)];
                        for (int i = 0; i < solutionVars.GetLength(0); i++)
                        {
                            for (int j = 0; j < solutionVars.GetLength(1); j++)
                            {
                                if (solutionVars[i, j] > 0.5)
                                {
                                    visitedVisits.Add(i);
                                    tspToVRPVisitMap[visitedVisits.Count - 1] = i;
                                }
                            }
                        }

                        var tspWays = new double[visitedVisits.Count, visitedVisits.Count];
                        for (int i = 0; i < tspWays.GetLength(0); i++)
                        {
                            for (int j = 0; j < tspWays.GetLength(1); j++)
                            {
                                tspWays[i, j] = solutionVars[tspToVRPVisitMap[i], tspToVRPVisitMap[j]];
                            }
                        }

                        int[] tour = tspWays.Length == 0 ? new int[0] : Findsubtour(tspWays);
                        int n = tspWays.GetLength(0);
                        if (tour.Length < n)
                        {
                            // Add subtour elimination constraint
                            var expr = new GRBLinExpr(0);
                            for (int i = 0; i < tour.Length; i++)
                            {
                                for (int j = 0; j < tour.Length; j++)
                                {
                                    expr += accessVar(vars[s], tspToVRPVisitMap[tour[i]], tspToVRPVisitMap[tour[j]]);
                                }
                            }

                            invalidSols.Add((tour.Length, expr));
                            AddLazy(expr <= tour.Length - 1);

                        }
                    }

                    if (invalidSols.Count > 0)
                    {
                        var expr = new GRBLinExpr(0);
                        foreach (var invalidSol in invalidSols)
                        {
                            expr += invalidSol.expr;
                        }

                        AddLazy(expr <= invalidSols.Select(s => s.cnt).Sum() - 1);
                    }
                }
            }
            catch (GRBException e)
            {
                Console.WriteLine("Error code: " + e.ErrorCode + ". " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine("something went wrong");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        // Given an integer-feasible solution 'sol', return the smallest
        // sub-tour (as a list of node indices).

        public static int[] Findsubtour(double[,] sol)
        {
            int n = sol.GetLength(0);
            bool[] seen = new bool[n];
            int[] tour = new int[n];
            int bestind, bestlen;
            int i, node, len, start;

            for (i = 0; i < n; i++)
                seen[i] = false;

            start = 0;
            bestlen = n + 1;
            bestind = -1;
            node = 0;
            while (start < n)
            {
                for (node = 0; node < n; node++)
                    if (!seen[node])
                        break;
                if (node == n)
                    break;
                for (len = 0; len < n; len++)
                {
                    tour[start + len] = node;
                    seen[node] = true;
                    for (i = 0; i < n; i++)
                    {
                        if (sol[node, i] > 0.5 && !seen[i])
                        {
                            node = i;
                            break;
                        }
                    }
                    if (i == n)
                    {
                        len++;
                        if (len < bestlen)
                        {
                            bestlen = len;
                            bestind = start;
                        }
                        start += len;
                        break;
                    }
                }
            }

            for (i = 0; i < bestlen; i++)
                tour[i] = tour[bestind + i];
            System.Array.Resize(ref tour, bestlen);

            return tour;
        }



    }
}
