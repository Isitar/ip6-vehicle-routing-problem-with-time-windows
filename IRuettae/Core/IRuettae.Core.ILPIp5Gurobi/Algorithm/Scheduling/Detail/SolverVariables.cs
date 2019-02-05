using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling.Detail
{
    internal class SolverVariables
    {
        /// <summary>
        /// [day][santa][Visit][timeslice] is visiting
        /// </summary>
        public GRBVar[][][][] VisitsPerSanta { get; set; }

        /// <summary>
        /// [day][Visit][timeslice] is beeing visited by any santa
        /// </summary>
        public GRBVar[][][] Visits { get; set; }

        /// <summary>
        /// [santa][Visit] is santa visiting Visit
        /// </summary>
        public GRBVar[][] SantaVisits { get; set; }

        /// <summary>
        /// [day][Visit][timeslice] is this the time when the Visit starts
        /// </summary>
        public GRBVar[][][] VisitStart { get; set; }

        /// <summary>
        /// [day][santa] is used
        /// </summary>
        public GRBVar[][] UsesSanta { get; set; }

        /// <summary>
        /// [day][santa][timeslice] is on his way (visiting or walking around)
        /// warning: those values may be too high,
        /// if TargetType.OverallMinTime is not part of the target function
        /// </summary>
        public GRBVar[][][] SantaEnRoute { get; set; }
    }
}