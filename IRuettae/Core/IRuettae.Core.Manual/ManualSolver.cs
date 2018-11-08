using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.Manual
{
    /// <summary>
    /// Class to generate a solution by hand
    /// </summary>
    public class ManualSolver : ISolver
    {
        private OptimizationInput input;
        private ManualStarterData starterData;

        public ManualSolver(OptimizationInput input, ManualStarterData starterData)
        {
            this.input = input;
            this.starterData = starterData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timelimit">unused</param>
        /// <param name="progress"></param>
        /// <param name="consoleProgress">unused</param>
        /// <returns></returns>
        public OptimizationResult Solve(int timelimit, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            var sw = Stopwatch.StartNew();

            List<Route> routes = CreateRoutes();

            sw.Stop();
            progress?.Invoke(this, new ProgressReport(1));
            return new OptimizationResult()
            {
                OptimizationInput = input,
                ResultState = ResultState.Finished,
                Routes = routes.ToArray(),
                TimeElapsed = sw.ElapsedMilliseconds / 1000,
            };
        }

        private List<Route> CreateRoutes()
        {
            var routes = new List<Route>(starterData.Routes.Length);

            foreach (var (santaId, day, visits) in starterData.Routes)
            {
                var route = new Route()
                {
                    SantaId = santaId
                };

                if (visits.Length == 0)
                {
                    // empty route
                    route.Waypoints = new Waypoint[0];
                    routes.Add(route);
                    continue;
                }

                var waypoints = new List<Waypoint>();

                // add start
                waypoints.Add(new Waypoint()
                {
                    VisitId = Constants.VisitIdHome,
                    StartTime = input.Days[day].from,
                }
                );

                foreach (var visit in visits)
                {
                    waypoints.Add(CreateNextWaypoint(waypoints[waypoints.Count - 1], visit));
                }
                waypoints.Add(CreateNextWaypoint(waypoints[waypoints.Count - 1], Constants.VisitIdHome));

                route.Waypoints = waypoints.ToArray();
                routes.Add(route);
            }

            return routes;
        }

        private Waypoint CreateNextWaypoint(Waypoint previousWaypoint, int visitId)
        {
            var waypoint = new Waypoint()
            {
                VisitId = visitId,
            };
            var previousVisit = input.Visits.Cast<Visit?>().FirstOrDefault(v => v != null && v.Value.Id == previousWaypoint.VisitId);
            var visit = input.Visits.Cast<Visit?>().FirstOrDefault(v => v != null && v.Value.Id == visitId);
            if (previousWaypoint.VisitId == Constants.VisitIdHome)
            {
                // home to visit
                waypoint.StartTime = previousWaypoint.StartTime + visit.Value.WayCostFromHome;
            }
            else if (visitId == Constants.VisitIdHome)
            {
                // visit to home
                waypoint.StartTime = previousWaypoint.StartTime + previousVisit.Value.Duration + previousVisit.Value.WayCostToHome;
            }
            else
            {
                // visit to visit
                waypoint.StartTime = previousWaypoint.StartTime + previousVisit.Value.Duration + input.RouteCosts[previousWaypoint.VisitId, visitId];
            }
            return waypoint;
        }
    }
}
