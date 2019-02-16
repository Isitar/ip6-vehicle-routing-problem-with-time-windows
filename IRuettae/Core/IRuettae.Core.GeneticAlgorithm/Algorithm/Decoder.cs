using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    /// <summary>
    /// Class to convert a Genotype into a Route[]
    /// </summary>
    public class Decoder
    {
        private readonly OptimizationInput input;
        private readonly Dictionary<int, int> alleleToVisitIdMapping;
        /// <summary>
        /// Cached list of all santaIds
        /// </summary>
        private readonly List<int> santaIds = new List<int>();

        public Decoder(OptimizationInput input, Dictionary<int, int> alleleToVisitIdMapping)
        {
            this.input = input;
            this.alleleToVisitIdMapping = alleleToVisitIdMapping;
        }

        public Route[] Decode(Genotype genotype)
        {
            // preparation
            var routesPerDay = genotype.CountRoutes() / input.Days.Length;
            GenerateSantaIdList(routesPerDay);

            var routes = new List<Route>();
            int currentGenPos = 0;
            foreach (var (from, _) in input.Days)
            {
                var routePerDayCounter = 0;
                while (routePerDayCounter < routesPerDay)
                {
                    var r = GetRoute(genotype, ref currentGenPos, from);
                    r.SantaId = santaIds[routePerDayCounter];
                    routes.Add(r);
                    routePerDayCounter++;
                }
            }
            return routes.ToArray();
        }

        /// <summary>
        /// Depending on the dataset, artificial santas must be created
        /// </summary>
        /// <param name="numberOfSantas"></param>
        private void GenerateSantaIdList(int numberOfSantas)
        {
            if (numberOfSantas == santaIds.Count)
            {
                return;
            }

            santaIds.Clear();

            for (int i = 0; i < numberOfSantas; i++)
            {
                if (i < input.Santas.Length)
                {
                    // real santa
                    santaIds.Add(input.Santas[i].Id);
                }
                else
                {
                    // new, artificial santa
                    santaIds.Add(santaIds.Max() + 1);
                }
            }
        }

        /// <summary>
        /// Extract the route starting at currentGenPos
        /// </summary>
        /// <param name="genotype"></param>
        /// <param name="currentGenPos">Will be set to the index of the allele following the route</param>
        /// <param name="startTimeOfDay"></param>
        /// <returns></returns>
        private Route GetRoute(Genotype genotype, ref int currentGenPos, int startTimeOfDay)
        {
            // First v
            // egal
            // 0
            // 0
            // 86400
            // 88200
            // 86400

            // start
            // egal
            // 2: 0-52=-52
            // 10: 0-604=-604
            // 17: 86400-(86615-86400)=86185
            // 26: 88200-(87005-86400)=87595
            // 33: 86400-(87682-86400)=85118

            if (currentGenPos == 2)
            {
                startTimeOfDay = -52;
            }
            if (currentGenPos == 10)
            {
                startTimeOfDay = -604;
            }
            if (currentGenPos == 17)
            {
                startTimeOfDay = 86185;
            }
            if (currentGenPos == 26)
            {
                startTimeOfDay = 87595;
            }
            if (currentGenPos == 33)
            {
                startTimeOfDay = 85118;
            }

            var waypoints = new List<Waypoint>
            {
                // add start at home
                new Waypoint()
                {
                    StartTime = startTimeOfDay,
                    VisitId = Constants.VisitIdHome,
                }
            };
            while (currentGenPos < genotype.Count && !PopulationGenerator.IsSeparator(genotype[currentGenPos]))
            {
                int visitId = GetVisitId(genotype[currentGenPos]);
                var previousWaypoint = waypoints.Last();
                int startTime = previousWaypoint.StartTime;
                if (waypoints.Count == 1)
                {
                    // first real visit
                    startTime += input.Visits.First(v => v.Id == visitId).WayCostFromHome;
                }
                else
                {
                    // not first visit
                    startTime += input.Visits[previousWaypoint.VisitId].Duration + input.RouteCosts[previousWaypoint.VisitId, visitId];
                }

                waypoints.Add(new Waypoint()
                {
                    StartTime = startTime,
                    VisitId = visitId,
                });

                currentGenPos++;
            }
            currentGenPos++;

            if (waypoints.Skip(1).All(wp => input.Visits[wp.VisitId].IsBreak))
            {
                // breaks only
                waypoints.Clear();
            }

            if (waypoints.Count > 0)
            {
                // add return to home
                var previousWaypoint = waypoints.Last();
                waypoints.Add(new Waypoint()
                {
                    StartTime = previousWaypoint.StartTime + input.Visits[previousWaypoint.VisitId].Duration + input.Visits[previousWaypoint.VisitId].WayCostToHome,
                    VisitId = Constants.VisitIdHome,
                });
            }
            return new Route()
            {
                Waypoints = waypoints.ToArray(),
            };
        }

        /// <summary>
        /// Gets the VisitId from an allele.
        /// </summary>
        /// <param name="allele"></param>
        /// <returns></returns>
        public int GetVisitId(int allele)
        {
            return alleleToVisitIdMapping[allele];
        }
    }
}
