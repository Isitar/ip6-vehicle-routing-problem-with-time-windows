using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering.Detail
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
                    var numberOfVisits = 0;
                    var santaIndex = day * realNumberOfSantas + santa;


                    foreach (var visit in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        if (Math.Abs(solverData.Variables.SantaVisit[santaIndex][visit].X - 1) < 0.0001)
                        {
                            numberOfVisits++;
                        }
                    }

                    var waypoints = new List<Waypoint> { new Waypoint(0, 0) };
                    var uesWay = solverData.Variables.SantaUsesWay[santaIndex];
                    for (int i = 1; i < numberOfVisits; i++)
                    {
                        waypoints.Add(new Waypoint(NextWaypoint(uesWay, waypoints.Last().Visit), i));
                    }

                    route.Waypoints[santa, day] = waypoints;
                }
            }
            return route;
        }

        private int NextWaypoint(GRBVar[] santaUsesWay, int fromVisit)
        {
            for (int i = 0; i < santaUsesWay.GetLength(1); i++)
            {
                if (Math.Abs(santaUsesWay[solverData.SourceDestArrPos(fromVisit,i)].X) > 0.0001)
                {
                    return i;
                }
            }

            return 0;

        }
    }
}