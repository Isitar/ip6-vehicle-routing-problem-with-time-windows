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
        /// Array containing all routes which contain at least one visit
        /// </summary>
        public IEnumerable<Route> NonEmptyRoutes
        {
            get
            {
                return Routes.Where(r => r.Waypoints != null && r.Waypoints.Any(wp => wp.VisitId != Constants.VisitIdHome));
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
            var cost =
                560d * NumberOfNotVisitedFamilies()
                + 560d * NumberOfMissingBreaks()
                + 400d * NumberOfAdditionalSantas()
                + (40d / hour) * AdditionalSantaWorkTime()
                + (120d / hour) * VisitTimeInUnavailable()
                + (120d / hour) * WayTimeOutsideBusinessHours()
                - (20d / hour) * VisitTimeInDesired()
                + (40d / hour) * SantaWorkTime()
                + (30d / hour) * LongestDay();
            return (int)Math.Ceiling(cost);
        }

        public virtual int NumberOfNotVisitedFamilies()
        {
            var visitedVisits = NonEmptyRoutes.SelectMany(r => r.Waypoints.Select(w => w.VisitId));
            return OptimizationInput.Visits.Count(v => !v.IsBreak && !visitedVisits.Contains(v.Id));
        }

        public virtual int NumberOfMissingBreaks()
        {
            var santaBreaks = new Dictionary<int, int>();
            foreach (var v in OptimizationInput.Visits.Where(v => v.IsBreak))
            {
                if (santaBreaks.ContainsKey(v.SantaId))
                {
                    throw new InvalidOperationException("each santa can only have at most one break");
                }
                santaBreaks.Add(v.SantaId, v.Id);
            }

            return NonEmptyRoutes.Count(r => santaBreaks.ContainsKey(r.SantaId)
                    && r.Waypoints.All(wp => wp.VisitId != santaBreaks[r.SantaId]));
        }

        public int NumberOfAdditionalSantas()
        {
            var additionalSantaIds = NonEmptyRoutes.Where(r => !OptimizationInput.Santas.Select(s => s.Id).Contains(r.SantaId))
                .Select(r => r.SantaId)
                .Distinct().ToList();
            return additionalSantaIds.Count;
        }

        public int AdditionalSantaWorkTime()
        {
            var additionalSantaIds = NonEmptyRoutes.Where(r => !OptimizationInput.Santas.Select(s => s.Id).Contains(r.SantaId))
                .Select(r => r.SantaId)
                .Distinct().ToList();
            var additionalSantaRoutes = NonEmptyRoutes.Where(r => additionalSantaIds.Contains(r.SantaId));
            return additionalSantaRoutes.Select(r =>
                    r.Waypoints.Max(wp => wp.StartTime) - r.Waypoints.Min(wp => wp.StartTime))
                .Sum();
        }

        public int VisitTimeInUnavailable()
        {
            var unavailableSum = 0;
            foreach (var route in NonEmptyRoutes)
            {
                foreach (var waypoint in route.Waypoints.Where(wp => wp.VisitId >= 0))
                {
                    var visit = OptimizationInput.Visits[waypoint.VisitId];

                    int startTime = waypoint.StartTime;
                    int endTime = startTime + visit.Duration;
                    foreach (var (from, to) in visit.Unavailable)
                    {
                        unavailableSum += IntersectionLength(startTime, endTime, from, to);
                    }
                }
            }

            return unavailableSum;
        }

        public int WayTimeOutsideBusinessHours()
        {
            var sum = 0;
            foreach (var route in NonEmptyRoutes)
            {
                var day = FindDay(route);

                // home, with duration = 0
                var endOfPreviousVisit = route.Waypoints[0].StartTime;
                foreach (var waypoint in route.Waypoints.Skip(1))
                {
                    var way = (from: endOfPreviousVisit, to: waypoint.StartTime);
                    sum += (way.to - way.from) - IntersectionLength(day.from, day.to, way.from, way.to);

                    var id = waypoint.VisitId;
                    if (id < 0)
                    {
                        continue;
                    }

                    endOfPreviousVisit = waypoint.StartTime + OptimizationInput.Visits[id].Duration;
                }
            }

            return sum;
        }

        public int VisitTimeInDesired()
        {
            var desiredSum = 0;

            foreach (var route in NonEmptyRoutes)
            {
                foreach (var waypoint in route.Waypoints.Where(wp => wp.VisitId >= 0))
                {
                    var visit = OptimizationInput.Visits[waypoint.VisitId];

                    int startTime = waypoint.StartTime;
                    int endTime = startTime + visit.Duration;
                    foreach (var (from, to) in visit.Desired)
                    {
                        desiredSum += IntersectionLength(startTime, endTime, from, to);
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
            var visitedVisits = NonEmptyRoutes.SelectMany(r => r.Waypoints.Select(w => w.VisitId));
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
                if (IntersectionLength(route.Waypoints.First().StartTime, route.Waypoints.Last().StartTime, day.from, day.to) > 0)
                {
                    return day;
                }
            }
            throw new ArgumentException("no matching day found");
        }

        /// <summary>
        /// Returns how much the two intervals overlap
        /// </summary>
        /// <param name="start1">start of first interval</param>
        /// <param name="end1">end of first interval</param>
        /// <param name="start2">start of second interval</param>
        /// <param name="end2">end of second interval</param>
        /// <returns></returns>
        private static int IntersectionLength(int start1, int end1, int start2, int end2)
        {
            int startIntersection = Math.Max(start1, start2);
            int endIntersection = Math.Min(end1, end2);
            if (startIntersection < endIntersection)
            {
                return endIntersection - startIntersection;
            }
            return 0;
        }

        /// <summary>
        /// Returns true if this OptimizationResult is valid.
        /// Otherwise false.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Validate() == null;
        }

        /// <summary>
        /// Returns null if this OptimizationResult is valid.
        /// Otherwise, returns an error message.
        /// This function returns immediatly if anything invalid is found.
        /// </summary>
        /// <returns></returns>
        public string Validate()
        {
            List<Route> routes = NonEmptyRoutes.ToList();

            // check santa is only used once per day
            {
                var multipleUses = routes.GroupBy(r => (r.SantaId, FindDay(r))).Where(g => g.Count() > 1).ToList();
                if (multipleUses.Count > 0)
                {
                    return $"Santa {multipleUses.First().Key} is used more than once on the same day.";
                }
            }

            // validate starts
            {
                var wrongRoutes = routes.Where(r => r.Waypoints.First().VisitId != Constants.VisitIdHome).ToList();
                if (wrongRoutes.Count > 0)
                {
                    var (from, to) = FindDay(wrongRoutes.First());
                    return $"Wrong start in route of santa {wrongRoutes.First().SantaId} on day {from}-{to}.";
                }
            }

            // validate ends
            {
                var wrongRoutes = routes.Where(r => r.Waypoints.Last().VisitId != Constants.VisitIdHome).ToList();
                if (wrongRoutes.Count > 0)
                {
                    var (from, to) = FindDay(wrongRoutes.First());
                    return $"Wrong end in route of santa {wrongRoutes.First().SantaId} on day {from}-{to}.";
                }
            }

            // check way from home to first visit
            {
                var wrongStartWays = routes.Where(r =>
                {
                    var visit = r.Waypoints.ElementAt(1);
                    return OptimizationInput.Visits[visit.VisitId].WayCostFromHome > visit.StartTime - r.Waypoints.First().StartTime;
                }).ToList();
                if (wrongStartWays.Count > 0)
                {
                    return $"Way between home and visit {wrongStartWays.First().Waypoints.ElementAt(1).VisitId} is too short.";
                }
            }

            // check way from last visit to home
            {
                var wrongEndWays = routes.Where(r =>
                {
                    var lastVisit = r.Waypoints.ElementAt(r.Waypoints.Length - 2);
                    return OptimizationInput.Visits[lastVisit.VisitId].WayCostToHome > r.Waypoints.Last().StartTime - lastVisit.StartTime;
                }).ToList();
                if (wrongEndWays.Count > 0)
                {
                    return $"Way between visit {wrongEndWays.First().Waypoints.Reverse().Skip(1).First().VisitId} and home is too short.";
                }
            }

            // check ways in between
            {
                foreach (var route in routes)
                {
                    var middleWaypoints = route.Waypoints.Take(route.Waypoints.Length - 1).Skip(1);
                    var previous = middleWaypoints.First();
                    foreach (var current in middleWaypoints.Skip(1))
                    {
                        if (OptimizationInput.RouteCosts[previous.VisitId, current.VisitId] > current.StartTime - previous.StartTime)
                        {
                            return $"Way between visit {previous.VisitId} and visit {current.VisitId} is too short.";
                        }
                        previous = current;
                    }
                }
            }

            // check multiple visits
            {
                var diffChecker = new HashSet<int>();
                var multipleVisitedIds = routes.SelectMany(r => r.Waypoints).Select(wp => wp.VisitId).Where(id => id != Constants.VisitIdHome && !diffChecker.Add(id)).ToList();
                if (multipleVisitedIds.Count > 0)
                {
                    return $"Visit {multipleVisitedIds.First()} is visited more than once.";
                }
            }

            // wrong breaks
            {
                var wrongBreaks = routes
                    .SelectMany(r => r.Waypoints
                        .Where(wp => wp.VisitId != Constants.VisitIdHome)
                        .Where(wp =>
                        {
                            var visit = OptimizationInput.Visits[wp.VisitId];
                            return visit.IsBreak && visit.SantaId != r.SantaId;
                        })).ToList();
                if (wrongBreaks.Count > 0)
                {
                    return $"Break with id={wrongBreaks.First().VisitId} is made by the wrong santa.";
                }
            }

            // valid
            return null;
        }
    }
}
