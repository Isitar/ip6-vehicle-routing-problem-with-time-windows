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

        /// <summary>
        /// Visits[HomeIndex] is the home for the normal santas.
        /// This home has no penalty term.
        /// </summary>
        public int HomeIndex { get; set; }

        /// <summary>
        /// Visits[HomeIndexAdditional] is the home for the additional santas.
        /// This home has a penalty term.
        /// </summary>
        public int HomeIndexAdditional { get; set; }
    }
}
