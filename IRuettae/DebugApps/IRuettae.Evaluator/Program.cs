using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using IRuettae.Core;
using IRuettae.Core.GeneticAlgorithm;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Google.Routing;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.ILPIp5Gurobi;
using IRuettae.Core.ILPIp5Gurobi.Algorithm.Models;
using IRuettae.Core.LocalSolver;
using IRuettae.Core.LocalSolver.Models;
using IRuettae.Core.Models;
using Newtonsoft.Json;

namespace IRuettae.Evaluator
{
    class Program
    {
        private enum Algorithms
        {
            ILP = 1,
            GA = 2,
            LocalSolver = 3,
            ILP2 = 4,
            ILPIP5Gurobi = 5,
            GoogleRouting = 6,
            ILPFast = 10,
            GAFast = 20,
            LocalSolverFast = 30,
            ILP2Fast = 40,
            ILPIP5GurobiFast = 50,
            GoogleRoutingFast = 60,
        }

        /// <summary>
        /// Dictionary used for selection
        /// </summary>
        static readonly Dictionary<Algorithms, string> AlgorithmsDictionary = new Dictionary<Algorithms, string>()
        {
            {Algorithms.ILP,"ILP"},
            {Algorithms.GA, "GA" },
            {Algorithms.LocalSolver, "LocalSolver" },
            {Algorithms.ILP2, "ILP 2" },
            {Algorithms.ILPIP5Gurobi, "ILP Ip5 Gurobi" },
            {Algorithms.GoogleRouting, "Google OR-Tools Routing" },
            {Algorithms.ILPFast,"ILP Fast"},
            {Algorithms.GAFast, "GA Fast" },
            {Algorithms.LocalSolverFast, "LocalSolver Fast" },
            {Algorithms.ILP2Fast, "ILP 2 Fast" },
            {Algorithms.ILPIP5GurobiFast, "ILP Ip5 Gurobi Fast" },
            {Algorithms.GoogleRoutingFast, "Google OR-Tools Routing Fast" },
        };

        static readonly Dictionary<int, string> DatasetDictionary = new Dictionary<int, string>()
        {
            {0, "All Datasets"},
            {1, "10 visits, 1 santa 2 days"},
            {2, "10 visits, 1 santas, 2 days 5 desired d1, 5 desired d2" },
            {3, "10 visits, 1 santas, 2 days 5 unavailable d1, 5 unavailable d2" },
            {4, "20 visits, 2 santas"},
            {5, "20 visits, 2 santas, 2 days, 10 desired d1, 10 desired d2, 4 breaks" },
            {6, "20 visits, 2 santas, 2 days, 10 unavailable d1, 10 unavailable d2" },
            {7, "Real example 2017" },
            {8, "Real example 2018" },
            {9, "50 visits, 5 santas, 2 days, 15 desired d1, 15 desired d2, 11 unavailable d1, 11 unavailable d2, 10 breaks" },
            {10, "100 visits, 10 santas, 2 days, 35 desired d1, 35 desired d2, 20 unavailable d1, 20 unavailable d2, 20 breaks" },
            {11, "200 visits, 20 santas, 2 days, 75 desired d1, 75 desired d2, 40 unavailable d1, 40 unavailable d2, 40 breaks" },
            {12, "1000 visits, 100 santas, 2 days, 300 desired d1, 300 desired d2, 150 unavailable d1, 150 unavailable d2, 200 breaks" },
            {55, "Datasets for desired / unavailable impact Tests Normal" },
            {56, "Datasets for desired / unavailable impact Tests Unavailable" },
            {57, "Datasets for desired / unavailable impact Tests Desired" },
        };

        static void Main(string[] args)
        {
            do
            {
                EvaluateAlgorithm(args);
                if (args.Length == 0)
                {
                    Console.Write("New run? [Y/N]: ");
                }

            } while (args.Length == 0 && Console.ReadLine().ToUpper().Equals("Y"));
        }

        private static void TestResultDrawer()
        {
            var result =
                JsonConvert.DeserializeObject<OptimizationResult>(File.ReadAllText("18-11-10-00-31-22_Dataset_3_ILP.json"));

            ResultDrawer.DrawResult("debug.gif", result, DatasetFactory.DataSet3().coordinates);
        }

        private static void EvaluateAlgorithm(string[] args)
        {
            BigHr();
            Console.WriteLine("Program written to evaluate the different optimisation algorithms.");
            Console.WriteLine();

            var algorithmSelection = args.Length == 0 ? QueryAlgorithmSelection() : (Algorithms)Enum.Parse(typeof(Algorithms), args[0]);

            SmallHr();
            Console.WriteLine();

            var datasetSelection = args.Length == 0 ? QueryDatasetSelection() : GetDatasetSelection(int.Parse(args[1]));

            SmallHr();
            Console.WriteLine();

            var runs = args.Length == 0 ? QueryNumberOfRuns() : int.Parse(args[2]);

            SmallHr();
            Console.WriteLine();
            Console.WriteLine("Starting the algorithm now");
            BigHr();


            for (int i = 0; i < runs; i++)
            {

                foreach (var dataset in datasetSelection)
                {
                    try
                    {
                        var (input, coordinates, timelimit) = GetDataset(dataset);
                        string savepath = $"{DateTime.Now:yy-MM-dd-HH-mm-ss}_DataSet_{dataset}";
                        ISolver solver = null;
                        var fastFactor = 60;
                        switch (algorithmSelection)
                        {
                            case Algorithms.ILPFast:
                                timelimit /= fastFactor;
                                goto case Algorithms.ILP;
                            case Algorithms.ILP:
                                solver = new ILPSolver(input, new ILPConfig
                                {
                                    ClusteringMIPGap = 0,
                                    SchedulingMIPGap = 0,

                                    ClusteringTimeLimitMiliseconds = (long)(0.7 * timelimit),
                                    SchedulingTimeLimitMiliseconds = (long)(0.3 * timelimit),
                                    TimeSliceDuration = 120
                                });
                                savepath += "_ILP";
                                break;
                            case Algorithms.LocalSolverFast:
                                timelimit /= fastFactor;
                                goto case Algorithms.LocalSolver;
                            case Algorithms.LocalSolver:
                                solver = new Solver(input, new LocalSolverConfig
                                {
                                    VrpTimeLimitFactor = 0.1,
                                    VrptwTimeLimitFactor = 0.8,
                                    MaxNumberOfAdditionalSantas = 0,
                                });
                                savepath += "_LocalSolver";
                                break;
                            case Algorithms.GA:
                                solver = new GenAlgSolver(input, new GenAlgConfig(input));
                                savepath += "_GA";
                                break;
                            case Algorithms.GAFast:
                                timelimit /= fastFactor;
                                solver = new GenAlgSolver(input, new GenAlgConfig(input));
                                savepath += "_GAFast";
                                break;
                            case Algorithms.ILP2Fast:
                                timelimit /= fastFactor;
                                goto case Algorithms.ILP2;
                            case Algorithms.ILP2:
                                solver = new IRuettae.Core.ILP2.Solver(input, 0.1, dataset.ToString());
                                savepath += "_ILP2";
                                break;
                            case Algorithms.ILPIP5GurobiFast:
                                timelimit /= fastFactor;
                                goto case Algorithms.ILPIP5Gurobi;
                            case Algorithms.ILPIP5Gurobi:
                                solver = new ILPIp5GurobiSolver(input, new ILPIp5GurobiConfig
                                {
                                    ClusteringMIPGap = 0,
                                    SchedulingMIPGap = 0,

                                    ClusteringTimeLimitMiliseconds = (long)(0.7 * timelimit),
                                    SchedulingTimeLimitMiliseconds = (long)(0.3 * timelimit),
                                    TimeSliceDuration = 120
                                });
                                savepath += "_ILPIp5Gurobi";
                                break;
                            case Algorithms.GoogleRoutingFast:
                                timelimit /= fastFactor;
                                solver = new GoogleRoutingSolver(input, GoogleRoutingConfig.GetDefault(input));
                                savepath += "_GoogleRoutingFast";
                                break;
                            case Algorithms.GoogleRouting:
                                solver = new GoogleRoutingSolver(input, GoogleRoutingConfig.GetDefault(input));
                                savepath += "_GoogleRouting";
                                break;
                        }

                        AddUnavailableBetweenDays(input);

                        OptimizationResult result = null;

                        void WriteConsoleInfo(object sender, string s)
                        {
                            Console.WriteLine($"Info ({DateTime.Now:HH-mm-ss}): {s}");
                        }

                        void WriteConsoleProgress(object sender, ProgressReport report)
                        {
                            Console.WriteLine($"Progress: {report}");
                        }
#if DEBUG
                        using (var sw = new StreamWriter(savepath + "-log.txt", true))
                        {
                            result = solver.Solve(timelimit, WriteConsoleProgress,
                                (sender, s) =>
                                {
                                    WriteConsoleInfo(sender, s);
                                    sw.WriteLine(s);
                                });
                        }
#else
                        result = solver.Solve(timelimit, WriteConsoleProgress, WriteConsoleInfo);
#endif

                        BigHr();

                        File.WriteAllText(savepath + ".json", JsonConvert.SerializeObject(result));

                        var summary = new StringBuilder();
                        summary.AppendLine($"Solver: {AlgorithmsDictionary[algorithmSelection]}");
                        summary.AppendLine($"Dataset{dataset}: {DatasetDictionary[dataset]}");
                        summary.AppendLine($"TimeElapsed [s]: {result.TimeElapsed}");
                        try
                        {
                            if (!result.IsValid())
                            {
                                summary.AppendLine(
                                    $"IMPORTANT: This result seems to be invalid. The reason is \"{result.Validate()}\"");
                            }
                        }
                        catch
                        {
                            summary.AppendLine("error while checking invalidity");
                        }

                        summary.AppendLine($"Cost: {result.Cost()}");
                        summary.AppendLine($"NumberOfNotVisitedFamilies: {result.NumberOfNotVisitedFamilies()}");
                        summary.AppendLine($"NumberOfMissingBreaks: {result.NumberOfMissingBreaks()}");
                        summary.AppendLine($"NumberOfAdditionalSantas: {result.NumberOfAdditionalSantas()}");
                        summary.AppendLine($"AdditionalSantaWorkTime: {result.AdditionalSantaWorkTime()}");
                        summary.AppendLine($"VisitTimeInUnavailable: {result.VisitTimeInUnavailable()}");
                        summary.AppendLine($"WayTimeOutsideBusinessHours: {result.WayTimeOutsideBusinessHours()}");
                        summary.AppendLine($"VisitTimeInDesired: {result.VisitTimeInDesired()}");
                        summary.AppendLine($"SantaWorkTime: {result.SantaWorkTime()}");
                        summary.AppendLine($"LongestDay: {result.LongestDay()}");
                        summary.AppendLine($"NumberOfRoutes: {result.NumberOfRoutes()}");

                        File.WriteAllText(savepath + ".txt", summary.ToString());
                        Console.WriteLine();
                        Console.WriteLine("Done solving");
                        Console.WriteLine(summary.ToString());
                        ResultDrawer.DrawResult(savepath, result, coordinates);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An exception occured: {e.Message}");
                    }
                }
            }
        }

        private static (OptimizationInput, (int, int)[] coordinates, int timelimit) GetDataset(int datasetSelection)
        {
            OptimizationInput input;
            (int, int)[] coordinates = null;
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
                    (input, coordinates) = DatasetFactory.DataSet7();
                    timelimit = 90 * 60 * 1000;
                    break;
                case 8:
                    (input, coordinates) = DatasetFactory.DataSet8();
                    timelimit = 90 * 60 * 1000;
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
                case 55:
                    (input, coordinates) = DatasetFactory.DataSet55Normal();
                    timelimit = 5 * 60 * 1000;
                    break;
                case 56:
                    (input, coordinates) = DatasetFactory.DataSet55Unavailable();
                    timelimit = 5 * 60 * 1000;
                    break;
                case 57:
                    (input, coordinates) = DatasetFactory.DataSet55Desired();
                    timelimit = 5 * 60 * 1000;
                    break;
            }
            return (input, coordinates, timelimit);
        }

        private static Algorithms QueryAlgorithmSelection()
        {
            Console.WriteLine("Please choose which algorithm to evaluate");
            foreach (var algorithm in AlgorithmsDictionary)
            {
                Console.WriteLine($"{(int)algorithm.Key}: {algorithm.Value}");
            }

            Algorithms algorithmSelection;
            do
            {
                Console.Write("Enter number: ");
                var enteredNumber = Console.ReadLine();

                if (!(Enum.TryParse<Algorithms>(enteredNumber, out algorithmSelection) &&
                      AlgorithmsDictionary.ContainsKey(algorithmSelection)))
                {
                    algorithmSelection = 0;
                    Console.WriteLine("Please enter a valid number");
                }
            } while (algorithmSelection == 0);

            Console.WriteLine($"You selected {AlgorithmsDictionary[algorithmSelection]}");
            return algorithmSelection;
        }

        private static IEnumerable<int> QueryDatasetSelection()
        {
            Console.WriteLine("Please select the dataset");
            foreach (var dataset in DatasetDictionary)
            {
                Console.WriteLine($"{dataset.Key}: {dataset.Value}");
            }

            const int invalidSelection = -1;
            var datasetSelection = invalidSelection;
            do
            {
                Console.Write("Enter number: ");
                var enteredNumber = Console.ReadLine();
                if (!(int.TryParse(enteredNumber, out datasetSelection) &&
                      DatasetDictionary.ContainsKey(datasetSelection)))
                {
                    datasetSelection = invalidSelection;
                    Console.WriteLine("Please enter a valid number");
                }
            } while (datasetSelection == invalidSelection);

            Console.WriteLine($"You selected dataset {datasetSelection}: {DatasetDictionary[datasetSelection]}");

            return GetDatasetSelection(datasetSelection);
        }

        private static IEnumerable<int> GetDatasetSelection(int datasetSelection)
        {
            var specialCases = new[] { 0, 55, 56, 57 };
            switch (datasetSelection)
            {
                case 0:
                    return DatasetDictionary.Keys.Where(k => !specialCases.Contains(k));
                case 55:
                case 56:
                case 57:
                    return new[] { 55, 56, 57 };
                default:
                    return new[] { datasetSelection };
            }

        }

        private static int QueryNumberOfRuns()
        {
            var runs = 1;
            do
            {
                Console.Write("Enter number of runs: ");
                var enteredNumber = Console.ReadLine();
                int.TryParse(enteredNumber, out runs);
                if (runs < 0)
                {
                    runs = 0;
                    Console.WriteLine("Please enter a positive number");
                }
            } while (runs == 0);

            Console.WriteLine($"Number of runs: {runs}");

            return runs;
        }

        private static void AddUnavailableBetweenDays(OptimizationInput input)
        {
            var orderedDays = input.Days.OrderBy(d => d.@from).ToList();
            var unavailabilities = new List<(int from, int to)>();
            (int from, int to) lastDay = orderedDays.First();

            // before first day
            unavailabilities.Add((int.MinValue, lastDay.@from - 1));

            // between days
            foreach (var day in orderedDays.Skip(1))
            {
                if (Math.Abs(day.@from - lastDay.to) > 1)
                {
                    unavailabilities.Add((lastDay.to + 1, day.@from - 1));
                }

                lastDay = day;
            }

            // after last day
            unavailabilities.Add((lastDay.to + 1, int.MaxValue));

            // add to visits
            for (int i = 0; i < input.Visits.Count(); i++)
            {
                var newUnavailable = new List<(int from, int to)>(input.Visits[i].Unavailable);
                newUnavailable.AddRange(unavailabilities);
                input.Visits[i].Unavailable = newUnavailable.ToArray();
            }
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
