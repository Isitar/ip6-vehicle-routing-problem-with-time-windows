using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm.Scheduling.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.Scheduling.TargetFunctionBuilders
{
    internal class DefaultTargetFunctionBuilder : AbstractTargetFunctionBuilder
    {
        private GLS.LinearExpr targetFunction = new GLS.LinearExpr();

        public override void CreateTargetFunction(SolverData solverData)
        {
            var factory = new TargetFunctionFactory(solverData);

            targetFunction += factory.CreateTargetFunction(TargetType.MinTime, null);
            targetFunction += factory.CreateTargetFunction(TargetType.TryVisitEarly, 1.0);
            targetFunction += factory.CreateTargetFunction(TargetType.TryVisitDesired, 0.5);

            solverData.Solver.Minimize(targetFunction);
        }
    }
}
