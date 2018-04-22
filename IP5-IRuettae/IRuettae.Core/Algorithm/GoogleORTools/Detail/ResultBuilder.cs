using System;
using System.Diagnostics;
using System.Linq;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class ResultBuilder
    {
        private readonly SolverData solverData;

        public ResultBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public Route CreateResult()
        {
            Debug.WriteLine($"{solverData.Solver.Objective().Value()} is the value of the target function.");

            var route = new Route(solverData.NumberOfSantas, solverData.NumberOfDays);
            {
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                    {
                        Waypoint? nextLocation = new Waypoint(0, 0);
                        do
                        {
                            route.Waypoints[santa, day].Add(nextLocation.Value);
                            nextLocation = GetNextLocation(day, santa, nextLocation.Value);
                        } while (nextLocation.HasValue);

                        // append way back home
                        var last = route.Waypoints[santa, day].LastOrDefault();
                        var addTime = solverData.Input.VisitsDuration[last.visit] + solverData.Input.Distances[last.visit, solverData.StartEndPoint];
                        route.Waypoints[santa, day].Add(new Waypoint(0, last.startTime + addTime));
                    }
                }
            }

            Debug.WriteLine("Result is:");
            Debug.WriteLine(route.ToString());

            return route;
        }

        private Waypoint? GetNextLocation(int day, int santa, Waypoint value)
        {
            for (int timeslice = value.startTime + 1; timeslice < solverData.SlicesPerDay[day]; timeslice++)
            {
                for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    if (solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice].SolutionValue() == 1)
                    {
                        return new Waypoint(visit, timeslice);
                    }
                }
            }
            return null;
        }
    }
}