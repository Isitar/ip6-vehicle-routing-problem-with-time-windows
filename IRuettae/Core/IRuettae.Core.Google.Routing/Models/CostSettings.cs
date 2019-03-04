namespace IRuettae.Core.Google.Routing.Models
{
    /// <summary>
    /// Settings regarding the cost function.
    /// Those costs are scaled to hours.
    /// </summary>
    public class CostSettings
    {
        private const int Hour = 3600;

        public int CostNotVisitedVisit { get; } = 560 * Hour;
        public int CostAdditionalSanta { get; } = 400 * Hour;
        public int CostAdditionalSantaPerHour { get; } = 40;
        public int CostVisitInUnavailablePerHour { get; } = 120;
        public int CostWayInUnavailablePerHour { get; } = 120;
        public int CostVisitInDesiredPerHour { get; } = 20;
        public int CostWorkPerHour { get; } = 40;
        public int CostLongestDayPerHour { get; } = 30;
    }
}
