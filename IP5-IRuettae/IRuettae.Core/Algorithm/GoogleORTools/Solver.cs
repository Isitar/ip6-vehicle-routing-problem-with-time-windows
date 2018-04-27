using System;
using System.Collections.Generic;
using System.Diagnostics;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;
using IRuettae.Core.Algorithm.GoogleORTools.TargetFunctionBuilders;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools
{
    class Solver : ISolver
    {
        private readonly SolverData solverData;

        private bool hasModel = false;
        private ResultState resultState = ResultState.NotSolved;
        //private Route result = null;

        private readonly GLS.Solver solver = new GLS.Solver("SantaProblem", GLS.Solver.CBC_MIXED_INTEGER_PROGRAMMING);
        private readonly AbstractTargetFunctionBuilder targetFunctionBuilder;

        /// <summary>
        ///
        /// </summary>
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

        private void CreateModel()
        {
            InitGoogleSolver();
            AddVariables();
            AddConstraints();
            AddTargetFunction();

            Debug.WriteLine($"Number of constraints: {solverData.Solver.NumConstraints()}");
            hasModel = true;
            resultState = ResultState.NotSolved;
        }

        private void InitGoogleSolver()
        {
            solver.Reset();
        }

        private void AddVariables()
        {
            var vb = new VariableBuilder(solverData);
            vb.CreateVariables();
        }

        private void AddConstraints()
        {
            var cb = new ConstraintBuilder(solverData);
            cb.CreateConstraints();
        }

        private void AddTargetFunction()
        {
            targetFunctionBuilder.CreateTargetFunction(solverData);
        }

        private ResultState SolveInternal()
        {
            resultState = FromGoogleResultState(solver.Solve());

#if DEBUG
            PrintDebugOutput();
#endif

            return resultState;
        }

        private void PrintDebugOutput()
        {
            Debug.WriteLine("");
            Debug.WriteLine("DEBUG OUTPUT");
            Debug.WriteLine("");

            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                Debug.WriteLine($"Visit: {visit}");
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    Debug.WriteLine($"Day: {day}");
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        Debug.Write(solverData.Variables.DebugStarts[day][visit][timeslice].SolutionValue());
                    }
                    Debug.WriteLine(" (DebugStarts)");
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

        public string ExportMPS()
        {
            if (!hasModel)
            {
                CreateModel();
            }
            return solver.ExportModelAsMpsFormat(true, false);
        }

        public string ImportMPS()
        {
            throw new NotImplementedException();
        }

        public Route GetResult()
        {
            var rb = new ResultBuilder(solverData);
            return rb.CreateResult();
        }
    }
}