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
        /// [day][santa][visit,timeslice] is visiting
        /// </summary>
        public GLS.Variable[][][,] Visits { get; set; }

        /// <summary>
        /// [santa,visit] number of slices, the santa is visiting visit
        /// </summary>
        public GLS.Variable[,] SantaVisits { get; set; }

        /// <summary>
        /// [day][santa,timeslice] is available
        /// </summary>
        public GLS.Variable[][,] Santas { get; set; }

        /// <summary>
        /// [santa][from,to] is using way
        /// </summary>
        public GLS.Variable[][,] UsesWay { get; set; }
    }
}