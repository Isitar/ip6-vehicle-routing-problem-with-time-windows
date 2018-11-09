using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        static readonly Dictionary<int, string> algorithmsDictionary = new Dictionary<int, string>()
        {
            {1,"ILP"},
            {2, "GA" },
            {3, "LocalSolver" },
        };

        static readonly Dictionary<int, string> datasetDictionary = new Dictionary<int, string>()
        {
            {1, "10 visits, 1 santa 2 days"},
            {2, "10 visits, 1 santas, 2 days 5 desired d1, 5 desired d2" },
            {3, "10 visits, 1 santas, 2 days 5 unavailable d1, 5 unavailable d2" },
            {4, "20 visits, 2 santas"},
            {5, "20 visits, 2 santas, 2 days, 10 desired d1, 10 desired d2" },
            {6, "20 visits, 2 santas, 2 days, 10 unavailable d1, 10 unavailable d2" },
        };

        static void Main(string[] args)
        {
            BigHr();
            Console.WriteLine("Program written to evaluate the different optimisation algorithms.");
            Console.WriteLine();

            Console.WriteLine("Pleace choose which algorithm to evaluate");
            foreach (var algorithm in algorithmsDictionary)
            {
                Console.WriteLine($"{algorithm.Key}: {algorithm.Value}");
            }
            int algorithmSelection = 0;

            while (algorithmSelection == 0)
            {
                Console.Write("Enter number: ");
                var enteredNumber = Console.ReadLine();
                if (!(int.TryParse(enteredNumber, out algorithmSelection) && algorithmsDictionary.ContainsKey(algorithmSelection)))
                {
                    algorithmSelection = 0;
                    Console.WriteLine("Please enter a valid number");
                }
            }
            Console.WriteLine($"You selected {algorithmsDictionary[algorithmSelection]}");
            SmallHr();
            Console.WriteLine();
            Console.WriteLine("Please select the dataset");
            foreach (var dataset in datasetDictionary)
            {
                Console.WriteLine($"{dataset.Key}: {dataset.Value}");
            }
            int datasetSelection = 0;

            while (datasetSelection == 0)
            {
                Console.Write("Enter number: ");
                var enteredNumber = Console.ReadLine();
                if (!(int.TryParse(enteredNumber, out datasetSelection) && datasetDictionary.ContainsKey(datasetSelection)))
                {
                    datasetSelection = 0;
                    Console.WriteLine("Please enter a valid number");
                }
            }

            Console.WriteLine($"You selected dataset {datasetSelection}: {datasetDictionary[datasetSelection]}");
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
                    savepath += "_Dataset_1";
                    break;
            }

            ISolver solver = null;
            switch (algorithmSelection)
            {
                case 1:
                    solver = new ILPSolver(input, new ILPStarterData
                    {
                        ClusteringMIPGap = 0,
                        SchedulingMIPGap = 2,

                        ClusteringTimeLimit = (long)(0.7 * timelimit),
                        SchedulingTimeLimit = (long)(0.3 * timelimit),
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

            Console.ReadLine();
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
