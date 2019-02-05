using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm
{
    public interface ISolver
    {
        ResultState Solve();
        ResultState Solve(double MIP_GAP, long timelimitMiliseconds);
        Route GetResult();

        double SolutionValue();
    }
}
