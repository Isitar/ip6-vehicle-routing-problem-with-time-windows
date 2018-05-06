using System;
using System.Diagnostics;
using System.Linq;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
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
            var route = new Route(solverData.NumberOfSantas, solverData.NumberOfDays);

            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                {
                    Waypoint? nextLocation = new Waypoint(0, -1);
                    nextLocation = GetNextLocation(day, santa, nextLocation.Value);
                    if (!nextLocation.HasValue)
                    {
                        // no visits that evening
                        continue;
                    }

                    // create way from home
                    {
                        var distance = solverData.Input.Distances[solverData.StartEndPoint, nextLocation.Value.visit];
                        nextLocation = new Waypoint(0, nextLocation.Value.startTime - distance - 1);
                    }

                    do
                    {
                        route.Waypoints[santa, day].Add(nextLocation.Value);
                        nextLocation = GetNextLocation(day, santa, nextLocation.Value);
                    } while (nextLocation.HasValue);

                    // append way back home
                    {
                        var last = route.Waypoints[santa, day].LastOrDefault();
                        var duration = solverData.Input.VisitsDuration[last.visit];
                        var distance = solverData.Input.Distances[last.visit, solverData.StartEndPoint];
                        route.Waypoints[santa, day].Add(new Waypoint(0, last.startTime + duration + distance));
                    }
                }
            }

            Debug.Write("Result is:");
            Debug.Write(route.ToString());

            return route;
        }

        private Waypoint? GetNextLocation(int day, int santa, Waypoint value)
        {
            for (int timeslice = value.startTime + 1; timeslice < solverData.SlicesPerDay[day]; timeslice++)
            {
                for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    if (visit == value.visit)
                    {
                        continue;
                    }

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