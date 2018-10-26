using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.Models;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;
using Santa = IRuettae.Persistence.Entities.Santa;
using Visit = IRuettae.Persistence.Entities.Visit;

namespace IRuettae.WebApi.Controllers
{
    [RoutePrefix("api/algorithm")]
    public class AlgorithmController : ApiController
    {
        [HttpPost]
        public OptimizationResult CalculateRoute([FromBody] AlgorithmStarter algorithmStarter)
        {

            using (var dbSession = SessionFactory.Instance.OpenSession())
            using (var transaction = dbSession.BeginTransaction())
            {
                var visits = dbSession.Query<Visit>().Where(v => v.Year == algorithmStarter.Year && v.Id != algorithmStarter.StarterId).ToList();
                visits.ForEach(v => v.Duration = 60 * (v.NumberOfChildren * algorithmStarter.TimePerChild + algorithmStarter.Beta0));
                var converter = new Converter.PersistenceToCoreConverter();

                var optimizationInput = converter.Convert(algorithmStarter.Days, dbSession.Query<Visit>().First(v => v.Id == algorithmStarter.StarterId), visits,
                    dbSession.Query<Santa>().ToList());

                var starterData = new ILPStarterData()
                {
                    TimeSliceDuration = algorithmStarter.TimeSliceDuration,
                    ClusteringMIPGap = Properties.Settings.Default.MIPGapClustering,
                    ClusteringTimeLimit = Properties.Settings.Default.TimelimitClustering,
                    SchedulingMIPGap = Properties.Settings.Default.MIPGapScheduling,
                    SchedulingTimeLimit = Properties.Settings.Default.TimelimitScheduling,
                };

                var ilpSolver = new ILPSolver(optimizationInput, starterData);
                var progress = new EventHandler<ProgressReport>((sender, i) => { Console.WriteLine($"Progress: {i}"); });
                var consoleProgress = new EventHandler<String>((sender, msg) => { Console.WriteLine(msg); });
                return ilpSolver.Solve(0, progress, consoleProgress);
            }
        }
        /// <summary>
        /// Starts a new route calculation job
        /// </summary>
        /// <param name="algorithmStarter"></param>
        /// <returns>the id of the route calcluation job</returns>
        [HttpPost]
        [Route("StartRouteCalculation")]
        public long StartRouteCalculation([FromBody]AlgorithmStarter algorithmStarter)
        {

            RouteCalculation rc;

            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var ilpData = new ILPStarterData()
                {
                    TimeSliceDuration = algorithmStarter.TimeSliceDuration,
                    ClusteringOptimizationFunction = ClusteringOptimizationGoals.OverallMinTime,
                    ClusteringMIPGap = Properties.Settings.Default.MIPGapClustering,
                    ClusteringTimeLimit = Properties.Settings.Default.TimelimitClustering,
                    SchedulingMIPGap = Properties.Settings.Default.MIPGapScheduling,
                    SchedulingTimeLimit = Properties.Settings.Default.TimelimitScheduling,
                };
                rc = new RouteCalculation
                {
                    Days = algorithmStarter.Days,
                    SantaJson = "",
                    VisitsJson = "",
                    StarterVisitId = algorithmStarter.StarterId,
                    State = RouteCalculationState.Creating,
                    TimePerChild = algorithmStarter.TimePerChild,
                    TimePerChildOffset = algorithmStarter.Beta0,
                    Year = algorithmStarter.Year,
                    Algorithm = AlgorithmType.ILP,
                    AlgorithmData = JsonConvert.SerializeObject(ilpData),
                };
                rc = dbSession.Merge(rc);
            }

            Task.Run(() => new RouteCalculator(rc).StartWorker());
            return rc.Id;
        }

        [HttpGet]
        [Route("RouteCalculations")]
        public IEnumerable<RouteCalculation> GetRouteCalculations()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                if (RouteCalculator.BackgroundWorkers.IsEmpty)
                {
                    dbSession.Query<RouteCalculation>()
                        .Where(rc => new[] { RouteCalculationState.Ready, RouteCalculationState.Running }.Contains(rc.State))
                        .ToList()
                        .ForEach(rc =>
                        {
                            rc.State = RouteCalculationState.Cancelled;
                            dbSession.Update(rc);
                        });
                    dbSession.Flush();
                }

                var routeCalculations = dbSession.Query<RouteCalculation>().ToList();
                return routeCalculations;
            }
        }


        [HttpGet]
        [Route("RouteCalculationWaypoints")]
        public IEnumerable<object> RouteCalculationWaypoints(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var routeCalculation = dbSession.Get<RouteCalculation>(id);
                var routeCalculationResult = JsonConvert.DeserializeObject<RouteCalculationResult>(routeCalculation.Result);

                var ret = routeCalculationResult.OptimizationResult.Routes.Select(r => r.Waypoints.Select(wp =>
                {
                    var v = dbSession.Get<Visit>(wp.VisitId == -1 ? routeCalculation.StarterVisitId : routeCalculationResult.VisitMap[wp.VisitId]);
                    return new
                    {
                        Visit = (VisitDTO)v,
                        VisitStartTime = routeCalculationResult.ConvertTime(wp.StartTime),
                        VisitEndtime = routeCalculationResult.ConvertTime(wp.StartTime).AddSeconds(v.Duration),
                        SantaName = dbSession.Get<Santa>(routeCalculationResult.VisitMap[r.SantaId])?.Name,
                    };
                }).ToList()).ToList();

                return ret;
            }
        }
    }
}
