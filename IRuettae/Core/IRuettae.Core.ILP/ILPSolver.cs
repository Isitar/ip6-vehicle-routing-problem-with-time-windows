using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILP.Algorithm.Clustering.TargetFunctionBuilders;
using IRuettae.Core.Models;
using IRuettae.Preprocessing.Mapping;

namespace IRuettae.Core.ILP
{
    public class ILPSolver : ISolver
    {
        private readonly OptimisationInput input;

        public ILPSolver(OptimisationInput input)
        {
            this.input = input;
        }

        public OptimisationResult Solve(int timelimit, IProgress<int> progress)
        {
            var clusteringSolverVariableBuilder = new ClusteringSolverVariableBuilder(input, 0);
            var clusteringSolverInputData = clusteringSolverVariableBuilder.Build();
            var clusterinSolver =
                new Algorithm.Clustering.Solver(clusteringSolverInputData, new MinAvgTimeTargetFunctionBuilder());
            var solutions = clusterinSolver.Solve(0, timelimit);
            progress.Report(50);



        }
    }
}
