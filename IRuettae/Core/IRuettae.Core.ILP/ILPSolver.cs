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
            var phase1Result = clusterinSolver.Solve(0, timelimit);
            progress.Report(50);

            foreach (var santa in Enumerable.Range(0, phase1Result..GetLength(0)))
            {
                foreach (var day in Enumerable.Range(0, phase1Result.Waypoints.GetLength(1)))
                {
                    var cluster = phase1Result.Waypoints[santa, day];
                    schedulingSovlerVariableBuilders.Add(new SchedulingSolverVariableBuilder(routeCalculation.TimeSliceDuration, new List<Santa> { santas[santa] }, visits.Where(v => cluster.Select(w => w.RealVisitId).Contains(v.Id)).ToList(), new List<(DateTime, DateTime)> { routeCalculation.Days[day] }));
                }
            }

        }
    }
}
