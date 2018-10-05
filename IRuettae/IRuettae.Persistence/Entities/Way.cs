using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    /// <summary>
    /// Directional way between two visits
    /// </summary>
    public class Way : BaseEntity
    {
        public virtual Visit From { get; set; }
        public virtual Visit To { get; set; }
        /// <summary>
        /// Distance in meters
        /// </summary>
        public virtual int Distance { get; set; }
        /// <summary>
        /// Duration in seconds
        /// </summary>
        public virtual int Duration { get; set; }
    }
}
