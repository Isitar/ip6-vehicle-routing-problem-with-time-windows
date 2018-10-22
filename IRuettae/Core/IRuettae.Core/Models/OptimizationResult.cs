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
        public int MetricValue()
        {
            const int hour = 3600;
            return (int)(Math.Ceiling(560 * NumberOfNonVisitedFamilies()
                                       + 400 * NumberOfAdditionalSantas()
                                       + (40d / hour) * AdditionalSantaWorkTime())
                                       + (120d / hour) * VisitTimeInUnavailabe()
                                       - (20d / hour) * VisitTimeDesired()
                                       + (40d / hour) * SantaWorkTime()
                                       + (30d / hour) * LongestDay()

                );
        }

        public int NumberOfNonVisitedFamilies()
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
                    try
                    {
                        var visit = OptimizationInput.Visits.First(v => v.Id == waypoint.VisitId);
                    }
                    catch 
                    {

                    }
                    

                }
            }

            return 0;
        }

        public int VisitTimeDesired()
        {
            // todo: implement
            return 0;
        }

        public int SantaWorkTime()
        {
            return Routes.Select(r => r.Waypoints.Max(wp => wp.StartTime) - r.Waypoints.Min(wp => wp.StartTime)).Sum();
        }

        public int LongestDay()
        {
            return Routes.Select(r => r.Waypoints.Max(wp => wp.StartTime) - r.Waypoints.Min(wp => wp.StartTime)).Max();
        }
    }
}
