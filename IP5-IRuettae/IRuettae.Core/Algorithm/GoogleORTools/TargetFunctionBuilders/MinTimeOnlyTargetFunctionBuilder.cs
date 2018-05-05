using IRuettae.Core.Algorithm.GoogleORTools.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.TargetFunctionBuilders
{
    class MinTimeOnlyTargetFunctionBuilder : AbstractTargetFunctionBuilder
    {
        private GLS.LinearExpr targetFunction = new GLS.LinearExpr();

        public override void CreateTargetFunction(SolverData solverData)
        {
            var factory = new TargetFunctionFactory(solverData);

            targetFunction += factory.CreateTargetFunction(TargetType.MinTime, null);

            solverData.Solver.Maximize(targetFunction);
        }
    }
}