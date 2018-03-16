using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Http;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Persistence;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;

namespace IRuettae.WebApi.Controllers
{
    public class VisitController : ApiController
    {
        public IEnumerable<Visit> Get()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var result = dbSession.Query<Visit>().ToList();
                return result;
            }

        }

        public Visit Get(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                return dbSession.Get<Visit>(id);
            }
        }


        public void Post([FromBody]Visit visit)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                using (var transaction = dbSession.BeginTransaction())
                {
                    visit = dbSession.Merge(visit);
                    transaction.Commit();
                }

                using (var transaction = dbSession.BeginTransaction())
                {

                    var otherAddresses = dbSession.Query<Visit>().Where(v => v.Year == visit.Year);
                    foreach (var otherAddress in otherAddresses)
                    {

                        var way = new Way
                        {
                            From = visit,
                            To = otherAddress,
                        };
                        UpdateWayDistanceDuration(way);
                        way = dbSession.Merge(way);

                        var wayBack = new Way
                        {
                            From = otherAddress,
                            To = visit,
                        };
                        UpdateWayDistanceDuration(wayBack);
                        wayBack = dbSession.Merge(wayBack);
                    }
                    transaction.Commit();
                }


            }
        }

        private string RouteCalcAddress(Visit v) => $"{v.Street} {v.Zip}";

        private void UpdateWayDistanceDuration(Way way)
        {
            // Todo: add dependency injection and add key to config file
            IRouteCalculator routeCalculator =
                new GeoCalculations.RouteCalculation.GoogleRouteCalculator(
                    "AIzaSyAdTPEkyVKvA0ZvVNAAZK5Ot3fl8zyBsks");
            var (distance, duration) = routeCalculator.CalculateWalkingDistance(RouteCalcAddress(way.From), RouteCalcAddress(way.To));
        }

        public void Put(long id, [FromBody]Visit visit)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                using (var transaction = dbSession.BeginTransaction())
                {
                    var origVisit = dbSession.Get<Visit>(id);
                    origVisit.NumberOfChildrean = visit.NumberOfChildrean;
                    origVisit.Street = visit.Street;
                    origVisit.Year = visit.Year;
                    origVisit.Zip = visit.Zip;
                    origVisit.ExternalReference = visit.ExternalReference;
                    // ignore times
                    transaction.Commit();
                }
            }
        }


        public void Delete(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                using (var transaction = dbSession.BeginTransaction())
                {
                    var origVisit = dbSession.Get<Visit>(id);
                    dbSession.Delete(origVisit);
                    transaction.Commit();
                }
            }
        }
    }
}
