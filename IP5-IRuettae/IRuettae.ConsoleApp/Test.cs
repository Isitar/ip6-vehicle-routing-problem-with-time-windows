using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.GeoCalculations.RouteCalculation;

namespace IRuettae.ConsoleApp
{
    class Test
    {
        internal static void Run(string[] args)
        {
            TestGeolocator();
        }


        private static void TestGeolocator()
        {
            var key = "AIzaSyAdTPEkyVKvA0ZvVNAAZK5Ot3fl8zyBsks";
            var routeCalculator = new GoogleRouteCalculator(key);
            var (distance, duration) = routeCalculator.CalculateWalkingDistance("Othmarsingerstrasse 18 5600 Lenzburg", "Migros Lenzburg");
            Console.WriteLine($"Distance [m]: {distance}, Duration [s]: {duration}");
        }
    }
}
