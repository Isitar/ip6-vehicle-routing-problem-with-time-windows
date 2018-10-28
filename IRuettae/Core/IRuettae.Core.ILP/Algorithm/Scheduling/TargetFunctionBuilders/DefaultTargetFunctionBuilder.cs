using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILP.Algorithm.Scheduling.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders
{
    internal class DefaultTargetFunctionBuilder : ITargetFunctionBuilder
    {
        private GLS.LinearExpr targetFunction = new GLS.LinearExpr();

        public void CreateTargetFunction(SolverData solverData)
        {
            var factory = new TargetFunctionFactory(solverData);

            targetFunction += factory.CreateTargetFunction(TargetType.MinTime, 40);
            targetFunction += factory.CreateTargetFunction(TargetType.TryVisitDesired, 20);

            solverData.Solver.Minimize(targetFunction);
        }
    }
}
