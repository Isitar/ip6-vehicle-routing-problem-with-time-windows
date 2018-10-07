using System;
using System.Collections.Generic;
using System.Linq;

namespace IRuettae.Core.ILP.Algorithm.Clustering.Detail
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
            var realNumberOfSantas = solverData.SolverInputData.Santas.GetLength(1);
            var realNumberOfDays = solverData.SolverInputData.Santas.GetLength(0);

            var route = new Route(realNumberOfSantas, realNumberOfDays);
            foreach (var santa in Enumerable.Range(0, realNumberOfSantas))
            {
                foreach (var day in Enumerable.Range(0, realNumberOfDays))
                {
                    var waypoints = new List<Waypoint>();

                    foreach (var visit in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        if (Math.Abs(solverData.Variables.SantaVisit[day * realNumberOfSantas + santa, visit].SolutionValue() - 1) < 0.0001)
                        {
                            waypoints.Add(new Waypoint(visit, 0, solverData.SolverInputData.VisitIds[visit]));
                        }
                    }

                    route.Waypoints[santa, day] = waypoints;
                }
            }
            return route;
        }
    }
}