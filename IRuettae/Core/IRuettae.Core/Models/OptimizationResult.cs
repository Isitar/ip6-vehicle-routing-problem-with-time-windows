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
                                       - (20d / hour) * VisitTimeDesired()
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
                        unavailableSum += CalculateIntersection(startTime, endTime, from, to);
                    }

                }
            }

            return unavailableSum;
        }

        /// <summary>
        /// Returns how much the two intervals overlap
        /// </summary>
        /// <param name="from1"></param>
        /// <param name="to1"></param>
        /// <param name="from2"></param>
        /// <param name="to2"></param>
        /// <returns></returns>
        private int CalculateIntersection(int from1, int to1, int from2, int to2)
        {
            int startUnavailableTime = Math.Max(from1, from2);
            int endUnavailableTime = Math.Min(to1, to2);
            if (startUnavailableTime < endUnavailableTime)
            {
                return endUnavailableTime - startUnavailableTime;
            }
            return 0;
        }

        public int VisitTimeDesired()
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
                        desiredSum += CalculateIntersection(startTime, endTime, from, to);
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
            return Routes.Select(r => FindDay(r)).GroupBy(d => d).Select(g => g.Count()).Max();
        }

        public int NumberOfRoutes()
        {
            return Routes.Count();
        }

        public int NumberOfVisits()
        {
            return OptimizationInput.Visits.Count();
        }

        public int TotalWaytime()
        {
            int totalTime = Routes.Select(r => r.Waypoints.Last().StartTime - r.Waypoints[0].StartTime).Sum();
            return TotalVisitTime() - totalTime;
        }

        public int TotalVisitTime()
        {
            var visitedVisits = Routes.SelectMany(r => r.Waypoints.Select(w => w.VisitId));
            return OptimizationInput.Visits.Where(v => visitedVisits.Contains(v.Id)).Select(v => v.Duration).Sum();
        }

        public int AverageWaytimePerRoute()
        {
            return TotalWaytime() / NumberOfRoutes();
        }

        /// <summary>
        /// Returns the day from OptimizationInput.Days which correspondes to the Route
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private (int from, int to) FindDay(Route route)
        {
            foreach (var (from, to) in OptimizationInput.Days)
            {
                if (CalculateIntersection(route.Waypoints.First().StartTime, route.Waypoints.Last().StartTime, from, to) > 0)
                {
                    return (from, to);
                }
            }
            throw new ArgumentException("no matching day found");
        }
    }
}
