using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    /// <summary>
    /// Class containing the result of an optimisation algorithm
    /// </summary>
    public class OptimisationResult
    {
        /// <summary>
        /// Array containing all routes
        /// </summary>
        public Route[] Routes { get; set; }

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

        /// <summary>
        /// Time elapsed to calculate this result (in ms)
        /// </summary>
        public int TimeElapsed { get; set; }

        /// <summary>
        /// Returns the value of our cost_function for this result
        /// </summary>
        /// <returns></returns>
        public int MetricValue()
        {
            //todo: implement cost function
            return 0;
        }
    }
}
