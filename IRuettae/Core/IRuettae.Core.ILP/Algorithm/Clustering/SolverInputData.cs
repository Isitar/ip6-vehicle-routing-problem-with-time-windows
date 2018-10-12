using System;

namespace IRuettae.Core.ILP.Algorithm.Clustering
{
    [Serializable]
    public class SolverInputData
    {
        /// <summary>
        /// [day][santa] is available
        /// </summary>
        public bool[,] Santas { get; }

        /// <summary>
        /// [Visit] duration in time unit
        /// </summary>
        public int[] VisitsDuration { get; }

        /// <summary>
        /// [day,Visit] is available
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

        /// <summary>
        /// Breaks for this santa
        /// [santa] [Visit,Visit,Visit]
        /// </summary>
        public int[][] SantaBreaks { get; }

        public SolverInputData(bool[,] santas, int[] visitsDuration, VisitState[,] visits, int[,] distances, int[] dayDuration, int[][]  santaBreaks)
        {
            Santas = santas;
            VisitsDuration = visitsDuration;
            Visits = visits;
            Distances = distances;
            DayDuration = dayDuration;
            SantaBreaks = santaBreaks;
        }


        /// <summary>
        /// [Visit] name
        /// </summary>
        public string[] VisitNames { get; set; }

        /// <summary>
        /// [Visit] id
        /// </summary>
        public long[] VisitIds{ get; set; }

        public DateTime[] DayStartingTime { get; set; }

    }
}
