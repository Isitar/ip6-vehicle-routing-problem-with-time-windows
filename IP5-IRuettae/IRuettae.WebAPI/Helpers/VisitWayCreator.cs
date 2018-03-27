using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Persistence;
using IRuettae.WebApi.Properties;
using NHibernate;

namespace IRuettae.WebApi.Helpers
{
    public class VisitWayCreator
    {

        public static void CreateWays(Visit visit)
        {
            // Todo: add dependency injection
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                using (var transaction = dbSession.BeginTransaction())
                {
                    var otherAddresses = dbSession.Query<Visit>().Where(v => v.Year == visit.Year);

                    foreach (var otherAddress in otherAddresses)
                    {
                        try
                        {
                            CreateWay(dbSession, visit, otherAddress);
                            CreateWay(dbSession, otherAddress, visit);
                        }
                        catch (RouteNotFoundException)
                        {
                            // unable to create Ways
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        private static void CreateWay(ISession dbSession, Visit from, Visit to)
        {
            if (dbSession.Query<Way>().Any(w => w.From.Id == from.Id && w.To.Id == to.Id))
            {
                // Way already exists
                return;
            }

            var way = new Way
            {
                From = from,
                To = to,
            };
            UpdateWayDistanceDuration(way);
            way = dbSession.Merge(way);
        }

        private static void UpdateWayDistanceDuration(Way way)
        {
            // if from == to return
            if (RouteCalcAddress(way.From).Equals(RouteCalcAddress(way.To)))
            {
                way.Duration = 0;
                way.Distance = 0;
                return;
            }

            var routeCalculator = DependencyResolver.Current.GetService<IRouteCalculator>();
            var (distance, duration) = routeCalculator.CalculateWalkingDistance(RouteCalcAddress(way.From), RouteCalcAddress(way.To));
            way.Distance = Convert.ToInt32(distance);
            way.Duration = Convert.ToInt32(duration);
        }

        private static string RouteCalcAddress(Visit v) => $"{v.Street} {v.Zip}";
    }
}