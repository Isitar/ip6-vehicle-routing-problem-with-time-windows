using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IRuettae.Core.Algorithm.Clustering.Detail;
using IRuettae.Core.Algorithm.Clustering.TargetFunctionBuilders;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.Clustering
{
    internal class Solver : ISolver
    {
        private readonly SolverData solverData;

        private bool hasModel = false;
        private ResultState resultState = ResultState.NotSolved;
        private double MIP_GAP = 0;
        private long timelimit = 0;

        private readonly GLS.Solver solver = new GLS.Solver("Santa Problem", GLS.Solver.SCIP_MIXED_INTEGER_PROGRAMMING);
        //new GLS.Solver("SantaProblem", GLS.Solver.CBC_MIXED_INTEGER_PROGRAMMING);
        private readonly AbstractTargetFunctionBuilder targetFunctionBuilder;

        public Solver(SolverInputData solverInputData, AbstractTargetFunctionBuilder targetFunctionBuilder)
        {
            this.solverData = new SolverData(solverInputData, solver);
            this.targetFunctionBuilder = targetFunctionBuilder;
        }

        public ResultState Solve()
        {
            CreateModel();
            return SolveInternal();
        }

        public ResultState Solve(double MIP_GAP, long timelimit)
        {
            this.MIP_GAP = MIP_GAP;
            this.timelimit = timelimit;
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

            var vb = new VariableBuilder(solverData);
            vb.CreateVariables();

            PrintDebugRessourcesAfter();
        }

        private void AddConstraints()
        {
            PrintDebugRessourcesBefore("AddConstraints");

            var cb = new ConstraintBuilder(solverData);
            cb.CreateConstraints();

            PrintDebugRessourcesAfter();
        }

        private void AddTargetFunction()
        {
            PrintDebugRessourcesBefore("AddTargetFunction");

            targetFunctionBuilder.CreateTargetFunction(solverData);

            PrintDebugRessourcesAfter();
        }

        private ResultState SolveInternal()
        {
            PrintDebugRessourcesBefore("SolveInternal");

            var param = new GLS.MPSolverParameters();

            param.SetDoubleParam(GLS.MPSolverParameters.RELATIVE_MIP_GAP, MIP_GAP);
            if (timelimit != 0)
            {
                solver.SetTimeLimit(timelimit);
            }

            solver.EnableOutput();
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

        private static void DebugHR()
        {
            Debug.WriteLine(new string('-', 20));
        }

        private void PrintVariables()
        {

            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                DebugHR();
                Debug.WriteLine($"{santa} Santa Visits: ");
                foreach (var visit in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    Debug.WriteLine($"Visit {visit}: {solverData.Variables.SantaVisit[santa, visit].SolutionValue()}");
                }
                Debug.WriteLine(string.Empty);
            }
            DebugHR();

            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                Debug.WriteLine($"{santa} Santa Uses Way");
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        var value = solverData.Variables.SantaUsesWay[santa][source, destination].SolutionValue();
                        if (Math.Abs(value) > 0.0001)
                        {
                            Debug.WriteLine($"S: {source}  |  D: {destination}");
                        }
                    }

                }
                DebugHR();
            }


            //foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            //{
            //    Debug.WriteLine($"{santa} Santa Way Flow");
            //    foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
            //    {
            //        foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
            //        {
            //            var value = solverData.Variables.SantaWayFlow[santa][source, destination].SolutionValue();
            //            Debug.WriteLine($"S: {source}  |  D: {destination} | {value}");
            //        }

            //    }
            //    DebugHR();
            //}

        }

        private void PrintMeta()
        {
            DebugHR();
            Debug.WriteLine("-Metadata");
            Debug.WriteLine(string.Empty);
            Debug.WriteLine($"Value of the target function: {solverData.Solver.Objective().Value()}");
            Debug.WriteLine($"Variables: {solverData.Solver.NumVariables()}");
            Debug.WriteLine($"Number of constraints: {solverData.Solver.NumConstraints()}");
            Debug.WriteLine($"Iterations: {solverData.Solver.Iterations()}");
            Debug.WriteLine($"Nodes: {solverData.Solver.Nodes()}");
            Debug.WriteLine($"Objective Minimization: {solverData.Solver.Objective().Minimization()}");
            Debug.WriteLine($"Best Bound: {solverData.Solver.Objective().BestBound()}");
            Debug.WriteLine(string.Empty);
            DebugHR();
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

            // return solver.ExportModelAsLpFormat(false);
            return solver.ExportModelAsMpsFormat(false, false);
        }

        public string ImportMPS()
        {
            throw new NotImplementedException();
        }

        public Route GetResult()
        {
            var route = new ResultBuilder(solverData).CreateResult();
            route.SolutionValue = SolutionValue();
            return route;
        }

        public double SolutionValue()
        {
            return solver.Objective().Value();
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
            Debug.WriteLine($"{description}: {solverData.Solver.NumConstraints()} constraint, {Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024} MB used memory;");
        }
    }
}