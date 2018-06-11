using IRuettae.Core.Algorithm.Clustering.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.Clustering.TargetFunctionBuilders
{
    internal class MinAvgTimeTargetFunctionBuilder : AbstractTargetFunctionBuilder
    {
        private GLS.LinearExpr targetFunction = new GLS.LinearExpr();

        public override void CreateTargetFunction(SolverData solverData)
        {
            var factory = new TargetFunctionFactory(solverData);

            targetFunction += factory.CreateTargetFunction(TargetType.RealMinTimePerSanta, null) - factory.CreateTargetFunction(TargetType.Bonus, null) * 20 * 60; ;

            solverData.Solver.Minimize(targetFunction);
        }
    }
}