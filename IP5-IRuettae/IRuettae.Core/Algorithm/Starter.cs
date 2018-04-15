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
        public static Route Optimise(int[,] distances)
        {
            var solver = new Solver(distances, new DefaultTargetFunctionBuilder());
            var resultState = solver.Solve();
            switch (resultState)
            {
                case ResultState.Optimal:
                case ResultState.Feasible:
                    return solver.GetResult();
                case ResultState.Infeasible:
                default:
                    break;
            }
            return null;
        }
    }
}
