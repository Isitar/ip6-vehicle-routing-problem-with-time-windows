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
    public struct Visit : ICloneable
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

        /// <summary>
        /// The cost to get from the depot (home) to this visit
        /// </summary>
        public int WayCostFromHome;

        /// <summary>
        /// The cost to get from this visit to the depot (home)
        /// </summary>
        public int WayCostToHome;

        /// <summary>
        /// If this visit is a break, the santa Id will be set.
        /// </summary>
        public int SantaId;

        /// <summary>
        /// If it's a santa break or not
        /// </summary>
        public bool IsBreak;

        /// <summary>
        /// Deep copy
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var ret = (Visit)MemberwiseClone();
            ret.Desired = Desired.Select(v => (v.from, v.to)).ToArray();
            ret.Unavailable = Unavailable.Select(v => (v.from, v.to)).ToArray();
            return ret;
        }
    }
}
