using IRuettae.Core.Algorithm.Clustering.Detail;

namespace IRuettae.Core.Algorithm.Clustering.TargetFunctionBuilders
{
    internal abstract class AbstractTargetFunctionBuilder
    {
        public abstract void CreateTargetFunction(SolverData solverData);
    }
}
