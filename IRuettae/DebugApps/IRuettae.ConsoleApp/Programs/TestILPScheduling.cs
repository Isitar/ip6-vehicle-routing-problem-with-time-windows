using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILP.Algorithm;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using IRuettae.GeoCalculations.RouteCalculation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IRuettae.ConsoleApp
{
    internal class TestILPScheduling
    {
        private const ConsoleColor OutputColor = ConsoleColor.Green;
        internal static void Run(string[] args)
        {

            //ExportMPSVisits(5);
            //ExportMPSVisits(10);
            //ExportMPSVisits(15);
            //ExportMPSVisits(20);
            //ExportMPSVisits(29);

            TestAlgorithm(5);
        }




        private static void TestAlgorithm(int n_visits)
        {
            ConsoleExt.WriteLine($"Start testing algorithm with {n_visits} visits", OutputColor);
            TestSerailDataVisits($"SerializedObjects/SolverInput{n_visits}Visits.serial");
        }

        private static void ExportMPSVisits(int n_visits)
        {
            var solverInputData = Deserialize($"SerializedObjects/SolverInput{n_visits}Visits.serial");
            var schedulingSolver = new SchedulingILPSolver(solverInputData);
            schedulingSolver.ExportMPSAsFile($"{n_visits}_mps.mps");
        }

        private static void TestSerailDataVisits(string serialDataName, int numberOfRuns = 5)
        {
            var solverInputData = Deserialize(serialDataName);

            for (int i = 1; i <= numberOfRuns; i++)
            {
                var sw = Stopwatch.StartNew();

                var solver = new SchedulingILPSolver(solverInputData);
                solver.Solve(0, 10 * 60 * 1000);
                var route = solver.GetResult();
                sw.Stop();
                ConsoleExt.WriteLine($"{i}/{numberOfRuns}: Elapsed s: {sw.ElapsedMilliseconds / 1000}", OutputColor);
                ConsoleExt.WriteLine($"SolutionVal: {route.SolutionValue}", ConsoleColor.Yellow);
            }
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
            ConsoleExt.WriteLine($"Distance [m]: {distance}, Duration [s]: {duration}", OutputColor);
        }
    }
}
