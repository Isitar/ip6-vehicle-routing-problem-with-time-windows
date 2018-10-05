using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.ILP.Algorithm.Scheduling.Detail
{
    internal class SolverVariables
    {
        /// <summary>
        /// [day][santa][Visit,timeslice] is visiting
        /// </summary>
        public GLS.Variable[][][,] VisitsPerSanta { get; set; }

        /// <summary>
        /// [day][Visit,timeslice] is beeing visited by any santa
        /// </summary>
        public GLS.Variable[][,] Visits { get; set; }

        /// <summary>
        /// [santa,Visit] is santa visiting Visit
        /// </summary>
        public GLS.Variable[,] SantaVisits { get; set; }

        /// <summary>
        /// [day][Visit,timeslice] is this the time when the Visit starts
        /// </summary>
        public GLS.Variable[][,] VisitStart { get; set; }

        /// <summary>
        /// [day,santa] is used
        /// </summary>
        public GLS.Variable[,] UsesSanta { get; set; }

        /// <summary>
        /// [day][santa,timeslice] is on his way (visiting or walking around)
        /// warning: those values may be too high,
        /// if TargetType.OverallMinTime is not part of the target function
        /// </summary>
        public GLS.Variable[][,] SantaEnRoute { get; set; }
    }
}