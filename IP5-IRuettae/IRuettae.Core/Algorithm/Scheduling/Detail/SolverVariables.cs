using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.Scheduling.Detail
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

        ///// <summary>
        ///// [day][santa,Visit] is santa visiting Visit on day
        ///// </summary>
        //public GLS.Variable[][,] SantaDayVisit { get; set; }

        /// <summary>
        /// [day][santa,timeslice] is available
        /// </summary>
        public GLS.Variable[][,] Santas { get; set; }

        /// <summary>
        /// [day][Visit][timeslice] is this the time when the Visit starts
        /// </summary>
        public GLS.Variable[][][] VisitStart { get; set; }

        /// <summary>
        /// [day,santa] is used
        /// </summary>
        public GLS.Variable[,] UsesSanta { get; set; }

        /// <summary>
        /// [day][santa,timeslice] is on his way (visiting or walking around)
        /// warning: those values may be too high,
        /// if TargetType.MinTime is not part of the target function
        /// </summary>
        public GLS.Variable[][,] SantaEnRoute { get; set; }

        /// <summary>
        /// [day] number of santas needed
        /// </summary>
        public GLS.Variable[] NumberOfSantasNeeded { get; set; }

        /// <summary>
        /// number of santas needed overall (maximum on some day)
        /// warning: this value may be too high,
        /// if TargetType.MinSantas is not part of the target function
        /// </summary>
        public GLS.Variable NumberOfSantasNeededOverall { get; set; }
    }
}