using System;
using System.Collections.Generic;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;
using IRuettae.Core.Algorithm.GoogleORTools.TargetFunctionBuilders;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools
{
    class Solver : ISolver
    {
        private readonly int[,] distances;

        private bool hasModel = false;
        private ResultState resultState = ResultState.NotSolved;
        private Route result = null;

        private readonly GLS.Solver solver = new GLS.Solver("SantaProblem", GLS.Solver.CBC_MIXED_INTEGER_PROGRAMMING);
        private readonly VariableBuilder variables;
        private readonly ConstraintBuilder constraints;
        private readonly AbstractTargetFunctionBuilder targetFunctionBuilder;
        private readonly ResultBuilder resultBuilder;

        /// <summary>
        ///
        /// </summary>
        /// <param name="distances">2d Array of all distances from - to, first element is starting point</param>
        public Solver(int[,] distances, AbstractTargetFunctionBuilder targetFunctionBuilder)
        {
            this.distances = distances;
            variables = new VariableBuilder(solver);
            constraints = new ConstraintBuilder(variables);
            this.targetFunctionBuilder = targetFunctionBuilder;
            resultBuilder = new ResultBuilder(variables);
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
            AddContraints();
            AddTargetFunction();

            hasModel = true;
            resultState = ResultState.NotSolved;
        }

        private void InitGoogleSolver()
        {
            solver.Reset();
        }

        private void AddVariables()
        {
            variables.CreateVariables(distances);
        }

        private void AddContraints()
        {
            constraints.CreateConstraints();
        }
        private void AddTargetFunction()
        {
            targetFunctionBuilder.CreateTargetFunction(variables);
        }

        private ResultState SolveInternal()
        {
            resultState = FromGoogleResultState(solver.Solve());
            return resultState;
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
            return resultBuilder.CreateResult();
        }
    }
}