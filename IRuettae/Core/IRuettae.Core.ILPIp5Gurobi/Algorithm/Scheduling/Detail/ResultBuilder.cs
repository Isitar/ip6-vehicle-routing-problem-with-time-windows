using System;
using System.Diagnostics;
using System.Linq;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling.Detail
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
            var route = new Route(solverData.NumberOfSantas, solverData.NumberOfDays)
            {
                SantaIds = solverData.Input.SantaIds
            };

            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                {
                    Waypoint? nextLocation = new Waypoint(0, -1);
                    nextLocation = GetNextLocation(day, santa, nextLocation.Value);
                    if (!nextLocation.HasValue || nextLocation.Value.StartTime < 0)
                    {
                        // no visits that evening
                        continue;
                    }

                    // create way from home
                    {
                        var distance = solverData.Input.Distances[solverData.StartEndPoint, nextLocation.Value.Visit];
                        nextLocation = new Waypoint(0, (nextLocation.Value.StartTime - distance - 1));
                    }

                    do
                    {
                        route.Waypoints[santa, day].Add(nextLocation.Value);
                        nextLocation = GetNextLocation(day, santa, nextLocation.Value);
                    } while (nextLocation.HasValue);

                    // append way back home
                    {
                        var last = route.Waypoints[santa, day].LastOrDefault();
                        var duration = solverData.Input.VisitsDuration[last.Visit];
                        var distance = solverData.Input.Distances[last.Visit, solverData.StartEndPoint];
                        route.Waypoints[santa, day].Add(new Waypoint(0, last.StartTime + duration + distance));
                    }
                    
                }
            }

            Debug.Write("Result is:");
            Debug.Write(route.ToString());

            return route;
        }

        private Waypoint? GetNextLocation(int day, int santa, Waypoint waypoint)
        {
            int nextPossibleStart = waypoint.StartTime + Math.Max(1, solverData.Input.VisitsDuration[waypoint.Visit]);
            for (int timeslice = nextPossibleStart; timeslice < solverData.SlicesPerDay[day]; timeslice++)
            {
                for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    if (visit == waypoint.Visit && nextPossibleStart != 0)
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