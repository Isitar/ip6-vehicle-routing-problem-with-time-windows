using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm.NoTimeSlicing
{
    [Serializable]
    public class SolverInputData
    {
        /// <summary>
        /// [day][santa] is available
        /// </summary>
        public bool[,] Santas { get; }

        /// <summary>
        /// [visit] duration in time unit
        /// </summary>
        public int[] VisitsDuration { get; }

        /// <summary>
        /// [day,visit] is available
        /// </summary>
        public VisitState[,] Visits { get; }

        /// <summary>
        /// [from,to] distance in time unit
        /// </summary>
        public int[,] Distances { get; }

        /// <summary>
        /// [day] duration in time unit
        /// </summary>
        public int[] DayDuration { get; set; }

        public SolverInputData(bool[,] santas, int[] visitsDuration, VisitState[,] visits, int[,] distances, int[] dayDuration)
        {
            Santas = santas;
            VisitsDuration = visitsDuration;
            Visits = visits;
            Distances = distances;
            DayDuration = dayDuration;
        }


        /// <summary>
        /// [visit] name
        /// </summary>
        public string[] VisitNames { get; set; }
    }
}
