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
    public class CostEvaluator : NodeEvaluator2
    {
        private readonly RoutingData data;
        private readonly int costCoefficient;
        private readonly int startCost;
        private readonly TimeEvaluator timeEvaluator;

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="costCoefficient">way will be multiplied with this</param>
        /// <param name="startCost">Additional cost from home to any visit</param>
        public CostEvaluator(RoutingData data, int costCoefficient, int startCost)
        {
            timeEvaluator = new TimeEvaluator(data);

            this.data = data;
            this.costCoefficient = costCoefficient;
            this.startCost = startCost;
        }

        public override long Run(int firstIndex, int secondIndex)
        {
            var cost = timeEvaluator.Run(firstIndex, secondIndex) * costCoefficient;

            bool firstIsHome = data.Visits[firstIndex].Id == Constants.VisitIdHome;
            if (firstIsHome)
            {
                cost += startCost;
            }

            return cost;
        }

    }
}
