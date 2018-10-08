using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    /// <summary>
    /// Struct containing the optimisation input data
    /// </summary>
    public struct OptimizationInput
    {
        /// <summary>
        /// Visit input data
        /// </summary>
        public Visit[] Visits { get; set; }

        /// <summary>
        /// Santa input data
        /// </summary>
        public Santa[] Santas { get; set; }

        /// <summary>
        /// Days input data
        /// </summary>
        public (int from, int to)[] Days { get; set; }

        /// <summary>
        /// Route costs from x to y in seconds
        /// x -> y is on position [x,y]
        /// </summary>
        public int[,] RouteCosts { get; set; }
    }
}
