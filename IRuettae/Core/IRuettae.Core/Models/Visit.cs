using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    /// <summary>
    /// Represents a visit
    /// </summary>
    public struct Visit
    {
        /// <summary>
        /// Identifies the visit
        /// </summary>
        public int Id;

        /// <summary>
        /// tuple-array containing timestamps in seconds from when to when this visit has desired time
        /// </summary>
        public (int from, int to)[] Desired;

        /// <summary>
        /// tuple-array containing timestamps in seconds from when to when this visit has unavailable time
        /// </summary>
        public (int from, int to)[] Unavailable;

        /// <summary>
        /// Containing the visit duration in seconds
        /// </summary>
        public int Duration;
    }
}
