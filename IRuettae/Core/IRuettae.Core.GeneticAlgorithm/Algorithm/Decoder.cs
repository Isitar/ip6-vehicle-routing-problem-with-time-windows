using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class Decoder
    {
        private readonly OptimizationInput input;
        private readonly Dictionary<int, int> alleleToVisitIdMapping;
        private List<int> santaIds = new List<int>();

        public Decoder(OptimizationInput input, Dictionary<int, int> alleleToVisitIdMap)
        {
            this.input = input;
            this.alleleToVisitIdMapping = alleleToVisitIdMap;
        }

        public Route[] Decode(List<int> genotype)
        {
            // preparation
            var routesPerDay = CountRoutes(genotype) / input.Days.Length;
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

        private void GenerateSantaIdList(int routesPerDay)
        {
            santaIds.Clear();

            for (int i = 0; i < routesPerDay; i++)
            {
                if (i < input.Santas.Length)
                {
                    // real santa
                    santaIds.Add(input.Santas[i].Id);
                }
                else
                {
                    // new, artificial santa
                    santaIds.Add(santaIds.LastOrDefault() + 1);
                }
            }
        }

        private Route GetRoute(List<int> genotype, ref int currentGenPos, int startTimeOfDay)
        {
            var waypoints = new List<Waypoint>();
            // add start at home
            waypoints.Add(new Waypoint()
            {
                StartTime = startTimeOfDay,
                VisitId = Constants.VisitIdHome,
            });
            while (currentGenPos < genotype.Count && !PopulationGenerator.IsSeparator(genotype[currentGenPos]))
            {
                int visitId = GetVisitId(genotype[currentGenPos]);
                var previousWaypoint = waypoints.Last();
                int startTime = previousWaypoint.StartTime;
                if (waypoints.Count == 1)
                {
                    // first real visit
                    startTime += input.Visits.Where(v => v.Id == visitId).First().WayCostFromHome;
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

        private int CountRoutes(List<int> genotype)
        {
            return genotype.Where(PopulationGenerator.IsSeparator).Count() + 1;
        }

        public int GetVisitId(int allele)
        {
            return alleleToVisitIdMapping[allele];
        }
    }
}
