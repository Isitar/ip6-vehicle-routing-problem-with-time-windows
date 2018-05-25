using System;
using System.Collections.Generic;
using System.Linq;

namespace IRuettae.Core.Algorithm.Clustering.Detail
{
    internal class ResultBuilder
    {
        private readonly SolverData solverData;

        public ResultBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public Route CreateResult()
        {

            var route = new Route(solverData.NumberOfSantas, 1);
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                route.Waypoints[santa, 0] = new List<Waypoint>();
                foreach (var visit in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    if (Math.Abs(solverData.Variables.SantaVisit[santa, visit].SolutionValue() - 1) < 0.0001)
                    {
                        route.Waypoints[santa, 0].Add(new Waypoint(visit,0));
                    }
                }
            }

            return route;
        }
    }
}