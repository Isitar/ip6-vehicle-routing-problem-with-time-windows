using IRuettae.Core.ILP.Algorithm.Clustering.Detail;

namespace IRuettae.Core.ILP.Algorithm.Clustering.TargetFunctionBuilders
{
    internal abstract class AbstractTargetFunctionBuilder
    {
        public abstract void CreateTargetFunction(SolverData solverData);
    }
}
