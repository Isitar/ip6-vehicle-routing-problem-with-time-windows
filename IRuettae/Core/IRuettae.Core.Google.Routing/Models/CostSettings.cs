using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Google.Routing.Models
{
    /// <summary>
    /// Settings regarding the cost function
    /// </summary>
    public class CostSettings
    {
        public int CostNotVisitedVisit { get; } = 560;
        public int CostAdditionalSanta { get; } = 400;
        public int CostAdditionalSantaPerHour { get; } = 40;
        public int CostVisitInUnavailablePerHour { get; } = 120;
        public int CostWayInUnavailablePerHour { get; } = 120;
        public int CostVisitInDesiredPerHour { get; } = 20;
        public int CostWorkPerHour { get; } = 40;
        public int CostLongestDayPerHour { get; } = 30;
    }
}
