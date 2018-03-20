using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Persistence;
using IRuettae.WebApi.Properties;

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

        private static void UpdateWayDistanceDuration(Way way)
        {
            // if from == to return
            if (RouteCalcAddress(way.From).Equals(RouteCalcAddress(way.To)))
            {
                way.Duration = 0;
                way.Distance = 0;
                return;
            }

            // Todo: add dependency injection
            IRouteCalculator routeCalculator =
                new GeoCalculations.RouteCalculation.GoogleRouteCalculator(Settings.Default.GoogleAPIKey);
            var (distance, duration) = routeCalculator.CalculateWalkingDistance(RouteCalcAddress(way.From), RouteCalcAddress(way.To));
            way.Distance = Convert.ToInt32(distance);
            way.Duration = Convert.ToInt32(duration);
        }

        private static string RouteCalcAddress(Visit v) => $"{v.Street} {v.Zip}";
    }
}