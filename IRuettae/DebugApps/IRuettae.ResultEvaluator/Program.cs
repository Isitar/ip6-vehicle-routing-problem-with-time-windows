using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IRuettae.ResultEvaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            BigHr();
            Console.WriteLine("Program written to gather all results of Evaluator in a folder and combine a structured excel");
            Console.WriteLine();
            SmallHr();
            string path;
            if (args.Length == 0)
            {
                Console.Write("Path: ");
                path = Console.ReadLine();
            }
            else
            {
                path = args[0];
            }

            string savePath;
            if (args.Length < 2)
            {
                Console.Write("Save file? (empty = no): ");
                savePath = Console.ReadLine();
            }
            else
            {
                savePath = args[1];
            }

            var resultDict = new Dictionary<string, Dictionary<string, List<int>>>();

            var results = new List<Result>();

            foreach (var file in Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories))
            {
                try
                {
                    if (!file.Contains("_DataSet_"))
                    {
                        continue;
                    }

                    Console.WriteLine($"Processing {file}");


                    var regex = new Regex(
                        "Solver\\d*: (?<solver>[\\w+\\s]*)\\nDataset(?<dataset>\\d+):(.|\\n)*Cost: (?<cost>\\d*)",
                        RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    var fileContent = File.ReadAllText(file);
                    var match = regex.Match(fileContent);
                    if (!match.Success)
                    {
                        Console.Error.WriteLine($"Error processing {file}");
                        continue;
                    }
                    var solver = match.Groups["solver"].Value;
                    var dataSet = match.Groups["dataset"].Value;
                    var cost = Convert.ToInt32(match.Groups["cost"].Value);
                    if (!resultDict.ContainsKey(solver))
                    {
                        resultDict.Add(solver, new Dictionary<string, List<int>>());
                    }

                    if (!resultDict[solver].ContainsKey(dataSet))
                    {
                        resultDict[solver].Add(dataSet, new List<int>());
                    }

                    resultDict[solver][dataSet].Add(cost);
                    results.Add(new Result { Cost = cost, DataSet = dataSet, Solver = solver });
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }
            BigHr();

            var sb = new StringBuilder();

            foreach (var solver in resultDict.Keys.OrderBy(k => k))
            {
                sb.AppendLine(solver);
                sb.AppendLine("DataSet;\tAvg;\tMin;\tMax;\t25-percentile;");
                foreach (var dataSet in resultDict[solver].Keys.OrderBy(int.Parse))
                {
                    var resultList = resultDict[solver][dataSet];
                    //Console.WriteLine($"{dataSet}: [{string.Join(",", resultList)}], avg: {resultList.Average()}, min: {resultList.Min()}, max: {resultList.Max()}, 25%percentile: {resultList.OrderBy(r => -r).Take((int)Math.Ceiling(resultList.Count * 0.25)).Average()}");
                    sb.AppendLine($"{dataSet};\t{resultList.Average():F1};\t{resultList.Min():F1};\t{resultList.Max():F1};\t{resultList.OrderBy(r => -r).Take((int)Math.Ceiling(resultList.Count * 0.25)).Average():F1}");
                }

                sb.AppendLine(new string('-', 40));
            }

            if (!string.IsNullOrWhiteSpace(savePath))
            {
                File.WriteAllText(savePath,sb.ToString());
            }
            Console.WriteLine(sb.ToString());

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
