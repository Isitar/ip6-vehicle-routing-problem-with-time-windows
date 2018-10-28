using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILP.Algorithm.Scheduling.Detail;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders
{
    internal interface ITargetFunctionBuilder
    {
        void CreateTargetFunction(SolverData solverData);
    }
}
