using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class VisitCreator
    {
        private readonly RoutingData data;

        public VisitCreator(RoutingData data)
        {
            this.data = data;
        }

        /// <summary>
        /// requires data.SantaIds
        /// creates data.Visits
        /// creates data.HomeIndex
        /// creates data.HomeIndexAdditional
        /// </summary>
        public void Create()
        {
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
                        breakVisit.Desired = breakVisit.Desired.Where(d => Utility.IntersectionLength(d.from, d.to, from, to) > 0).ToArray();

                        // set unavailable on other days
                        breakVisit.Unavailable = new(int, int)[]
                        {
                                (int.MinValue, from - 1),
                                (to + 1, int.MaxValue),
                        };

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
                    clone.Desired = clone.Desired ?? new(int, int)[0];
                    clone.Unavailable = clone.Unavailable ?? new(int, int)[0];
                    visits.Add(clone);
                }
            }

            // add home
            {
                var home = new Visit()
                {
                    Id = Constants.VisitIdHome,
                    IsBreak = false,
                    SantaId = Constants.InvalidSantaId,
                    Duration = 0,
                    Desired = new(int, int)[0],
                    Unavailable = new(int, int)[0],
                };
                visits.Add(home);
                data.HomeIndex = visits.Count - 1;

                // home for aditional santas
                visits.Add(home);
                data.HomeIndexAdditional = visits.Count - 1;
            }

            data.Visits = visits.ToArray();
        }
    }
}
