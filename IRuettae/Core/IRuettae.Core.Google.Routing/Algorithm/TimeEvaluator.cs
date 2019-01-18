using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class TimeEvaluator : NodeEvaluator2
    {
        private readonly RoutingData data;

        /// <summary>
        /// requires data.Visits
        /// requires data.HomeIndex
        /// requires data.HomeIndexAdditional
        /// </summary>
        /// <param name="data"></param>
        public TimeEvaluator(RoutingData data)
        {
            this.data = data ?? throw new ArgumentException("data must not be null");
        }

        public override long Run(int firstIndex, int secondIndex)
        {
            if (firstIndex >= data.Visits.Count || secondIndex >= data.Visits.Count)
            {
                throw new ArgumentOutOfRangeException("index must be smaller than numberOfVisits");
            }

            bool firstIsHome = firstIndex == data.HomeIndex || firstIndex == data.HomeIndexAdditional;
            bool secondIsHome = secondIndex == data.HomeIndex || secondIndex == data.HomeIndexAdditional;

            if (firstIsHome && secondIsHome)
            {
                // home to home is zero
                return 0;
            }
            else if (firstIsHome)
            {
                return data.Visits[secondIndex].WayCostFromHome;
            }

            // first index is not home
            // -> add duration

            var firstVisit = data.Visits[firstIndex];
            var duration = firstVisit.Duration;
            if (secondIsHome)
            {
                return duration + firstVisit.WayCostToHome;
            }
            else
            {
                return duration + data.Input.RouteCosts[firstVisit.Id, data.Visits[secondIndex].Id];
            }
        }

    }
}
