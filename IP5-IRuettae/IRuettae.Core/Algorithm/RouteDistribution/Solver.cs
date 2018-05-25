using System;
using System.Collections.Generic;
using System.Diagnostics;
using Google.OrTools.LinearSolver;
using IRuettae.Core.Algorithm.Scheduling.Detail;
using IRuettae.Core.Algorithm.Scheduling.TargetFunctionBuilders;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.RouteDistribution
{
    // Todo: Meyerj create or-tools solver base class
    // Todo: implement starter class
    public class Solver
    {
        private readonly SolverInputData solverInputData;
        private double MIP_GAP = 0;
        private bool hasModel = false;
        private ResultState resultState = ResultState.NotSolved;
        // [santa][route,day]
        private Variable[][,] variables;
        private readonly GLS.Solver solver = new GLS.Solver("SantaProblem", GLS.Solver.SCIP_MIXED_INTEGER_PROGRAMMING);

        /// <summary>
        ///
        /// </summary>
        public Solver(SolverInputData solverInputData)
        {
            this.solverInputData = solverInputData;
        }

        public ResultState Solve()
        {
            CreateModel();
            return SolveInternal();
        }

        public ResultState Solve(double MIP_GAP)
        {
            this.MIP_GAP = MIP_GAP;
            return Solve();
        }

        private void CreateModel()
        {
            InitGoogleSolver();
            AddVariables();
            AddConstraints();
            AddTargetFunction();

            hasModel = true;
            resultState = ResultState.NotSolved;
        }

        private void InitGoogleSolver()
        {
            PrintDebugRessourcesBefore("InitGoogleSolver");

            solver.Reset();

            PrintDebugRessourcesAfter();
        }

        private void AddVariables()
        {
            PrintDebugRessourcesBefore("AddVariables");

            variables = new Variable[solverInputData.NumberOfSantas][,];
            for (int santa = 0; santa < solverInputData.NumberOfSantas; santa++)
            {
                variables[santa] = solver.MakeBoolVarMatrix(solverInputData.NumberOfRoutes, solverInputData.NumberOfDays, $"variables_santa{santa}");
            }

            PrintDebugRessourcesAfter();
        }

        private void AddConstraints()
        {
            PrintDebugRessourcesBefore("AddConstraints");

            AddUseEveryRouteOnceConstraint();
            AddOnlyOneRoutePerDayConstraint();

            PrintDebugRessourcesAfter();
        }


        /// <summary>
        /// Each Route must be done exactly once
        /// </summary>
        private void AddUseEveryRouteOnceConstraint()
        {
            for (int route = 0; route < solverInputData.NumberOfRoutes; route++)
            {
                // 1 == Z1 + Z2 + ...
                var sum = new LinearExpr();
                for (int santa = 0; santa < solverInputData.NumberOfSantas; santa++)
                {
                    for (int day = 0; day < solverInputData.NumberOfDays; day++)
                    {
                        sum += variables[santa][route, day];
                    }
                }
                solver.Add(sum == 1);
            }
        }

        /// <summary>
        /// A santa can only do one route per day
        /// </summary>
        private void AddOnlyOneRoutePerDayConstraint()
        {
            for (int santa = 0; santa < solverInputData.NumberOfSantas; santa++)
            {
                for (int day = 0; day < solverInputData.NumberOfDays; day++)
                {
                    // 1 >= Z1 + Z2 + ...
                    var sum = new LinearExpr();
                    for (int route = 0; route < solverInputData.NumberOfRoutes; route++)
                    {
                        sum += variables[santa][route, day];
                    }
                    solver.Add(sum <= 1);
                }
            }
        }

        private void AddTargetFunction()
        {
            PrintDebugRessourcesBefore("AddTargetFunction");

            var sum = new LinearExpr();
            for (int santa = 0; santa < solverInputData.NumberOfSantas; santa++)
            {
                for (int day = 0; day < solverInputData.NumberOfDays; day++)
                {
                    for (int route = 0; route < solverInputData.NumberOfRoutes; route++)
                    {
                        sum += variables[santa][route, day] * solverInputData.RouteCost[santa][route, day];
                    }
                }
            }

            solver.Minimize(sum);

            PrintDebugRessourcesAfter();
        }

        private ResultState SolveInternal()
        {
            PrintDebugRessourcesBefore("SolveInternal");

            var param = new GLS.MPSolverParameters();
            param.SetDoubleParam(GLS.MPSolverParameters.RELATIVE_MIP_GAP, MIP_GAP);

            solver.Objective().SetMinimization();
            //solver.EnableOutput();
            resultState = FromGoogleResultState(solver.Solve(param));

            PrintDebugRessourcesAfter();
#if DEBUG
            PrintDebugOutput();
#endif

            return resultState;
        }

        private void PrintDebugOutput()
        {
            Debug.WriteLine(string.Empty);
            Debug.WriteLine("DEBUG OUTPUT");
            Debug.WriteLine(string.Empty);

            PrintMeta();
            PrintVariables();
        }

        private void PrintMeta()
        {
            Debug.WriteLine("====================");
            Debug.WriteLine("-Metadata");
            Debug.WriteLine(string.Empty);
            Debug.WriteLine($"Value of the target function: {solver.Objective().Value()}");
            Debug.WriteLine($"Variables: {solver.NumVariables()}");
            Debug.WriteLine($"Number of constraints: {solver.NumConstraints()}");
            Debug.WriteLine($"Iterations: {solver.Iterations()}");
            Debug.WriteLine($"Nodes: {solver.Nodes()}");
            Debug.WriteLine($"Objective Minimization: {solver.Objective().Minimization()}");
            Debug.WriteLine(string.Empty);
        }

        private void PrintVariables()
        {
            Debug.WriteLine("====================");
            Debug.WriteLine("-Variables");


            for (int santa = 0; santa < solverInputData.NumberOfSantas; santa++)
            {
                Debug.WriteLine($"Santa: {santa} [route,day]");
                for (int day = 0; day < solverInputData.NumberOfDays; day++)
                {
                    for (int route = 0; route < solverInputData.NumberOfRoutes; route++)
                    {
                        Debug.Write(variables[santa][route, day].SolutionValue());
                    }
                    Debug.WriteLine(string.Empty);
                }
                Debug.WriteLine(string.Empty);
            }
        }

        private ResultState FromGoogleResultState(int resultState)
        {
            Dictionary<int, ResultState> mapping = new Dictionary<int, ResultState>()
            {
                {GLS.Solver.OPTIMAL, ResultState.Optimal },
                {GLS.Solver.FEASIBLE, ResultState.Feasible },
                {GLS.Solver.INFEASIBLE, ResultState.Infeasible },
                {GLS.Solver.NOT_SOLVED, ResultState.NotSolved },
            };
            if (mapping.TryGetValue(resultState, out var value))
            {
                return value;
            }
            return ResultState.Unknown;
        }

        /// <summary>
        /// Returns the mps as string
        /// </summary>
        /// <returns>mps as string</returns>
        public string ExportMPS()
        {
            if (!hasModel)
            {
                CreateModel();
            }

            return solver.ExportModelAsMpsFormat(false, false);
        }

        public string ImportMPS()
        {
            throw new NotImplementedException();
        }

        public RouteDistribution GetResult()
        {
            var result = new RouteDistribution(solverInputData.NumberOfSantas, solverInputData.NumberOfDays);

            for (int santa = 0; santa < solverInputData.NumberOfSantas; santa++)
            {
                for (int day = 0; day < solverInputData.NumberOfDays; day++)
                {
                    for (int route = 0; route < solverInputData.NumberOfRoutes; route++)
                    {
                        if (variables[santa][route, day].SolutionValue() == 1.0)
                        {
                            result.Distribution[santa, day] = route;
                        }
                    }
                }
            }

            return result;
        }

        private void PrintDebugRessourcesBefore(string name)
        {
            Debug.WriteLine("");
            Debug.WriteLine($"{name}");
            PrintDebugRessources("before");
        }

        private void PrintDebugRessourcesAfter()
        {
            PrintDebugRessources("after");
        }

        private void PrintDebugRessources(string description)
        {
            Debug.WriteLine($"{description}: {solver.NumConstraints()} constraint, {Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024} MB used memory;");
        }
    }
}