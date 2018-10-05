using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILP.Algorithm.Scheduling.Detail;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders;

namespace IRuettae.Core.ILP.Algorithm
{
    public class Starter
    {
        public static Route Optimise(object solverInputData, TargetBuilderType builderType = TargetBuilderType.Default,
            double MIP_GAP = 0, long timelimit = 0)
        {
            ISolver solver;
            switch (solverInputData)
            {
                case SolverInputData sid:
                    solver = new Solver(sid, TargetFunctionBuilderFactory.Create(builderType));
                    break;
                case Clustering.SolverInputData sid:
                    solver = new Clustering.Solver(sid, Clustering.TargetFunctionBuilders.TargetFunctionBuilderFactory.Create(builderType));
                    break;
                default:
                    throw new ArgumentException("SolverInputData not recognized");
            }


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

        public static void SaveMps(string path, object solverInputData, TargetBuilderType builderType = TargetBuilderType.Default)
        {
            ISolver solver;
            switch (solverInputData)
            {
                case SolverInputData sid:
                    solver = new Solver(sid, TargetFunctionBuilderFactory.Create(builderType));
                    break;
                case Clustering.SolverInputData sid:
                    solver = new Clustering.Solver(sid, Clustering.TargetFunctionBuilders.TargetFunctionBuilderFactory.Create(builderType));
                    break;
                default:
                    throw new ArgumentException("SolverInputData not recognized");
            }


            System.IO.File.WriteAllText(path, solver.ExportMPS());
        }
    }
}
