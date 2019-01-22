using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class TimeEvaluator : NodeEvaluator2
    {
        private readonly RoutingData data;
        private readonly int costCoefficient;

        /// <summary>
        /// requires data.Visits
        /// requires data.HomeIndex
        /// requires data.HomeIndexAdditional
        /// </summary>
        /// <param name="data"></param>
        public TimeEvaluator(RoutingData data, int costCoefficient)
        {
            this.data = data ?? throw new ArgumentException("data must not be null");
            this.costCoefficient = costCoefficient;
        }

        public override long Run(int firstIndex, int secondIndex)
        {
            if (firstIndex >= data.Visits.Length || secondIndex >= data.Visits.Length)
            {
                throw new ArgumentOutOfRangeException("index must be smaller than numberOfVisits");
            }

            bool firstIsHome = data.Visits[firstIndex].Id == Constants.VisitIdHome;
            bool secondIsHome = data.Visits[secondIndex].Id == Constants.VisitIdHome;

            long time;
            if (firstIsHome && secondIsHome)
            {
                // home to home is zero
                time = 0;
            }
            else if (firstIsHome)
            {
                time = data.Visits[secondIndex].WayCostFromHome;
            }
            else
            {
                // first index is not home
                // -> add duration

                var firstVisit = data.Visits[firstIndex];
                var duration = firstVisit.Duration;
                if (secondIsHome)
                {
                    time = duration + firstVisit.WayCostToHome;
                }
                else
                {
                    time = duration + data.Input.RouteCosts[firstVisit.Id, data.Visits[secondIndex].Id];
                }
            }

            return time * costCoefficient;
        }

    }
}
