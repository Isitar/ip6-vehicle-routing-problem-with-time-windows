using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm
{
    public class SolverInputData
    {
        /// <summary>
        /// day * santa * timeslice (5min), is available
        /// </summary>
        public bool[][,] Santas { get; }

        /// <summary>
        /// [min]
        /// </summary>
        public int[] VisitsLength { get; }

        /// <summary>
        /// day * visit * timeslice (5min), is available
        /// </summary>
        public VisitState[][,] Visits { get; }

        /// <summary>
        /// [min]
        /// </summary>
        public int TimesliceLength { get; }

        /// <summary>
        /// 2d Array of all distances from - to, first element is the starting point [min]
        /// </summary>
        public int[,] Distances { get; }

        public SolverInputData(bool[][,] santas, int[] visitsLength, VisitState[][,] visits, int timesliceLength, int[,] distances)
        {
            Santas = santas;
            VisitsLength = visitsLength;
            Visits = visits;
            TimesliceLength = timesliceLength;
            Distances = distances;
        }

        public bool IsValid()
        {
            int numberOfSantas = Santas[0].GetLength(0);
            int numberOfDays = Santas.Length;
            int numberOfVisits = VisitsLength.Length;

            return true
                && Santas.All(day => day.GetLength(0) == numberOfSantas)
                && Santas.Zip(Visits, (a, b) => Tuple.Create(a, b)).All(e => e.Item1.GetLength(1) == e.Item2.GetLength(1))
                && Santas.Length == numberOfDays
                && VisitsLength.Length == numberOfVisits
                && Visits.All(v => v.GetLength(0) == numberOfVisits)
                && Distances.GetLength(0) == numberOfVisits
                && Distances.GetLength(1) == numberOfVisits
                ;
        }
    }
}
