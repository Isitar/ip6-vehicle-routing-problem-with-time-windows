using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    /// <summary>
    /// Wrapper class containing the route cost for all possible ways
    /// </summary>
    public class WayMatrix
    {
        /// <summary>
        /// Matrix containing the route costs
        /// x -> y is on position [x,y], y->x on [y,x]
        /// </summary>
        public int[,] RouteCosts;


        /// <summary>
        /// Route costs from x to y
        /// </summary>
        /// <param name="x">From this point</param>
        /// <param name="y">To this point</param>
        /// <returns>The route cost</returns>
        public int this[int x, int y]
        {
            get => RouteCosts[x, y];
            set => RouteCosts[x, y] = value;
        }

    }
}
