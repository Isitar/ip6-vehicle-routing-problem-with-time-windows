using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm.RouteDistribution
{
    [Serializable]
    public class SolverInputData
    {
        /// <summary>
        /// [santa][route,day] cost of route
        /// </summary>
        public int[][,] RouteCost { set; get; }

        public int NumberOfSantas
        {
            get
            {
                return RouteCost == null ? 0 : RouteCost.Length;
            }
        }

        public int NumberOfRoutes
        {
            get
            {
                return NumberOfSantas == 0 ? 0 : RouteCost[0].GetLength(0);
            }
        }

        public int NumberOfDays
        {
            get
            {
                return NumberOfSantas == 0 ? 0 : RouteCost[0].GetLength(1);
            }
        }

    }
}
