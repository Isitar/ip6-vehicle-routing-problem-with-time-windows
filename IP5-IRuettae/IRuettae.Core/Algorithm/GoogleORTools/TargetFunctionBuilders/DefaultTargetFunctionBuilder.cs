using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.TargetFunctionBuilders
{
    class DefaultTargetFunctionBuilder : AbstractTargetFunctionBuilder
    {
        private GLS.LinearExpr targetFunction = new GLS.LinearExpr();

        public override void CreateTargetFunction(SolverData solverData)
        {
            var factory = new TargetFunctionFactory(solverData);

            targetFunction += factory.CreateTargetFunction(TargetType.MinTime, null);
            targetFunction += factory.CreateTargetFunction(TargetType.MinSantas, null);
            targetFunction += factory.CreateTargetFunction(TargetType.MinSantaShifts, null);

            solverData.Solver.Maximize(targetFunction);
        }
    }
}
