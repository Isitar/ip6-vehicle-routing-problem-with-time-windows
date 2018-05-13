using IRuettae.Core.Algorithm.TimeSlicing.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.TimeSlicing.TargetFunctionBuilders
{
    internal class MinTimeOnlyTargetFunctionBuilder : AbstractTargetFunctionBuilder
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