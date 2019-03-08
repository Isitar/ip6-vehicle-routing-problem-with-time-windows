using System;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    internal class BreakEvaluator : NodeEvaluator2
    {
        private readonly RoutingData data;
        private readonly int santa;

        /// <summary>
        /// requires data.SantaIds
        /// requires data.Visits
        /// requires data.HomeIndex
        /// </summary>
        /// <param name="data"></param>
        /// <param name="santa"></param>
        public BreakEvaluator(RoutingData data, int santa)
        {
            this.data = data ?? throw new ArgumentException("must not be null", "data");
            this.santa = santa;
        }

        /// <summary>
        /// Returns 1, if firstIndex is a break of santa
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        /// <returns></returns>
        public override long Run(int firstIndex, int secondIndex)
        {
            if (firstIndex >= data.Visits.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(firstIndex), "index must be smaller than numberOfVisits");
            }

            var visit = data.Visits[firstIndex];
            if (visit.IsBreak && visit.SantaId == santa)
            {
                return 1;
            }
            return 0;
        }
    }
}