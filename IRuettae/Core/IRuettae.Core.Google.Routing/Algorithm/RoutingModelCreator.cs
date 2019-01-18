using System;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class RoutingModelCreator
    {
        private readonly RoutingData data;

        public RoutingModelCreator(RoutingData data)
        {
            this.data = data;
        }

        /// <summary>
        /// requires data.SantaIds
        /// requires data.Visits
        /// requires data.HomeIndex
        /// requires data.HomeIndexAdditional
        /// requires data.Unavailable
        /// requires data.Start
        /// requires data.End
        /// creates data.RoutingModel
        /// </summary>
        public void Create()
        {
            var model =
                new RoutingModel(data.Visits.Length, data.SantaIds.Length,
                                 data.SantaStartIndex, data.SantaEndIndex);

            // setting up dimensions
            var maxTime = GetMaxTime();
            var timeEvaluator = new TimeEvaluator(data);
            model.AddDimension(timeEvaluator, maxTime, maxTime, false, "time");

            data.RoutingModel = model;
        }

        /// <summary>
        /// Returns the duration of the longest possible day.
        /// </summary>
        /// <returns></returns>
        public int GetMaxTime()
        {
            var longestDay = int.MinValue;
            foreach (var (from, to) in data.Input.Days)
            {
                longestDay = Math.Max(longestDay, to - from);
            }

            return longestDay + 2 * data.Input.MaxWayDuration();
        }
    }
}