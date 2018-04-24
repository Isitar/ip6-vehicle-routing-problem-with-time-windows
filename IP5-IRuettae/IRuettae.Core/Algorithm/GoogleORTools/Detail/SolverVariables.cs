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
        public GLS.Variable[][][,] VisitsPerSanta { get; set; }

        /// <summary>
        /// [day][visit,timeslice] is beeing visited by any santa
        /// </summary>
        public GLS.Variable[][,] Visits { get; set; }

        /// <summary>
        /// [santa,visit] is santa visiting visit
        /// </summary>
        public GLS.Variable[,] SantaVisits { get; set; }

        /// <summary>
        /// [day][santa,timeslice] is available
        /// </summary>
        public GLS.Variable[][,] Santas { get; set; }

        /// <summary>
        /// [day][visit][timeslice]
        /// </summary>
        public GLS.Variable[][][] DebugStarts { get; set; }

        /// <summary>
        /// [day,santa] is used
        /// </summary>
        public GLS.Variable[,] UsesSanta { get; set; }
    }
}