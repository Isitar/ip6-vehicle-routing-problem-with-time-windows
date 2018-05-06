using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm;
using IRuettae.GeoCalculations.RouteCalculation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IRuettae.ConsoleApp
{
    class Test
    {
        internal static void Run(string[] args)
        {
            TestRealDataAlgorithm();
            //TestGeolocator();
        }


        private static void TestRealDataAlgorithm()
        {
            SolverInputData solverInputData;
            // test
            using (var stream = File.Open("SerializedObjects/SolverInput10Visits.serial", FileMode.Open))
            {
                solverInputData = (SolverInputData)new BinaryFormatter().Deserialize(stream);
            }
            
            var sw = Stopwatch.StartNew();
            Starter.Optimise(solverInputData);
            sw.Stop();
            Console.WriteLine("Elapsed ms: " + sw.ElapsedMilliseconds);
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
