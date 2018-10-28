using IRuettae.Core.ILP.Algorithm.Scheduling.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders
{
    internal class TryDesiredOnlyTargetFunctionBuilder : ITargetFunctionBuilder
    {
        private GLS.LinearExpr targetFunction = new GLS.LinearExpr();

        public void CreateTargetFunction(SolverData solverData)
        {
            var factory = new TargetFunctionFactory(solverData);

            targetFunction += factory.CreateTargetFunction(TargetType.TryVisitDesired);

            solverData.Solver.Minimize(targetFunction);
        }
    }
}