using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    public enum ResultState
    {
        Finished,
        TimelimitReached,
        Cancelled,
        Error
    }

    /// <summary>
    /// Class containing the result of an optimization algorithm
    /// </summary>
    public class OptimizationResult
    {

        /// <summary>
        /// The state of the result
        /// </summary>
        public ResultState ResultState { get; set; }

        /// <summary>
        /// Array containing all routes
        /// </summary>
        public Route[] Routes { get; set; }

        /// <summary>
        /// The input used to calculate this result
        /// </summary>
        public OptimizationInput OptimizationInput { get; set; }

        /// <summary>
        /// Time elapsed to calculate this result (in s)
        /// </summary>
        public long TimeElapsed { get; set; }

        /// <summary>
        /// Returns the value of our cost_function for this result
        /// </summary>
        /// <returns></returns>
        public int Cost()
        {
            const int hour = 3600;
            return (int)(Math.Ceiling(
                                       +560 * NumberOfNotVisitedFamilies()
                                       + 400 * NumberOfAdditionalSantas()
                                       + (40d / hour) * AdditionalSantaWorkTime())
                                       + (120d / hour) * VisitTimeInUnavailabe()
                                       - (20d / hour) * VisitTimeInDesired()
                                       + (40d / hour) * SantaWorkTime()
                                       + (30d / hour) * LongestDay()
                );
        }

        public int NumberOfNotVisitedFamilies()
        {
            var visitedVisits = Routes.SelectMany(r => r.Waypoints.Select(w => w.VisitId));
            return OptimizationInput.Visits.Count(v => !visitedVisits.Contains(v.Id));
        }

        public int NumberOfAdditionalSantas()
        {
            var additionalSantaIds = Routes.Where(r => !OptimizationInput.Santas.Select(s => s.Id).Contains(r.SantaId))
                .Select(r => r.SantaId)
                .Distinct().ToList();
            return additionalSantaIds.Count;
        }

        public int AdditionalSantaWorkTime()
        {
            var additionalSantaIds = Routes.Where(r => !OptimizationInput.Santas.Select(s => s.Id).Contains(r.SantaId))
                .Select(r => r.SantaId)
                .Distinct().ToList();
            var additionalSantaRoutes = Routes.Where(r => additionalSantaIds.Contains(r.SantaId));
            return additionalSantaRoutes.Select(r =>
                    r.Waypoints.Max(wp => wp.StartTime) - r.Waypoints.Min(wp => wp.StartTime))
                .Sum();
        }

        public int VisitTimeInUnavailabe()
        {
            var unavailableSum = 0;
            foreach (var route in Routes)
            {
                foreach (var waypoint in route.Waypoints)
                {

                    var visit = OptimizationInput.Visits.Cast<Visit?>().FirstOrDefault(v => v != null && v.Value.Id == waypoint.VisitId);
                    if (!visit.HasValue) { continue; }

                    int startTime = waypoint.StartTime;
                    int endTime = startTime + visit.Value.Duration;
                    foreach (var (from, to) in visit.Value.Unavailable)
                    {
                        unavailableSum += IntersectionLength(new[] { (startTime, endTime), (from, to) });
                    }

                }
            }

            return unavailableSum;
        }

        /// <summary>
        /// Returns how much the two intervals overlap
        /// </summary>
        /// <param name="intervals"></param>
        /// <returns></returns>
        private int IntersectionLength(IEnumerable<(int from, int to)> intervals)
        {
            int startIntersection = intervals.Max(interval => interval.from);
            int endIntersection = intervals.Min(interval => interval.to);
            if (startIntersection < endIntersection)
            {
                return endIntersection - startIntersection;
            }
            return 0;
        }

        public int VisitTimeInDesired()
        {
            var desiredSum = 0;

            foreach (var route in Routes)
            {
                foreach (var waypoint in route.Waypoints)
                {

                    var visit = OptimizationInput.Visits.Cast<Visit?>().FirstOrDefault(v => v != null && v.Value.Id == waypoint.VisitId);
                    if (!visit.HasValue) { continue; }

                    int startTime = waypoint.StartTime;
                    int endTime = startTime + visit.Value.Duration;
                    foreach (var (from, to) in visit.Value.Desired)
                    {
                        desiredSum += IntersectionLength(new[] { (startTime, endTime), (from, to) });
                    }

                }
            }

            return desiredSum;
        }

        public int SantaWorkTime()
        {
            return Routes.Select(r => r.Waypoints.Max(wp => wp.StartTime) - r.Waypoints.Min(wp => wp.StartTime)).Sum();
        }

        public int LongestDay()
        {
            return Routes.Select(r => r.Waypoints.Max(wp => wp.StartTime) - r.Waypoints.Min(wp => wp.StartTime)).Max();
        }

        public int NumberOfNeededSantas()
        {
            return Routes.Select(FindDay).GroupBy(d => d).Select(g => g.Count()).Max();
        }

        public int NumberOfRoutes()
        {
            return Routes.Length;
        }

        public int NumberOfVisits()
        {
            return OptimizationInput.Visits.Length;
        }

        public int TotalWayTime()
        {
            int totalTime = Routes.Select(r => r.Waypoints.Last().StartTime - r.Waypoints[0].StartTime).Sum();
            return totalTime - TotalVisitTime();
        }

        public int TotalVisitTime()
        {
            var visitedVisits = Routes.SelectMany(r => r.Waypoints.Select(w => w.VisitId));
            return OptimizationInput.Visits.Where(v => visitedVisits.Contains(v.Id)).Select(v => v.Duration).Sum();
        }

        public int AverageWayTimePerRoute()
        {
            return TotalWayTime() / NumberOfRoutes();
        }

        public int AverageDurationPerRoute()
        {
            return (TotalVisitTime() + TotalWayTime()) / NumberOfRoutes();
        }

        /// <summary>
        /// Returns the day from OptimizationInput.Days which correspondes to the Route
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private (int from, int to) FindDay(Route route)
        {
            foreach (var day in OptimizationInput.Days)
            {
                if (IntersectionLength(new[] { (route.Waypoints.First().StartTime, route.Waypoints.Last().StartTime), day }) > 0)
                {
                    return day;
                }
            }
            throw new ArgumentException("no matching day found");
        }
    }
}
