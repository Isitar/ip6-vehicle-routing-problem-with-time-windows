using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Persistence.Entities;


namespace IRuettae.Converter
{
    public class PersistenceToCoreConverter
    {
        /// <summary>
        /// Mapping for backward conversion
        /// Visit-Number to Visit.Id
        /// </summary>
        public Dictionary<int, long> VisitMap { get; } = new Dictionary<int, long>();

        /// <summary>
        /// Mapping for backward conversion
        /// Santa-Number to Santa.Id
        /// </summary>
        public Dictionary<int, long> SantaMap { get; } = new Dictionary<int, long>();

        /// <summary>
        /// Starting time of the first day
        /// Used to retreive the real time from a relative time in seconds
        /// </summary>
        public DateTime ZeroTime { get; private set; } = new DateTime();

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
            VisitMap.Clear();
            SantaMap.Clear();

            var recidToSantaMap = new Dictionary<long, int>();

            var input = new Core.Models.OptimizationInput
            {
                Visits = new Core.Models.Visit[visits.Count],
                Santas = new Core.Models.Santa[santas.Count],
                Days = new(int from, int to)[workingDays.Count],
                RouteCosts = new int[visits.Count, visits.Count],
            };

            // set 0-time
            ZeroTime = workingDays.Min(wd => wd.Start);

            // create santas
            santas = santas.OrderBy(s => s.Id).ToList();
            for (int i = 0; i < santas.Count; i++)
            {
                var persistenceSanta = santas[i];
                recidToSantaMap.Add(persistenceSanta.Id, i);
                SantaMap.Add(i, persistenceSanta.Id);
                input.Santas[i] = new Core.Models.Santa { Id = i };
            }

            // create visits
            visits = visits.OrderBy(v => v.Id).ToList();
            for (int x = 0; x < visits.Count; x++)
            {
                var persistenceVisit = visits[x];
                VisitMap.Add(x, persistenceVisit.Id);

                var isBreak = persistenceVisit.VisitType == VisitType.Break;

                input.Visits[x] = new Core.Models.Visit
                {
                    Id = x,
                    Desired = persistenceVisit.Desired
                        .Select(d => ((int)(d.Start.Value - ZeroTime).TotalSeconds, (int)(d.End.Value - ZeroTime).TotalSeconds)).ToArray(),
                    Unavailable = persistenceVisit.Unavailable
                        .Select(d => ((int)(d.Start.Value - ZeroTime).TotalSeconds, (int)(d.End.Value - ZeroTime).TotalSeconds)).ToArray(),
                    Duration = (int)persistenceVisit.Duration,
                    WayCostFromHome = startVisit.FromWays.First(w => w.To.Id.Equals(persistenceVisit.Id)).Duration,
                    WayCostToHome = startVisit.ToWays.First(w => w.From.Id.Equals(persistenceVisit.Id)).Duration,
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
                        input.RouteCosts[x, y] = persistenceVisit.FromWays.First(w => w.To.Id.Equals(visits[y].Id)).Duration;
                    }
                }
            }

            for (int i = 0; i < workingDays.Count; i++)
            {
                input.Days[i] = ((int)(workingDays[i].Start - ZeroTime).TotalSeconds, (int)(workingDays[i].End - ZeroTime).TotalSeconds);
            }

            // Add unavailable outside of working hours
            {
                var orderedDays = input.Days.OrderBy(d => d.from).ToList();
                var unavailabilities = new List<(int from, int to)>();
                (int from, int to) lastDay = orderedDays.First();

                // before first day
                unavailabilities.Add((int.MinValue, lastDay.from));

                // between days
                foreach (var day in orderedDays.Skip(1))
                {
                    unavailabilities.Add((lastDay.to, day.from));
                    lastDay = day;
                }

                // after last day
                unavailabilities.Add((lastDay.to, int.MaxValue));

                // add to visits
                for (int i = 0; i < input.Visits.Count(); i++)
                {
                    var newUnavailable = new List<(int from, int to)>(input.Visits[i].Unavailable);
                    newUnavailable.AddRange(unavailabilities);
                    input.Visits[i].Unavailable = newUnavailable.ToArray();
                }
            }
            return input;
        }
    }
}
