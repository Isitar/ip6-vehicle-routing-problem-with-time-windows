using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.ExtensionMethods;
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
            if (way.From.RouteCalcAddress().Equals(way.To.RouteCalcAddress()))
            {
                way.Duration = 0;
                way.Distance = 0;
                return;
            }

            var routeCalculator = DependencyResolver.Current.GetService<IRouteCalculator>();
            var (distance, duration) = routeCalculator.CalculateWalkingDistance(way.From.RouteCalcAddress(), way.To.RouteCalcAddress());
            way.Distance = Convert.ToInt32(distance);
            way.Duration = Convert.ToInt32(duration);
        }

    }
}