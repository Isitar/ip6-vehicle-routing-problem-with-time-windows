using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm;
using SolverInputData = IRuettae.Core.Algorithm.NoTimeSlicing.SolverInputData;

namespace IRuettae.ConsoleApp
{
    public class TestNewSolution
    {
        private const ConsoleColor InfoColor = ConsoleColor.Cyan;
        private const ConsoleColor ResultColor = ConsoleColor.Green;
        public static void Test()
        {
            ExportMPSVisits(33);
            TestAlgorithm(33);
        }



        private static void TestAlgorithm(int n_visits)
        {
            ConsoleExt.WriteLine($"Start testing algorithm with {n_visits} visits", InfoColor);
            TestSerailDataVisits($"SerializedObjects/SolverInputNew{n_visits}Visits.serial");
        }

        private static void ExportMPSVisits(int n_visits)
        {
            var solverInputData = Deserialize($"SerializedObjects/SolverInputNew{n_visits}Visits.serial");
            Starter.SaveMps($"New_{n_visits}_mps.mps", solverInputData, TargetBuilderType.Default);
            ConsoleExt.WriteLine($"Saved mps for {n_visits} visits", InfoColor);
        }
        private static void TestSerailDataVisits(string serialDataName, int numberOfRuns = 5)
        {
            var solverInputData = Deserialize(serialDataName);

            for (int i = 1; i <= numberOfRuns; i++)
            {
                var sw = Stopwatch.StartNew();

                var route = Starter.Optimise(solverInputData, useNewSovler: true);
                sw.Stop();
                ConsoleExt.WriteLine($"{i}/{numberOfRuns}: Elapsed s: {sw.ElapsedMilliseconds / 1000}", InfoColor);

                Console.WriteLine();
                Console.WriteLine();

                foreach (var santaDayWaypoints in route.Waypoints)
                {
                    ConsoleExt.WriteLine(santaDayWaypoints.Aggregate("",(carry, n) => carry + Environment.NewLine + solverInputData.VisitNames[n.visit]), ResultColor);
                }
            }
        }

        private static SolverInputData Deserialize(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                return (SolverInputData)new BinaryFormatter().Deserialize(stream);
            }
        }

        private void TestFakeData()
        {
            const int numberOfDays = 1;
            const int numberOfSantas = 2;
            const int numberOfVisits = 5;
            var santas = new bool[numberOfDays, numberOfSantas]
            {
                { true, true },
                // { true, true }
            };

            var visitsDuration = new int[numberOfVisits] { 0, 3, 3, 3, 3 };

            var t = VisitState.Default;
            var visits = new VisitState[numberOfDays, numberOfVisits]
            {
                { t, t, t, t, t},
                //   { t, t, t, t, t},
            };


            var distances = new int[numberOfVisits, numberOfVisits]
            {
                {0, 2, 2, 2, 2 },
                {2, 0, 1, 3, 4 },
                {2, 1, 0, 2, 3 },
                {2, 3, 2, 0, 1 },
                {2, 4, 3, 1, 0 },
            };
            var dayDuration = new int[numberOfDays]
            {
                10,
                //10
            };
            var solverInputData = new SolverInputData(santas, visitsDuration, visits, distances, dayDuration);
            Starter.Optimise(solverInputData, TargetBuilderType.Default, true);

        }
    }
}
