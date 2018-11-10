using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILP.Algorithm
{
    public interface ISolver
    {
        ResultState Solve();
        ResultState Solve(double MIP_GAP, long timelimitMiliseconds);
        string ExportMPS();
        string ImportMPS();
        Route GetResult();

        double SolutionValue();
    }
}
