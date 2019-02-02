using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Tuning.LocalSolverGridSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dataSets = new DataSets.OptimizationDataSets().TwentyDataSets.Skip(5).Take(10).ToArray();
            var dataSets = new DataSets.OptimizationDataSets().DataSetsFifteen.Take(10).ToArray();

            const int numValuesPerRow = 11;
            const int numRuns = 2;
            var grid = new int[numRuns][,];
            var allCosts = new int[numRuns][,][];

            var gridFactorX = Math.Ceiling(100d / numValuesPerRow);
            var gridFactorY = Math.Ceiling(100d / numValuesPerRow);
            const int timeLimit = 60000;
            {
                var calcDuration = ((numValuesPerRow * (numValuesPerRow + 1)) / 2) * dataSets.Length * (timeLimit / 1000) * numRuns;
                Console.WriteLine($"calculated duration: {calcDuration}s , {calcDuration / 60} min, {calcDuration / 3600d} h");
            }
            for (int run = 0; run < numRuns; run++)
            {
                grid[run] = new int[numValuesPerRow, numValuesPerRow];
                allCosts[run] = new int[numValuesPerRow, numValuesPerRow][];
                for (int vrpTimeLimitFactor = 0; vrpTimeLimitFactor < numValuesPerRow; vrpTimeLimitFactor++)
                {
                    for (int vrptwTimeLimitFactor = 0; vrptwTimeLimitFactor < numValuesPerRow - vrpTimeLimitFactor; vrptwTimeLimitFactor++)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"tuning param: {vrpTimeLimitFactor}, {vrptwTimeLimitFactor}");
                        Console.ForegroundColor = ConsoleColor.White;
                        allCosts[run][vrpTimeLimitFactor, vrptwTimeLimitFactor] = new int[dataSets.Length];
                        var cumCost = 0;
                        var i = 0;
                        foreach (var dataSet in dataSets)
                        {
                            // ReSharper disable PossibleLossOfFraction
                            var solver = new IRuettae.Core.LocalSolver.Solver(dataSet, vrpTimeLimitFactor / gridFactorX, vrptwTimeLimitFactor / gridFactorY);
                            allCosts[run][vrpTimeLimitFactor, vrptwTimeLimitFactor][i] = solver.Solve(timeLimit, null, null).Cost();
                            
                            cumCost += allCosts[run][vrpTimeLimitFactor, vrptwTimeLimitFactor][i];
                            i++;
                        }


                        grid[run][vrpTimeLimitFactor, vrptwTimeLimitFactor] = cumCost;
                    }
                }


                // write result in csv file
                using (var sw = new StreamWriter($"grid{run}.csv"))
                {
                    for (int y = 0; y < grid[run].GetLength(1); y++)
                    {
                        var results = new int[grid[run].GetLength(0)];
                        for (int x = 0; x < grid[run].GetLength(0); x++)
                        {
                            results[x] = grid[run][x, y];
                        }
                        sw.WriteLine(string.Join(";", results));
                        Console.WriteLine(string.Join(";", results));
                    }
                }

                // write full result in csv file
                using (var sw = new StreamWriter($"full_{run}.csv"))
                {
                    for (int y = 0; y < allCosts[run].GetLength(1); y++)
                    {
                        var results = new string[allCosts[run].GetLength(0)];
                        for (int x = 0; x < allCosts[run].GetLength(0); x++)
                        {
                            if (allCosts[run][x, y] != null)
                            {
                                var cellResult = new int[allCosts[run][x, y].Length];
                                for (int res = 0; res < allCosts[run][x, y].Length; res++)
                                {
                                    cellResult[res] = allCosts[run][x, y][res];
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



            Console.ReadLine();
        }
    }
}
