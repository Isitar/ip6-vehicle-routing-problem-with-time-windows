using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Persistence.Entities;


namespace IRuettae.Converter
{
    public class PersistenceCoreConverter
    {
        /// <summary>
        /// Mapping for backward conversion
        /// Visit-Number to Visit.Id
        /// </summary>
        private Dictionary<int, long> visitMap = new Dictionary<int, long>();

        /// <summary>
        /// Mapping for backward conversion
        /// Santa-Number to Santa.Id
        /// </summary>
        private Dictionary<int, long> santaMap = new Dictionary<int, long>();

        /// <summary>
        /// Converts the input params to an OptimizationInput
        /// </summary>
        /// <param name="workingDays">All working days for the santas</param>
        /// <param name="startVisit">Where all routes have to start</param>
        /// <param name="visits">All visits for the problem</param>
        /// <param name="santas">All santas for the problem</param>
        /// <returns>An optimization input that can be used to solve the problem</returns>
        public Core.Models.OptimizationInput Convert(List<(DateTime Start, DateTime End)> workingDays, Persistence.Entities.Visit startVisit, List<Persistence.Entities.Visit> visits, List<Persistence.Entities.Santa> santas)
        {
            if (visitMap.Count != 0 || visitMap.Count != 0)
            {
                throw new InvalidOperationException("each instance of " + this.GetType().FullName + "can only be used once");
            }

            var recidToSantaMap = new Dictionary<long, int>();

            var input = new Core.Models.OptimizationInput
            {
                Visits = new Core.Models.Visit[visits.Count],
                Santas = new Core.Models.Santa[santas.Count],
                Days = new(int from, int to)[workingDays.Count],
                RouteCosts = new int[visits.Count, visits.Count],
            };

            // set 0-time
            var zeroTime = workingDays.Min(wd => wd.Start);

            // create santas
            santas = santas.OrderBy(s => s.Id).ToList();
            for (int i = 0; i < santas.Count; i++)
            {
                var persistenceSanta = santas[i];
                recidToSantaMap.Add(persistenceSanta.Id, i);
                santaMap.Add(i, persistenceSanta.Id);
                input.Santas[i] = new Core.Models.Santa { Id = i };
            }

            // create visits
            visits = visits.OrderBy(v => v.Id).ToList();
            for (int x = 0; x < visits.Count; x++)
            {
                var persistenceVisit = visits[x];
                visitMap.Add(x, persistenceVisit.Id);

                var isBreak = persistenceVisit.VisitType == VisitType.Break;

                input.Visits[x] = new Core.Models.Visit
                {
                    Id = x,
                    Desired = persistenceVisit.Desired
                        .Select(d => ((int)(d.Start.Value - zeroTime).TotalSeconds, (int)(d.End.Value - zeroTime).TotalSeconds)).ToArray(),
                    Unavailable = persistenceVisit.Unavailable
                        .Select(d => ((int)(d.Start.Value - zeroTime).TotalSeconds, (int)(d.End.Value - zeroTime).TotalSeconds)).ToArray(),
                    Duration = (int)persistenceVisit.Duration,
                    WayCostFromHome = startVisit.ToWays.First(w => w.To.Id.Equals(persistenceVisit.Id)).Duration,
                    WayCostToHome = startVisit.FromWays.First(w => w.From.Id.Equals(persistenceVisit.Id)).Duration,
                    IsBreak = isBreak,
                    SantaId = isBreak ? recidToSantaMap[persistenceVisit.Santa.Id] : -1,
                };

                // fill distance matrix
                for (int y = 0; y < visits.Count; y++)
                {
                    if (x == y)
                    {
                        input.RouteCosts[x, y] = 0;
                    }
                    else
                    {
                        input.RouteCosts[x, y] = persistenceVisit.ToWays.First(w => w.To.Id.Equals(visits[y].Id)).Duration;
                    }
                }
            }

            for (int i = 0; i < workingDays.Count; i++)
            {
                input.Days[i] = ((int)(workingDays[i].Start - zeroTime).TotalSeconds, (int)(workingDays[i].End - zeroTime).TotalSeconds);
            }
            return input;
        }
    }
}
