using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm
{
    [Serializable]
    public class SolverInputData
    {
        /// <summary>
        /// [day][santa,timeslice] is available
        /// </summary>
        public bool[][,] Santas { get; }

        /// <summary>
        /// [Visit] duration in timeslices
        /// </summary>
        public int[] VisitsDuration { get; }

        /// <summary>
        /// [day][Visit,timeslice] is available
        /// </summary>
        public VisitState[][,] Visits { get; }

        /// <summary>
        /// [from,to] distance in timeslices
        /// </summary>
        public int[,] Distances { get; }


        public long[] VisitIds { get; }

        public SolverInputData(bool[][,] santas, int[] visitsDuration, VisitState[][,] visits, int[,] distances, long[] visitIds)
        {
            Santas = santas;
            VisitsDuration = visitsDuration;
            Visits = visits;
            Distances = distances;
            VisitIds = visitIds;
        }

        public bool IsValid()
        {
            int numberOfSantas = Santas[0].GetLength(0);
            int numberOfDays = Santas.Length;
            int numberOfVisits = VisitsDuration.Length;

            return true
                && Santas.All(day => day.GetLength(0) == numberOfSantas)
                && Santas.Zip(Visits, (a, b) => Tuple.Create(a, b)).All(e => e.Item1.GetLength(1) == e.Item2.GetLength(1))
                && Santas.Length == numberOfDays
                && VisitsDuration.Length == numberOfVisits
                && Visits.All(v => v.GetLength(0) == numberOfVisits)
                && Distances.GetLength(0) == numberOfVisits
                && Distances.GetLength(1) == numberOfVisits
                ;
        }
    }
}
