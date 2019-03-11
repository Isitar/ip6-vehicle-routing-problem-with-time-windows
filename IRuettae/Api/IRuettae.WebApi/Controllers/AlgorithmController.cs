using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
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
        /// <summary>
        /// Starts a new route calculation job
        /// </summary>
        /// <param name="algorithmStarter"></param>
        /// <returns>the id of the route calcluation job</returns>
        [HttpPost]
        [Route("StartRouteCalculation")]
        public long StartRouteCalculation([FromBody]AlgorithmStarter algorithmStarter)
        {
            var routeCalculation = RouteCalculationFactory.CreateRouteCalculation(algorithmStarter);
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                routeCalculation = dbSession.Merge(routeCalculation);
            }

            RouteCalculator.EnqueueRouteCalculation(routeCalculation.Id);


            return routeCalculation.Id;

        }


        [HttpGet]
        [Route("RouteCalculations")]
        public IEnumerable<RouteCalculationDTO> GetRouteCalculations()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
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

                var ret = routeCalculationResult.OptimizationResult.Routes.Select(r => r.Waypoints?.Select(wp =>
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
