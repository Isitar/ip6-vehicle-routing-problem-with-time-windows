using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IRuettae.Converter
{
    public class PersistenceToCoreConverter
    {
        /// <summary>
        /// Converts the input params to an OptimisationInput
        /// </summary>
        /// <param name="workingDays">All working days for the santas</param>
        /// <param name="startVisit">Where all routes have to start</param>
        /// <param name="visits">All visits for the problem</param>
        /// <param name="santas">All santas for the problem</param>
        /// <param name="breaks">All breaks for the santas</param>
        /// <returns>An optimisation input that can be used to solve the problem</returns>
        public Core.Models.OptimisationInput Convert(List<(DateTime, DateTime)> workingDays, Persistence.Entities.Visit startVisit, List<Persistence.Entities.Visit> visits, List<Persistence.Entities.Santa> santas, List<Persistence.Entities.Visit> breaks)
        {
            var input = new Core.Models.OptimisationInput
            {
                WayMatrix = new Core.Models.WayMatrix
                {
                    RouteCosts = new int[visits.Count, visits.Count]
                },
                Visits = new Core.Models.Visit[visits.Count],
                Santas = new Core.Models.Santa[santas.Count * workingDays.Count],
            };

            // set 0-time
            var zeroTime = workingDays.Min(wd => wd.Item1);

            // create visits

            // sort visits to be sure they are in id order
            visits = visits.OrderBy(v => v.Id).ToList();

            for (int x = 0; x < visits.Count; x++)
            {
                var persistenceVisit = visits[x];
                input.Visits[x] = new Core.Models.Visit()
                {
                    Id = x,
                    Desired = persistenceVisit.Desired
                        .Select(d => ((d.Start.Value - zeroTime).Seconds, (d.End.Value - zeroTime).Seconds)).ToArray(),
                    Unavailable = persistenceVisit.Unavailable
                        .Select(d => ((d.Start.Value - zeroTime).Seconds, (d.End.Value - zeroTime).Seconds)).ToArray(),
                    Duration = persistenceVisit.NumberOfChildren * 5 + 15,
                    WayCostFromHome = startVisit.ToWays.First(w => w.To.Id.Equals(persistenceVisit.Id)).Duration,
                    WayCostToHome = startVisit.FromWays.First(w => w.To.Id.Equals(persistenceVisit.Id)).Duration,
                };

                // fill distance matrix
                for (int y = 0; y < visits.Count; y++)
                {
                    if (x == y)
                    {
                        input.WayMatrix[x, y] = 0;
                    }
                    else
                    {
                        input.WayMatrix[x, y] = persistenceVisit.FromWays.First(w => w.To.Id.Equals(visits[y].Id)).Duration;
                    }
                }
            }

            for (int i = 0; i < santas.Count * workingDays.Count; i++)
            {
                input.Santas[i] = new Core.Models.Santa { Id = i };
            }

            // todo: work from here ?break handling?

            return input;
        }
    }
}
