using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm
{
    public interface ISolver
    {
        ResultState Solve();
        string ExportMPS();
        string ImportMPS();
        Route GetResult();
    }
}
