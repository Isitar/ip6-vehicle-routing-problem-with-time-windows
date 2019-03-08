using System;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class TimeEvaluator : NodeEvaluator2
    {
        protected readonly RoutingData Data;

        /// <summary>
        /// requires data.Visits
        /// requires data.HomeIndex
        /// </summary>
        /// <param name="data"></param>
        public TimeEvaluator(RoutingData data)
        {
            Data = data ?? throw new ArgumentException("must not be null", "data");
        }

        public override long Run(int firstIndex, int secondIndex)
        {
            if (firstIndex >= Data.Visits.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(firstIndex), "index must be smaller than numberOfVisits");
            }

            if (secondIndex >= Data.Visits.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(secondIndex), "index must be smaller than numberOfVisits");
            }

            bool firstIsHome = Data.Visits[firstIndex].Id == Constants.VisitIdHome;
            bool secondIsHome = Data.Visits[secondIndex].Id == Constants.VisitIdHome;

            long time;
            if (firstIsHome && secondIsHome)
            {
                // home to home is zero
                time = 0;
            }
            else if (firstIsHome)
            {
                time = Data.Visits[secondIndex].WayCostFromHome;
            }
            else
            {
                // first index is not home
                // -> add duration

                var firstVisit = Data.Visits[firstIndex];
                var duration = firstVisit.Duration;
                if (secondIsHome)
                {
                    time = duration + firstVisit.WayCostToHome;
                }
                else
                {
                    time = duration + Data.Input.RouteCosts[firstVisit.Id, Data.Visits[secondIndex].Id];
                }
            }

            return time;
        }

    }
}
