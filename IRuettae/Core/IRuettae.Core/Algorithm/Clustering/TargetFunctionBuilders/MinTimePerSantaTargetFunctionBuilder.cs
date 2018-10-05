using IRuettae.Core.ILP.Algorithm.Clustering.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.ILP.Algorithm.Clustering.TargetFunctionBuilders
{
    internal class MinTimePerSantaTargetFunctionBuilder : AbstractTargetFunctionBuilder
    {
        private GLS.LinearExpr targetFunction = new GLS.LinearExpr();

        public override void CreateTargetFunction(SolverData solverData)
        {
            var factory = new TargetFunctionFactory(solverData);

            targetFunction += factory.CreateTargetFunction(TargetType.MinTimePerSanta, null) - factory.CreateTargetFunction(TargetType.Bonus, null) * 20 * 60;

            solverData.Solver.Minimize(targetFunction);
        }
    }
}
