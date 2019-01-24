using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.LocalSolver.Algorithm
{
    internal struct VRPTWSolution
    {
        public int[][] SantaVisitSequence;
        public int[][] SantaVisitStartTime;
        public int[] SantaWaitBeforeStart;
    }
}
