using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;

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
                System.IO.File.WriteAllText("distances.txt", JsonConvert.SerializeObject(distance));
                System.IO.File.WriteAllText("durations.txt", JsonConvert.SerializeObject(duration));
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

        public void FillRemainingWays()
        {
            var rnd = new Random();
            var orsKey = Environment.GetEnvironmentVariable("open-route-service-key");
            var openRouteServiceCalculator =
                new GeoCalculations.RouteCalculation.OpenRouteServiceCalculator(orsKey);

            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var fromVisits = dbSession.Query<Visit>().Where(v => v.Year == 2018 && v.Id == 133)

                    .ToArray();
                var toVisits = dbSession.Query<Visit>().Where(v => v.Year == 2018 && v.Id == 108)
                    .ToArray();
                foreach (var fromVisit in fromVisits)
                {
                    foreach (var toVisit in toVisits)
                    {
                        var (distance, duration) = openRouteServiceCalculator.CalculateWalkingDistance((fromVisit.Lat, fromVisit.Long),
                            (toVisit.Lat, toVisit.Long));
                        var way = new Way
                        {
                            Distance = (int)Math.Ceiling(distance),
                            Duration = (int)Math.Ceiling(duration),
                            From = fromVisit,
                            To = toVisit
                        };
                        dbSession.Save(way);
                        Thread.Sleep((int)(300 + rnd.NextDouble() * 100));
                    }
                }
            }
        }

        public void FromTxtFile()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var visits = dbSession.Query<Visit>().Where(v => v.Year == 2018).ToArray();
                var distances = JsonConvert.DeserializeObject<double[,]>(System.IO.File.ReadAllText("distances.txt"));
                var durations = JsonConvert.DeserializeObject<double[,]>(System.IO.File.ReadAllText("distances.txt"));
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        var way = new Way
                        {
                            Distance = (int)Math.Ceiling(distances[i, j]),
                            Duration = (int)Math.Ceiling(durations[i, j]),
                            From = visits[i],
                            To = visits[j]
                        };
                        dbSession.Save(way);
                    }
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
