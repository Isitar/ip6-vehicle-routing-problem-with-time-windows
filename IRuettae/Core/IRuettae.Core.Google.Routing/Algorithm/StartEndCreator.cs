using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class StartEndCreator
    {
        private readonly RoutingData data;

        public StartEndCreator(RoutingData data)
        {
            this.data = data;
        }

        /// <summary>
        /// requires data.SantaIds
        /// requires data.Visits
        /// requires data.HomeIndex
        /// requires data.HomeIndexAdditional
        /// creates data.Start
        /// creates data.End
        /// </summary>
        public void Create()
        {
            var numberOfSantas = data.SantaIds.Length;
            var starts = new int[numberOfSantas];
            var ends = new int[numberOfSantas];
            for (int i = 0; i < numberOfSantas; i++)
            {
                var startIndex = data.HomeIndex;
                var endIndex = data.HomeIndex;
                if (IsAdditionalSanta(data.SantaIds[i]))
                {
                    startIndex = data.HomeIndexAdditional;
                }
                starts[i] = startIndex;
                ends[i] = endIndex;
            }

            data.SantaStartIndex = starts;
            data.SantaEndIndex = ends;
        }

        private bool IsAdditionalSanta(int santaId)
        {
            return data.Input.Santas.All(s => s.Id != santaId);
        }
    }
}
