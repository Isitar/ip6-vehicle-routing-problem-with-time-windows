using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public static class Converter
    {
        public static RoutingData Convert(OptimizationInput input, int maxNumberOfAdditionalSantas)
        {
            var data = new RoutingData(input);

            CreateSantas(data, maxNumberOfAdditionalSantas);
            CreateVisits(data);
            CreateUnavailable(data);
            CreateDesired(data);
            CreateStartEnd(data);

            return data;
        }

        /// <summary>
        /// creates data.SantaIds
        /// </summary>
        /// <param name="data">routing input data</param>
        /// <param name="maxNumberOfAdditionalSantas"></param>
        private static void CreateSantas(RoutingData data, int maxNumberOfAdditionalSantas)
        {
            var santaIds = new List<int>();
            for (int i = 0; i < data.Input.Santas.Length; i++)
            {
                // real santa
                santaIds.Add(data.Input.Santas[i].Id);
            }
            for (int i = 0; i < maxNumberOfAdditionalSantas; i++)
            {
                // new, artificial santa
                santaIds.Add(santaIds.Max() + 1);
            }

            // duplicate for each day
            var santaIdsPerDay = new List<int>();
            foreach (var _ in data.Input.Days)
            {
                santaIdsPerDay.AddRange(santaIds);
            }

            data.SantaIds = santaIdsPerDay.ToArray();
        }

        /// <summary>
        /// requires data.SantaIds
        /// creates data.Visits
        /// creates data.HomeIndex
        /// </summary>
        private static void CreateVisits(RoutingData data)
        {
            if (data.SantaIds == null)
            {
                throw new ArgumentNullException();
            }

            var visits = new List<Visit>();
            foreach (var visit in data.Input.Visits)
            {
                if (data.SantaIds.Contains(visit.SantaId) && visit.IsBreak)
                {
                    // break
                    var santaIndex = -1;
                    foreach (var (from, to) in data.Input.Days)
                    {
                        // create a break for each day
                        var breakVisit = (Visit)visit.Clone();

                        // remove desired on other days
                        breakVisit.Desired = breakVisit.Desired?.Where(d => Core.Utility.IntersectionLength(d.from, d.to, from, to) > 0).ToArray() ?? new (int, int)[0];

                        // not needed because of the new santaId
                        // which assigns this break to a specific day
                        breakVisit.Unavailable = new (int, int)[0];

                        // set santa index
                        santaIndex++;
                        santaIndex = Array.IndexOf(data.SantaIds, breakVisit.SantaId, santaIndex);
                        breakVisit.SantaId = santaIndex;

                        visits.Add(breakVisit);
                    }
                }
                else
                {
                    // normal visit
                    var clone = (Visit)visit.Clone();
                    clone.Desired = clone.Desired ?? new (int, int)[0];
                    clone.Unavailable = clone.Unavailable ?? new (int, int)[0];
                    visits.Add(clone);
                }
            }

            // add home for each day
            {
                var home = new Visit
                {
                    Id = Constants.VisitIdHome,
                    IsBreak = false,
                    SantaId = Constants.InvalidSantaId,
                    Duration = 0,
                    Desired = new (int, int)[0],
                    Unavailable = new (int, int)[0],
                };

                var numberOfDays = data.Input.Days.Length;
                data.HomeIndex = new int[numberOfDays];
                for (int i = 0; i < numberOfDays; i++)
                {
                    visits.Add(home);
                    data.HomeIndex[i] = visits.Count - 1;
                }
            }

            data.Visits = visits.ToArray();
        }


        /// <summary>
        /// requires data.Visits
        /// creates data.Unavailable
        /// </summary>
        private static void CreateUnavailable(RoutingData data)
        {
            if (data.Visits == null)
            {
                throw new ArgumentNullException();
            }


            var unavailables = new List<(int, int)[]>();
            foreach (var visit in data.Visits)
            {
                var duration = visit.Duration;
                var unavailable = new List<(int, int)>();
                foreach (var (from, to) in visit.Unavailable.Where(u => u.from < u.to))
                {
                    if (from == int.MinValue)
                    {
                        // avoid underflow problems
                        unavailable.Add((from, to));
                    }
                    else
                    {
                        unavailable.Add((from - duration, to));
                    }
                }
                unavailables.Add(unavailable.ToArray());
            }

            data.Unavailable = unavailables.ToArray();
        }

        /// <summary>
        /// Important: Desired which have a smaller duration
        /// than the maximal duration of a desired are dropped.
        /// requires data.Visits
        /// creates data.BestDesired
        /// </summary>
        private static void CreateDesired(RoutingData data)
        {
            if (data.Visits == null)
            {
                throw new ArgumentNullException();
            }

            var desireds = new List<(int, int)[]>();
            foreach (var visit in data.Visits)
            {
                var duration = visit.Duration;
                var desired = new List<(int, int)>();

                int Length((int, int) d) => Utility.GetRealDesiredLength(data, d);
                var maxLength = visit.Desired.Select(Length).Append(1).Max();

                // filter desired which are shorter than the maximum or business hours
                foreach (var (from, to) in visit.Desired.Where(d => Length(d) == maxLength))
                {
                    if (to - from >= duration)
                    {
                        // entire duration can be in desired
                        desired.Add((from, to - duration));
                    }
                    else
                    {
                        // only a part of the visit can be in desired
                        desired.Add((to - duration, from));
                    }
                }
                desireds.Add(desired.ToArray());
            }

            data.BestDesired = desireds.ToArray();
        }

        /// <summary>
        /// requires data.SantaIds
        /// requires data.Visits
        /// requires data.HomeIndex
        /// creates data.SantaStartIndex
        /// creates data.SantaEndIndex
        /// </summary>
        private static void CreateStartEnd(RoutingData data)
        {
            if (data.SantaIds == null || data.Visits == null)
            {
                throw new ArgumentNullException();
            }

            var numberOfSantas = data.SantaIds.Length;
            var starts = new int[numberOfSantas];
            var ends = new int[numberOfSantas];
            for (int i = 0; i < numberOfSantas; i++)
            {
                var day = data.GetDayFromSanta(i);
                starts[i] = data.HomeIndex[day];
                ends[i] = data.HomeIndex[day];
            }

            data.SantaStartIndex = starts;
            data.SantaEndIndex = ends;
        }
    }
}
