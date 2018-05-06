using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
        //    ExportMPSVisits(5);
        //    ExportMPSVisits(10);
        //    ExportMPSVisits(15);
        //    ExportMPSVisits(20);
        //    ExportMPSVisits(29);
            TestAlgorithm(5);
        }



        private static void TestAlgorithm(int n_visits)
        {
            TestSerailDataVisits($"SerializedObjects/SolverInput{n_visits}Visits.serial");
        }

        private static void ExportMPSVisits(int n_visits)
        {
            var solverInputData = Deserialize($"SerializedObjects/SolverInput{n_visits}Visits.serial");
            Starter.SaveMps($"{n_visits}_mps.mps", solverInputData, TargetBuilderType.Default);
        }

        private static void TestSerailDataVisits(string serialDataName)
        {
            var solverInputData = Deserialize(serialDataName);

            var sw = Stopwatch.StartNew();
            Starter.Optimise(solverInputData);
            sw.Stop();
            Console.WriteLine("Elapsed ms: " + sw.ElapsedMilliseconds);
        }

        private static SolverInputData Deserialize(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                return (SolverInputData)new BinaryFormatter().Deserialize(stream);
            }
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
