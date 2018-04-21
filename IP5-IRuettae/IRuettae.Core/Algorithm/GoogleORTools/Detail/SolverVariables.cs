using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class SolverVariables
    {
        /// <summary>
        /// [day][santa,visit,timeslice] is visiting
        /// </summary>
        public GLS.Variable[][,,] Visits { get; set; }
    }
}