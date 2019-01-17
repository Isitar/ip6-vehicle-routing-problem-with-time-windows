using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Models
{
    public class RoutingData
    {
        public readonly OptimizationInput Input;

        public RoutingData(OptimizationInput input)
        {
            Input = input;
        }

        /// <summary>
        /// List of all santa Ids.
        /// The SantaIds.Count equals maxNumberOfSantas*numberOfDays.
        /// </summary>
        public List<int> SantaIds { get; set; } = new List<int>();

        /// <summary>
        /// List of all visits.
        /// Visit.SantaId of the elements in this list refer to an index of SantaIds.
        /// The Visits.Count equals the sum of normal visits and breaks*numberOfDays.
        /// </summary>
        public List<Visit> Visits { get; set; } = new List<Visit>();
        public int HomeIndex { get; internal set; }
        public int HomeIndexAdditional { get; internal set; }
    }
}
