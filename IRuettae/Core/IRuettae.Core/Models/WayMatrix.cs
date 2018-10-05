using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    /// <summary>
    /// Wrapper struct containing the route cost for all possible ways
    /// </summary>
    public struct WayMatrix
    {
        /// <summary>
        /// Matrix containing the route costs
        /// x -> y is on position [x,y], y->x on [y,x]
        /// </summary>
        public int[,] RouteCosts;
    }
}
