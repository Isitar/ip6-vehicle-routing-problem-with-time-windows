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
        /// Array containing all routes which have more than zero Waypoints
        /// </summary>
        public IEnumerable<Route> NonEmptyRoutes
        {
            get
            {
                return Routes.Where(r => r.Waypoints != null && r.Waypoints.Length > 0);
            }
        }

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
            var additionalSantaRoutes = NonEmptyRoutes.Where(r => additionalSantaIds.Contains(r.SantaId));
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
            return NonEmptyRoutes.Select(r => r.Waypoints.Max(wp => wp.StartTime) - r.Waypoints.Min(wp => wp.StartTime)).Sum();
        }

        public int LongestDay()
        {
            return NonEmptyRoutes.Select(r => r.Waypoints.Max(wp => wp.StartTime) - r.Waypoints.Min(wp => wp.StartTime)).Append(0).Max();
        }

        public int NumberOfNeededSantas()
        {
            return NonEmptyRoutes.Select(FindDay).GroupBy(d => d).Select(g => g.Count()).Append(0).Max();
        }

        public int NumberOfRoutes()
        {
            return NonEmptyRoutes.Count();
        }

        public int NumberOfVisits()
        {
            return OptimizationInput.Visits.Length;
        }

        public int TotalWayTime()
        {
            int totalTime = NonEmptyRoutes.Select(r => r.Waypoints.Last().StartTime - r.Waypoints[0].StartTime).Sum();
            return totalTime - TotalVisitTime();
        }

        public int TotalVisitTime()
        {
            var visitedVisits = Routes.SelectMany(r => r.Waypoints.Select(w => w.VisitId));
            return OptimizationInput.Visits.Where(v => visitedVisits.Contains(v.Id)).Select(v => v.Duration).Sum();
        }

        public int AverageWayTimePerRoute()
        {
            int numberOfRoutes = NumberOfRoutes();
            return numberOfRoutes > 0 ? TotalWayTime() / numberOfRoutes : 0;
        }

        public int AverageDurationPerRoute()
        {
            int numberOfRoutes = NumberOfRoutes();
            return numberOfRoutes > 0 ? (TotalVisitTime() + TotalWayTime()) / numberOfRoutes : 0;
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

        /// <summary>
        /// Returns how much the two intervals overlap
        /// </summary>
        /// <param name="intervals"></param>
        /// <returns></returns>
        private static int IntersectionLength(IEnumerable<(int from, int to)> intervals)
        {
            int startIntersection = intervals.Max(interval => interval.from);
            int endIntersection = intervals.Min(interval => interval.to);
            if (startIntersection < endIntersection)
            {
                return endIntersection - startIntersection;
            }
            return 0;
        }
    }
}
