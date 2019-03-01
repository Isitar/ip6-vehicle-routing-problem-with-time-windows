using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Tuning.ILP2Tuning
{
    class Program
    {
        static void Main(string[] args)
        {
            TuneSecond();

            Console.ReadLine();
        }


        static void TuneZeroToHundred()
        {
            var dataSets = new DataSets.OptimizationDataSets().TwentyDataSets.Take(15).ToArray();

            const int numValuesPerRow = 11;
            const int numRuns = 1;
            var table = new int[numRuns][];
            var allCosts = new int[numRuns][][];
            const int timeLimit = 360 * 1000;

            const double stepSize = 0.1;


            {
                var calcDuration = ((numValuesPerRow) * dataSets.Length * (timeLimit / 1000) * numRuns);
                Console.WriteLine($"calculated duration: {calcDuration}s , {calcDuration / 60} min, {calcDuration / 3600d} h");
            }

            for (int run = 0; run < numRuns; run++)
            {
                table[run] = new int[numValuesPerRow];
                allCosts[run] = new int[numValuesPerRow][];
                for (int vrpTimeLimitFactor = 0; vrpTimeLimitFactor < numValuesPerRow; vrpTimeLimitFactor++)
                {

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"tuning param: {vrpTimeLimitFactor}");
                    Console.ForegroundColor = ConsoleColor.White;
                    allCosts[run][vrpTimeLimitFactor] = new int[dataSets.Length];
                    var cumCost = 0;

                    var i = 0;
                    foreach (var dataSet in dataSets)
                    {
                        // ReSharper disable PossibleLossOfFraction
                        var solver = new IRuettae.Core.ILP2.Solver(dataSet, vrpTimeLimitFactor * stepSize);
                        allCosts[run][vrpTimeLimitFactor][i] = solver.Solve(timeLimit, null, null).Cost();

                        cumCost += allCosts[run][vrpTimeLimitFactor][i];
                        i++;
                    }


                    table[run][vrpTimeLimitFactor] = cumCost;
                }


                // write result in csv file
                using (var sw = new StreamWriter($"{DateTime.Now:yy-MM-dd-HH-mm-ss}{run}.csv"))
                {
                    sw.WriteLine(string.Join(";", table[run]));
                    Console.WriteLine(string.Join(";", table[run]));
                }

                // write full result in csv file
                using (var sw = new StreamWriter($"{DateTime.Now:yy-MM-dd-HH-mm-ss}full_{run}.csv"))
                {
                    var results = new string[allCosts[run].Length];
                    for (int x = 0; x < allCosts[run].Length; x++)
                    {
                        if (allCosts[run][x] != null)
                        {
                            var cellResult = new int[allCosts[run][x].Length];
                            for (int res = 0; res < allCosts[run][x].Length; res++)
                            {
                                cellResult[res] = allCosts[run][x][res];
                            }

                            results[x] = string.Join(",", cellResult);
                        }
                        else
                        {
                            results[x] = "";
                        }
                    }

                    sw.WriteLine(string.Join(";", results));
                    Console.WriteLine(string.Join(";", results));

                }
            }
        }

        static void TuneSecond()
        {
            var dataSets = new DataSets.OptimizationDataSets().TwentyDataSets.Take(15).ToArray();

            const int numValues = 4;
            const int numRuns = 1;
            var table = new int[numRuns][];
            var allCosts = new int[numRuns][][];
            const int timeLimit = 360 * 1000;
            
            {
                var calcDuration = ((numValues) * dataSets.Length * (timeLimit / 1000) * numRuns);
                Console.WriteLine($"calculated duration: {calcDuration}s , {calcDuration / 60} min, {calcDuration / 3600d} h");
            }

            for (int run = 0; run < numRuns; run++)
            {
                table[run] = new int[numValues];
                allCosts[run] = new int[numValues][];
                for (int vrpTimeLimitFactor = 0; vrpTimeLimitFactor < numValues; vrpTimeLimitFactor++)
                {

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"tuning param: {vrpTimeLimitFactor}");
                    Console.ForegroundColor = ConsoleColor.White;
                    allCosts[run][vrpTimeLimitFactor] = new int[dataSets.Length];
                    var cumCost = 0;

                    var i = 0;
                    foreach (var dataSet in dataSets)
                    {
                        double factor = 0;
                        switch (vrpTimeLimitFactor)
                        {
                            case 0:
                                factor = 0;
                                break;
                            case 1:
                                factor = 0.05;
                                break;
                            case 2:
                                factor = 0.10;
                                break;
                            case 3:
                                factor = 0.15;
                                break;
                        }
                        // ReSharper disable PossibleLossOfFraction
                        var solver = new IRuettae.Core.ILP2.Solver(dataSet, factor);
                        allCosts[run][vrpTimeLimitFactor][i] = solver.Solve(timeLimit, null, null).Cost();

                        cumCost += allCosts[run][vrpTimeLimitFactor][i];
                        i++;
                    }


                    table[run][vrpTimeLimitFactor] = cumCost;
                }


                // write result in csv file
                using (var sw = new StreamWriter($"{DateTime.Now:yy-MM-dd-HH-mm-ss}{run}.csv"))
                {
                    sw.WriteLine(string.Join(";", table[run]));
                    Console.WriteLine(string.Join(";", table[run]));
                }

                // write full result in csv file
                using (var sw = new StreamWriter($"{DateTime.Now:yy-MM-dd-HH-mm-ss}full_{run}.csv"))
                {
                    var results = new string[allCosts[run].Length];
                    for (int x = 0; x < allCosts[run].Length; x++)
                    {
                        if (allCosts[run][x] != null)
                        {
                            var cellResult = new int[allCosts[run][x].Length];
                            for (int res = 0; res < allCosts[run][x].Length; res++)
                            {
                                cellResult[res] = allCosts[run][x][res];
                            }

                            results[x] = string.Join(",", cellResult);
                        }
                        else
                        {
                            results[x] = "";
                        }
                    }

                    sw.WriteLine(string.Join(";", results));
                    Console.WriteLine(string.Join(";", results));

                }
            }
        }
    }
}
