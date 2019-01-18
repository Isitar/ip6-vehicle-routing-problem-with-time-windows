using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class UnavailableCreator
    {
        private readonly RoutingData data;

        public UnavailableCreator(RoutingData data)
        {
            this.data = data;
        }

        /// <summary>
        /// requires data.Visits
        /// creates data.Unavailable
        /// </summary>
        public void Create()
        {
            data.Unavailable.Clear();

            foreach (var visit in data.Visits)
            {
                var duration = visit.Duration;
                var unavailable = new List<(int, int)>();
                foreach (var (from, to) in visit.Unavailable)
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
                data.Unavailable.Add(unavailable);
            }
        }
    }
}
