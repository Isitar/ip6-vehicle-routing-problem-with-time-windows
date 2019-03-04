using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using IRuettae.Evaluator;
using Newtonsoft.Json;

namespace IRuettae.ResultFixer
{
    class Program
    {


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
        };

        static void Main(string[] args)
        {

            var datasets = new (int x, int y)[100][];

            datasets[0] = DatasetFactory.DataSet1().coordinates;
            datasets[1] = DatasetFactory.DataSet2().coordinates;
            datasets[2] = DatasetFactory.DataSet3().coordinates;
            datasets[3] = DatasetFactory.DataSet4().coordinates;
            datasets[4] = DatasetFactory.DataSet5().coordinates;
            datasets[5] = DatasetFactory.DataSet6().coordinates;
            datasets[6] = DatasetFactory.DataSet7().coordinates;
            datasets[7] = DatasetFactory.DataSet8().coordinates;
            datasets[8] = DatasetFactory.DataSet9().coordinates;
            datasets[9] = DatasetFactory.DataSet10().coordinates;
            datasets[10] = DatasetFactory.DataSet11().coordinates;
            datasets[11] = DatasetFactory.DataSet12().coordinates;

            datasets[54] = DatasetFactory.DataSet55Normal().coordinates;
            datasets[55] = DatasetFactory.DataSet55Unavailable().coordinates;
            datasets[56] = DatasetFactory.DataSet55Desired().coordinates;

            foreach (var file in Directory.GetFiles(@"C:\git\ip6-vehicle-routing-problem-with-time-windows\Results\Unavailable_Desired_impact", "*_56_*.json", SearchOption.AllDirectories))
            {
                var dataset = int.Parse(Path.GetFileName(file).Split('_')[2]);
                var result = JsonConvert.DeserializeObject<OptimizationResult>(File.ReadAllText(file));
                ResultDrawer.DrawResult(Path.GetFileNameWithoutExtension(file) + ".gif", result, datasets[dataset - 1]);
            }
        }

        static void FixResult(string filename, string solver, int dataset)
        {
            var result = JsonConvert.DeserializeObject<OptimizationResult>(File.ReadAllText(filename));

            foreach (var route in result.Routes)
            {
                var (dayStart, _) = result.FindDay(route);
                if (route.Waypoints[0].StartTime < dayStart)
                {
                    route.Waypoints[0].StartTime = dayStart;
                }
            }
            File.WriteAllText(Path.GetFileNameWithoutExtension(filename) + ".json", JsonConvert.SerializeObject(result));

            var summary = new StringBuilder();
            summary.AppendLine($"Solver: {solver}");
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

            File.WriteAllText(Path.GetFileNameWithoutExtension(filename) + ".txt", summary.ToString());
        }
    }
}
