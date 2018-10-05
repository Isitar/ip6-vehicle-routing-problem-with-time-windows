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
    public struct OptimisationInput
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
        /// Way Matrix input data
        /// </summary>
        public WayMatrix WayMatrix { get; set; }
    }
}
