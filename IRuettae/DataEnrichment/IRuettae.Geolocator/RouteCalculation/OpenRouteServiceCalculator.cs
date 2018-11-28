using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IRuettae.GeoCalculations.RouteCalculation
{
    public class OpenRouteServiceCalculator : IRouteCalculator
    {
        public (double distance, double duration) CalculateWalkingDistance(string @from, string to)
        {
            throw new NotImplementedException();
        }

        private readonly string apiKey;

        public OpenRouteServiceCalculator(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public (double[,] distance, double[,] duration) CalculateWalkingDistanceMatrix(
            (double lat, double lng)[] coordinates)
        {
            var callUrl = $"https://api.openrouteservice.org/matrix?api_key={apiKey}&profile=foot-walking";

            var retJson = new WebClient().UploadString(callUrl,
                "POST",
                $"{{ \"profile\":\"foot-walking\"," +
                $" \"locations\": [{string.Join(",", coordinates.Select(c => $"[{c.lng},{c.lat}]"))}]," +
                $"\"metrics\":\"duration|distance\"" +
                $"}}");

            dynamic retData = JObject.Parse(retJson);
            var distances = retData.distances;
            var retDistances = new double[coordinates.Length, coordinates.Length];
            for (int i = 0; i < distances.Count; i++)
            {
                for (int j = 0; j < distances[i].Count; j++)
                {
                    retDistances[i, j] = distances[i][j];
                }
            }

            var durations = retData.durations;
            var retDurations = new double[coordinates.Length, coordinates.Length];
            for (int i = 0; i < durations.Count; i++)
            {
                for (int j = 0; j < durations[i].Count; j++)
                {
                    retDurations[i, j] = durations[i][j];
                }
            }
            return (distance: retDistances, duration: retDurations);
        }

        public (double distance, double duration) CalculateWalkingDistance(double fromLat, double fromLong, double toLat,
            double toLong)
        {
            var result = CalculateWalkingDistanceMatrix(new[] { (fromLat, fromLong), (toLat, toLong) });
            return (result.distance[0, 1], result.duration[0, 1]);
        }
    }
}
