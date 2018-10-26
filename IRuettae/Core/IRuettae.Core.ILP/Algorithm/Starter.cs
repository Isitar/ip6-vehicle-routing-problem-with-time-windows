using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILP.Algorithm.Scheduling.Detail;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders;
using IRuettae.Core.ILP.Algorithm.Models;

namespace IRuettae.Core.ILP.Algorithm
{
    /// <summary>
    /// only for testing purposes
    /// </summary>
    public class Starter
    {
        public static Route Optimise(Clustering.SolverInputData solverInputData, ClusteringOptimizationGoals goal,
            double MIP_GAP = 0, long timelimit = 0)
        {
            ISolver solver = new Clustering.Solver(solverInputData);

            var resultState = solver.Solve(MIP_GAP, timelimit);
            switch (resultState)
            {
                case ResultState.Optimal:
                case ResultState.Feasible:
                    return solver.GetResult();
                case ResultState.Unknown:
                case ResultState.NotSolved:
                case ResultState.Infeasible:
                    break;
                default:
                    Console.WriteLine("Warning: Api changed, new result state");
                    break;
            }

            return null;
        }
        public static Route Optimise(SolverInputData solverInputData, SchedulingOptimizationGoals builderType = SchedulingOptimizationGoals.Default,
            double MIP_GAP = 0, long timelimit = 0)
        {
            ISolver solver = new Solver(solverInputData, TargetFunctionBuilderFactory.Create(builderType));

            var resultState = solver.Solve(MIP_GAP, timelimit);
            switch (resultState)
            {
                case ResultState.Optimal:
                case ResultState.Feasible:
                    return solver.GetResult();
                case ResultState.Unknown:
                case ResultState.NotSolved:
                case ResultState.Infeasible:
                    break;
                default:
                    Console.WriteLine("Warning: Api changed, new result state");
                    break;
            }

            return null;
        }

        public static void SaveMps(string path, Clustering.SolverInputData solverInputData, Models.ClusteringOptimizationGoals goal)
        {
            ISolver solver = new Clustering.Solver(solverInputData);

            System.IO.File.WriteAllText(path, solver.ExportMPS());
        }

        public static void SaveMps(string path, SolverInputData solverInputData, SchedulingOptimizationGoals builderType = SchedulingOptimizationGoals.Default)
        {
            ISolver solver = new Solver(solverInputData, TargetFunctionBuilderFactory.Create(builderType));

            System.IO.File.WriteAllText(path, solver.ExportMPS());
        }
    }
}
