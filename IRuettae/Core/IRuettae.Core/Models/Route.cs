using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    /// <summary>
    /// Wrapper struct containing waypoints and a santa identifier
    /// </summary>
    public struct Route
    {
        /// <summary>
        /// All waypoints for this route
        /// </summary>
        public Waypoint[] Waypoints;

        /// <summary>
        /// Santa reference identifier
        /// </summary>
        public int SantaId;
    }
}
