using System;
using System.Collections.Generic;
using System.IO;
using IRuettae.Core;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.Models;
using Newtonsoft.Json;

namespace IRuettae.Evaluator
{
    class Program
    {
        /// <summary>
        /// Dictionary used for selection
        /// </summary>
        static readonly Dictionary<int, string> AlgorithmsDictionary = new Dictionary<int, string>()
        {
            {1,"ILP"},
            {2, "GA" },
            {3, "LocalSolver" },
        };

        static readonly Dictionary<int, string> DatasetDictionary = new Dictionary<int, string>()
        {
            {1, "10 visits, 1 santa 2 days"},
            {2, "10 visits, 1 santas, 2 days 5 desired d1, 5 desired d2" },
            {3, "10 visits, 1 santas, 2 days 5 unavailable d1, 5 unavailable d2" },
            {4, "20 visits, 2 santas"},
            {5, "20 visits, 2 santas, 2 days, 10 desired d1, 10 desired d2" },
            {6, "20 visits, 2 santas, 2 days, 10 unavailable d1, 10 unavailable d2" },
            {7, "Real example 2017" },
            {8, "Real example 2018" },
            {9, "50 visits, 5 santas, 2 days, 15 desired d1, 15 desired d2, 11 unavailable d1, 11 unavailable d2" },
            {10, "100 visits, 10 santas, 2 days, 35 desired d1, 35 desired d2, 20 unavailable d1, 20 unavailable d2" },
            {11, "200 visits, 20 santas, 2 days, 75 desired d1, 75 desired d2, 40 unavailable d1, 40 unavailable d2" },
            {12, "1000 visits, 100 santas, 2 days, 300 desired d1, 300 desired d2, 150 unavailable d1, 150 unavailable d2" },
        };

        static void Main(string[] args)
        {
            EvaluateAlgorithm();
            //TestResultDrawer();
            Console.Write("Press any key to exit: ");
            Console.Read();
        }

        private static void TestResultDrawer()
        {
            var result =
                JsonConvert.DeserializeObject<OptimizationResult>(File.ReadAllText("18-11-10-00-31-22_Dataset_3_ILP.json"));

            ResultDrawer.DrawResult("debug.gif", result, DatasetFactory.DataSet3().coordinates);
        }

        private static void EvaluateAlgorithm()
        {
            BigHr();
            Console.WriteLine("Program written to evaluate the different optimisation algorithms.");
            Console.WriteLine();

            Console.WriteLine("Pleace choose which algorithm to evaluate");
            foreach (var algorithm in AlgorithmsDictionary)
            {
                Console.WriteLine($"{algorithm.Key}: {algorithm.Value}");
            }

            int algorithmSelection;
            do
            {
                Console.Write("Enter number: ");
                var enteredNumber = Console.ReadLine();
                if (!(int.TryParse(enteredNumber, out algorithmSelection) &&
                      AlgorithmsDictionary.ContainsKey(algorithmSelection)))
                {
                    algorithmSelection = 0;
                    Console.WriteLine("Please enter a valid number");
                }
            } while (algorithmSelection == 0);

            Console.WriteLine($"You selected {AlgorithmsDictionary[algorithmSelection]}");
            SmallHr();
            Console.WriteLine();
            Console.WriteLine("Please select the dataset");
            foreach (var dataset in DatasetDictionary)
            {
                Console.WriteLine($"{dataset.Key}: {dataset.Value}");
            }

            int datasetSelection;

            do
            {
                Console.Write("Enter number: ");
                var enteredNumber = Console.ReadLine();
                if (!(int.TryParse(enteredNumber, out datasetSelection) &&
                      DatasetDictionary.ContainsKey(datasetSelection)))
                {
                    datasetSelection = 0;
                    Console.WriteLine("Please enter a valid number");
                }
            } while (datasetSelection == 0);

            Console.WriteLine($"You selected dataset {datasetSelection}: {DatasetDictionary[datasetSelection]}");
            SmallHr();
            Console.WriteLine();
            Console.WriteLine("Starting the algorithm now");
            BigHr();
            OptimizationInput input;
            (int, int)[] coordinates = null;
            string savepath = $"{DateTime.Now:yy-MM-dd-HH-mm-ss}";
            int timelimit = 0;
            switch (datasetSelection)
            {
                case 1:
                    (input, coordinates) = DatasetFactory.DataSet1();
                    timelimit = 10 * 60 * 1000;
                    break;
                case 2:
                    (input, coordinates) = DatasetFactory.DataSet2();
                    timelimit = 10 * 60 * 1000;
                    break;
                case 3:
                    (input, coordinates) = DatasetFactory.DataSet3();
                    timelimit = 10 * 60 * 1000;
                    break;
                case 4:
                    (input, coordinates) = DatasetFactory.DataSet4();
                    timelimit = 20 * 60 * 1000;
                    break;
                case 5:
                    (input, coordinates) = DatasetFactory.DataSet5();
                    timelimit = 20 * 60 * 1000;
                    break;
                case 6:
                    (input, coordinates) = DatasetFactory.DataSet6();
                    timelimit = 20 * 60 * 1000;
                    break;
                case 7:
                    throw new NotImplementedException();
                    //(input, coordinates) = DatasetFactory.DataSet7();
                    //timelimit = 90 * 60 * 1000;
                    break;
                case 8:
                    throw new NotImplementedException();
                    //(input, coordinates) = DatasetFactory.DataSet8();
                    //timelimit = 90 * 60 * 1000;
                    break;
                case 9:
                    (input, coordinates) = DatasetFactory.DataSet9();
                    timelimit = 90 * 60 * 1000;
                    break;
                case 10:
                    (input, coordinates) = DatasetFactory.DataSet10();
                    timelimit = 120 * 60 * 1000;
                    break;
                case 11:
                    (input, coordinates) = DatasetFactory.DataSet11();
                    timelimit = 120 * 60 * 1000;
                    break;
                case 12:
                    (input, coordinates) = DatasetFactory.DataSet12();
                    timelimit = 120 * 60 * 1000;
                    break;
            }

            savepath += $"_Dataset_{datasetSelection}";
            ISolver solver = null;
            switch (algorithmSelection)
            {
                case 1:
                    solver = new ILPSolver(input, new ILPStarterData
                    {
                        ClusteringMIPGap = 0,
                        SchedulingMIPGap = 0,

                        ClusteringTimeLimitMiliseconds = (long)(0.7 * timelimit),
                        SchedulingTimeLimitMiliseconds = (long)(0.3 * timelimit),
                        TimeSliceDuration = 120
                    });
                    savepath += "_ILP";
                    break;
            }

            var result = solver.Solve(timelimit, (sender, report) => Console.WriteLine($"Progress: {report}"),
                (sender, s) => Console.WriteLine($"Info: {s}"));
            BigHr();

            File.WriteAllText(savepath + ".json", JsonConvert.SerializeObject(result));
            Console.WriteLine();
            Console.WriteLine("Done solving");
            Console.WriteLine($"TimeElapsed [s]: {result.TimeElapsed}");
            Console.WriteLine($"Target function value: {result.Cost()}");
            ResultDrawer.DrawResult(savepath, result, coordinates);
        }

        private static void SmallHr()
        {
            Console.WriteLine(new string('-', 40));
        }
        private static void BigHr()
        {
            Console.WriteLine(new string('-', 100));
        }
    }
}
