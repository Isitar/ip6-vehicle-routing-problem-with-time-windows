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
        // Todo is this still needed?
        /*[HttpPost]
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

                var config = new ILPConfig()
                {
                    TimeSliceDuration = algorithmStarter.TimeSliceDuration,
                    ClusteringMIPGap = Properties.Settings.Default.MIPGapClustering,
                    ClusteringTimeLimitMiliseconds = Properties.Settings.Default.TimelimitClusteringMiliseconds,
                    SchedulingMIPGap = Properties.Settings.Default.MIPGapScheduling,
                    SchedulingTimeLimitMiliseconds = Properties.Settings.Default.TimelimitSchedulingMiliseconds,
                };

                var ilpSolver = new ILPSolver(optimizationInput, config);
                var progress = new EventHandler<ProgressReport>((sender, i) => { Console.WriteLine($"Progress: {i}"); });
                var consoleProgress = new EventHandler<String>((sender, msg) => { Console.WriteLine(msg); });
                return ilpSolver.Solve(0, progress, consoleProgress);
            }
        }*/

        /// <summary>
        /// Starts a new route calculation job
        /// </summary>
        /// <param name="algorithmStarter"></param>
        /// <returns>the id of the route calcluation job</returns>
        [HttpPost]
        [Route("StartRouteCalculation")]
        public long StartRouteCalculation([FromBody]AlgorithmStarter algorithmStarter)
        {
            long ret = 0;
            var calculations = RouteCalculationFactory.CreateRouteCalculation(algorithmStarter);
            for (int i = 0; i < calculations.Length; i++)
            {
                var rc = calculations[i];
                using (var dbSession = SessionFactory.Instance.OpenSession())
                {
                    rc = dbSession.Merge(rc);
                }
                Task.Run(() => new RouteCalculator(rc).StartWorker());
                ret = rc.Id;
            }
            return ret;
        }

        private AlgorithmType[] ExtendAlgorithmTypes(AlgorithmType type)
        {
            switch (type)
            {
                case AlgorithmType.Hybrid:
                    return new[]
                    {
                        AlgorithmType.GoogleRouting,
                        AlgorithmType.GeneticAlgorithm,
                    };
            }
            return new[]
            {
                type,
            };
        }

        [HttpGet]
        [Route("RouteCalculations")]
        public IEnumerable<RouteCalculationDTO> GetRouteCalculations()
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

                var routeCalculations = dbSession.Query<RouteCalculation>().ToList().Select(rc => (RouteCalculationDTO)rc);
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
                    var v = dbSession.Get<Visit>(wp.VisitId == Constants.VisitIdHome ? routeCalculation.StarterVisitId : routeCalculationResult.VisitMap[wp.VisitId]);
                    return new
                    {
                        Visit = (VisitDTO)v,
                        VisitStartTime = routeCalculationResult.ConvertTime(wp.StartTime),
                        VisitEndtime = routeCalculationResult.ConvertTime(wp.StartTime).AddSeconds(v.Duration),
                        SantaName = dbSession.Get<Santa>(routeCalculationResult.SantaMap[r.SantaId])?.Name,
                    };
                }).ToList()).ToList();

                return ret;
            }
        }
    }
}
