using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class SantaCreator
    {
        private readonly RoutingData data;

        public SantaCreator(RoutingData data)
        {
            this.data = data;
        }

        /// <summary>
        /// creates data.SantaIds
        /// </summary>
        /// <param name="maxNumberOfSantas"></param>
        public void Create(int maxNumberOfSantas)
        {
            data.SantaIds.Clear();

            List<int> santaIds = new List<int>();
            for (int i = 0; i < maxNumberOfSantas; i++)
            {
                if (i < data.Input.Santas.Length)
                {
                    // real santa
                    santaIds.Add(data.Input.Santas[i].Id);
                }
                else
                {
                    // new, artificial santa
                    santaIds.Add(santaIds.Max() + 1);
                }
            }

            // duplicate for each day
            for (int i = 0; i < data.Input.Days.Length; i++)
            {
                data.SantaIds.AddRange(santaIds);
            }
        }
    }
}
