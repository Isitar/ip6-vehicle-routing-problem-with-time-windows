using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Persistence;

namespace IRuettae.ConsoleApp.Programs
{
    class DataSet8Helper
    {
        public void CallDistanceMatrixFunctionality()
        {
            var orsKey = Environment.GetEnvironmentVariable("open-route-service-key");
            var openRouteServiceCalculator =
                new GeoCalculations.RouteCalculation.OpenRouteServiceCalculator(orsKey);

            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var visits = dbSession.Query<Visit>().Where(v => v.Year == 2018).ToArray();
                var coordinates = visits.Select(v => (v.Lat, v.Long)).ToArray();
                var (distance, duration) = openRouteServiceCalculator.CalculateWalkingDistanceMatrix(coordinates);
                for (int i = 0; i < coordinates.Length; i++)
                {
                    for (int j = 0; j < coordinates.Length; j++)
                    {
                        visits[i].ToWays.Add(new Way
                        {
                            Distance = (int)Math.Ceiling(distance[i, j]),
                            Duration = (int)Math.Ceiling(duration[i, j]),
                            From = visits[i],
                            To = visits[j]
                        });
                    }
                    dbSession.Update(visits[i]);
                }
            }
        }

        public void GeocodeAllVisits()
        {
            var geocoder = new GeoCalculations.Geocoding.NominatimGeocoder("luescherpascal@gmail.com", "ch", "ip6.isitar.ch");
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var visits = dbSession.Query<Visit>().Where(v => v.Year == 2018 && Math.Abs(v.Lat) < 0.001).ToList();
                foreach (var visit in visits)
                {
                    try
                    {
                        (var lat, var lng) = geocoder.Locate($"{visit.Street}, {visit.Zip} {visit.City}");
                        visit.Lat = lat;
                        visit.Long = lng;
                        dbSession.Update(visit);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error {e.Message} for visit {visit.Id}");
                    }
                }
                dbSession.Flush();
            }
        }
    }
}
