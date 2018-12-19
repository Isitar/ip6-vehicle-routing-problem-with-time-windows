using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.GeoCalculations.RouteCalculation
{
    public class DebugRouteCalculator : IRouteCalculator
    {
        /// <summary>
        /// Maximal distance between from and to in meters
        /// </summary>
        private const double MaxDistance = 2000;

        /// <summary>
        /// Walking speed of a pedestrian in meter per second
        /// </summary>
        private const double WalkingSpeed = 1.1;

        /// <summary>
        /// Always the same seed for reproducability
        /// </summary>
        private readonly Random random = new Random(0);

        public (double distance, double duration) CalculateWalkingDistance(string from, string to)
        {
            var distance = random.NextDouble() * MaxDistance;
            return (distance, distance / WalkingSpeed);
        }

        public (double distance, double duration) CalculateWalkingDistance(double fromLat, double fromLong, double toLat, double toLong)
        {
            return CalculateWalkingDistance("", "");
        }
    }
}
