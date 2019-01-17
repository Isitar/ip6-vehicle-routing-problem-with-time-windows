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
        /// creates data.Visits
        /// requires data.SantaIds
        /// </summary>
        public void Create()
        {
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
                        santaIndex = data.SantaIds.IndexOf(breakVisit.SantaId, santaIndex);
                        breakVisit.SantaId = santaIndex;

                        data.Visits.Add(breakVisit);
                    }
                }
                else
                {
                    // normal visit
                    data.Visits.Add((Visit)visit.Clone());
                }
            }
        }
    }
}
