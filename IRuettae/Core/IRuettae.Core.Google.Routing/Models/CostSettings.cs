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
        public const int CostNotVisitedVisit = 560;
        public const int CostAdditionalSanta = 400;
        public const int CostAdditionalSantaPerHour = 40;
        public const int CostVisitInUnavailablePerHour = 120;
        public const int CostWayInUnavailablePerHour = 120;
        public const int CostVisitInDesiredPerHour = 20;
        public const int CostWorkPerHour = 40;
        public const int CostLongestDayPerHour = 30;
    }
}
