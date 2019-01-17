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

        public void Create(int maxNumberOfSantas)
        {
            data.SantaIds.Clear();

            for (int i = 0; i < maxNumberOfSantas; i++)
            {
                if (i < data.Input.Santas.Length)
                {
                    // real santa
                    data.SantaIds.Add(data.Input.Santas[i].Id);
                }
                else
                {
                    // new, artificial santa
                    data.SantaIds.Add(data.SantaIds.Max() + 1);
                }
            }
        }
    }
}
