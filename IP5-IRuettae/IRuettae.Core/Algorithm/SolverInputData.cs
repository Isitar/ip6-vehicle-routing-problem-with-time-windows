using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm
{
    public class SolverInputData
    {
        public int NumberOfSantas { get; }

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

        public SolverInputData(int numberOfSantas, int[] visitsLength, VisitState[][,] visits, int timesliceLength, int[,] distances)
        {
            NumberOfSantas = numberOfSantas;
            VisitsLength = visitsLength;
            Visits = visits;
            TimesliceLength = timesliceLength;
            Distances = distances;
        }
    }
}
