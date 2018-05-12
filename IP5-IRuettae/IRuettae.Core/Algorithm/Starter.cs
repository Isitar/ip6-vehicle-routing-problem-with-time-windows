using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm.GoogleORTools;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;
using IRuettae.Core.Algorithm.GoogleORTools.TargetFunctionBuilders;

namespace IRuettae.Core.Algorithm
{
    public class Starter
    {
        public static Route Optimise(object solverInputData, TargetBuilderType builderType = TargetBuilderType.Default, bool useNewSovler =false)
        {
            //if (!solverInputData.IsValid())
            //{
            //    throw new ArgumentException();
            //}

            ISolver solver;
            if (useNewSovler)
            {
                solver = new NoTimeSlicing.Solver((NoTimeSlicing.SolverInputData) solverInputData,
                    NoTimeSlicing.TargetFunctionBuilders.TargetFunctionBuilderFactory.Create(builderType));
            }
            else
            {
                solver = new Solver((SolverInputData) solverInputData, TargetFunctionBuilderFactory.Create(builderType));
            }
            
        
            var resultState = solver.Solve();
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

        public static void SaveMps(string path,
            SolverInputData solverInputData,
            TargetBuilderType builderType = TargetBuilderType.Default
            )
        {
            var solver = new Solver(solverInputData, TargetFunctionBuilderFactory.Create(builderType));
            System.IO.File.WriteAllText(path, solver.ExportMPS());
        }
    }
}
