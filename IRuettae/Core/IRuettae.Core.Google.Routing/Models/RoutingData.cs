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
        /// The SantaIds.Length equals maxNumberOfSantas*numberOfDays.
        /// </summary>
        public int[] SantaIds { get; set; }

        /// <summary>
        /// List of all visits.
        /// Visit.SantaId of the elements in this list refer to an index of SantaIds.
        /// The Visits.Length equals the sum of normal visits and breaks*numberOfDays.
        /// </summary>
        public Visit[] Visits { get; set; }

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

        /// <summary>
        /// Intervals in which a visit start can not be.
        /// Unavailable[visitId][interval].
        /// Unavailable.Length equals Visits.Length.
        /// </summary>
        public (int startFrom, int startEnd)[][] Unavailable { get; set; }

        /// <summary>
        /// Index of the visit which is the start of the santa.
        /// SantaStartIndex[santaIndex].
        /// </summary>
        public int[] SantaStartIndex { get; set; }

        /// <summary>
        /// Index of the visit which is the end of the santa.
        /// SantaEndIndex[santaIndex].
        /// </summary>
        public int[] SantaEndIndex { get; set; }
    }
}
