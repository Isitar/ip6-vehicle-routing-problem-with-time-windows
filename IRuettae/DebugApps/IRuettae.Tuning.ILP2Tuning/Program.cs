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

            var dataSets = new DataSets.OptimizationDataSets().DataSetsFifteen.Take(10).ToArray();

            const int numValuesPerRow = 11;
            const int numRuns = 2;
            var table = new int[numRuns][];
            var allCosts = new int[numRuns][][];
            const int timeLimit = 75000;

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
                        var solver = new IRuettae.Core.ILP2.Solver(dataSet, vrpTimeLimitFactor / (numValuesPerRow - 1));
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

            Console.ReadLine();
        }
    }
}
