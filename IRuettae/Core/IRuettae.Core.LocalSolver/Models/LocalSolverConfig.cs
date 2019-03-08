using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.LocalSolver.Models
{
    public class LocalSolverConfig : IStarterData
    {
        public double VrpTimeLimitFactor { get; set; }
        public double VrptwTimeLimitFactor { get; set; }
        public int MaxNumberOfAdditionalSantas { get; set; } = 0;
    }
}
