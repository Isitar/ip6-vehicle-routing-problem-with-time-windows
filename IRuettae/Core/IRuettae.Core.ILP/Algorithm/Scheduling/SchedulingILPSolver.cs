using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.ILP.Algorithm.Scheduling.Detail;
using IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.ILP.Algorithm.Scheduling
{
    public class SchedulingILPSolver : ISolver
    {
        private readonly SolverData solverData;
        private double MIP_GAP = 0;
        private bool hasModel = false;
        private ResultState resultState = ResultState.NotSolved;

        private readonly GLS.Solver solver = new GLS.Solver("SantaProblem", GLS.Solver.SCIP_MIXED_INTEGER_PROGRAMMING);
        private readonly ITargetFunctionBuilder targetFunctionBuilder;
        private long timelimit = 0;

        /// <summary>
        ///
        /// </summary>
        public SchedulingILPSolver(SolverInputData solverInputData, SchedulingOptimizationGoals optimizationGoal = SchedulingOptimizationGoals.Default)
        {
            this.solverData = new SolverData(solverInputData, solver);
            this.targetFunctionBuilder = TargetFunctionBuilderFactory.Create(optimizationGoal);
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
            PrintVisits();
            PrintVisitsPerSanta();
            PrintSantaEnRoute();
        }

        private void PrintMeta()
        {
            Debug.WriteLine("====================");
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
        }

        private void PrintSantaEnRoute()
        {
            Debug.WriteLine("====================");
            Debug.WriteLine("-PrintSantaEnRoute");
            for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
            {
                Debug.WriteLine($"Santa: {santa}");
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    Debug.WriteLine($"Day: {day}");
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        Debug.Write(solverData.Variables.SantaEnRoute[day][santa, timeslice].SolutionValue());
                    }
                    Debug.WriteLine($" (SantaEnRoute)");
                    Debug.WriteLine(string.Empty);
                }
                Debug.WriteLine(string.Empty);
            }
        }

        private void PrintVisitsPerSanta()
        {
            Debug.WriteLine("====================");
            Debug.WriteLine("-PrintVisitsPerSanta");
            for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
            {
                Debug.WriteLine($"Santa: {santa}");
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    Debug.WriteLine($"Day: {day}");
                    for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                    {
                        for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                        {
                            Debug.Write(solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice].SolutionValue());
                        }
                        Debug.WriteLine($" (Visit {visit})");
                    }
                    Debug.WriteLine(string.Empty);
                }
                Debug.WriteLine(string.Empty);
            }
        }

        private void PrintVisits()
        {
            Debug.WriteLine("====================");
            Debug.WriteLine("-PrintVisits");
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                Debug.WriteLine($"Visit: {visit}");
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    Debug.WriteLine($"Day: {day}");
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        Debug.Write(solverData.Variables.VisitStart[day][visit, timeslice].SolutionValue());
                    }
                    Debug.WriteLine(" (VisitStart)");
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        Debug.Write(solverData.Variables.Visits[day][visit, timeslice].SolutionValue());
                    }
                    Debug.WriteLine(" (Visits)");
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

            // return solver.ExportModelAsLpFormat(false);
            return solver.ExportModelAsMpsFormat(false, false);
        }

        /// <summary>
        /// Exports the mps to the path as file
        /// </summary>
        /// <param name="path">where to save the mps</param>
        public void ExportMPSAsFile(string path)
        {
            System.IO.File.WriteAllText(path, ExportMPS());
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