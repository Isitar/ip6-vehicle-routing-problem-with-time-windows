using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.DatasetGenerator
{
    class DatasetGenerator
    {
        private readonly int mapWidth;
        private readonly int mapHeight;
        private readonly int numberOfVisits;
        private readonly int numberOfDays;
        private readonly int numberOfSantas;
        private readonly int[] numberOfDesired;
        private readonly int[] numberOfUnavailable;

        private readonly Random random = new Random();

        /// <summary>
        /// Generates a dataset with the given parameters
        /// </summary>
        /// <param name="mapWidth">x upper bound</param>
        /// <param name="mapHeight">y upper bound</param>
        /// <param name="numberOfVisits">how many visits total (excluding starting point)</param>
        /// <param name="numberOfDays">how many working days</param>
        /// <param name="numberOfSantas">how many santas for one day</param>
        /// <param name="numberOfDesired">array containing how many visits should have desired time for day [i]</param>
        /// <param name="numberOfUnavailable">array containing how many visits should have unavailable time for day [i]</param>
        public DatasetGenerator(int mapWidth, int mapHeight, int numberOfVisits, int numberOfDays, int numberOfSantas, int[] numberOfDesired,
            int[] numberOfUnavailable)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.numberOfVisits = numberOfVisits;
            this.numberOfDays = numberOfDays;
            this.numberOfSantas = numberOfSantas;
            this.numberOfDesired = numberOfDesired;
            this.numberOfUnavailable = numberOfUnavailable;
        }

        /// <summary>
        /// Generates the Dataset method
        /// </summary>
        /// <returns></returns>
        public string Generate(string name)
        {
            (int x, int y) clusterpoint1 = (mapWidth / 3, mapHeight / 3);
            (int x, int y) clusterpoint2 = (clusterpoint1.x * 2, clusterpoint1.y * 2);
            double clusterPointWidth = 0.9d * mapWidth / 3;
            double clusterPointHeight = 0.9d * mapHeight / 3;



            var coordinates = new (int x, int y)[numberOfVisits + 1];
            for (int i = 0; i < coordinates.Length; i++)
            {
                int rnd = random.Next(0, 100);

                (int x, int y) clusterpoint;
                double clusterWidth;
                double clusterHeight;
                if (rnd <= 45)
                {
                    clusterpoint = clusterpoint1;
                    clusterWidth = clusterPointWidth;
                    clusterHeight = clusterPointHeight;
                }
                else if (rnd <= 90)
                {
                    clusterpoint = clusterpoint2;
                    clusterWidth = clusterPointWidth;
                    clusterHeight = clusterPointHeight;
                }
                else
                {
                    clusterpoint = (mapWidth / 2, mapHeight / 2);
                    clusterWidth = mapWidth / 2d * 0.9;
                    clusterHeight = mapHeight / 2d * 0.9;
                }

                var x = -1;
                while (x < 0 || x > mapWidth)
                {
                    x = (int)GaussianRandomGenerator.RandomGauss(clusterpoint.x, clusterWidth / 3);
                }

                var y = -1;
                while (y < 0 || y > mapHeight)
                {
                    y = (int)GaussianRandomGenerator.RandomGauss(clusterpoint.y, clusterHeight / 3);
                }

                coordinates[i] = (x, y);
            }




            var sb = new StringBuilder();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"/// {numberOfVisits} Visits, {numberOfDays} Days, {numberOfSantas} Santas");
            for (int i = 0; i < numberOfDays; i++)
            {
                sb.AppendLine($"/// {numberOfDesired[i]} Desired, {numberOfUnavailable[i]} Unavailable on day {i}");
            }

            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public static (OptimizationInput input, (int x, int y)[] coordinates) {name}()");
            sb.AppendLine("{");

            sb.AppendLine("\t// Coordinates of points");
            sb.AppendLine("\tvar coordinates = new[]");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\t{string.Join($",{Environment.NewLine}\t\t", coordinates.Select(c => $"({c.x},{c.y})"))}");
            sb.AppendLine("\t};");
            sb.AppendLine("\tconst int workingDayDuration = 12 * Hour;");
            sb.AppendLine("\tvar input = new OptimizationInput");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tDays = new[] {{{string.Join(", ", Enumerable.Range(0, numberOfDays).Select(x => $"({x * 24}* Hour, {x * 24}*Hour + workingDayDuration)"))} }},");
            sb.AppendLine();
            sb.AppendLine("\t\tRouteCosts = new[,]");
            sb.AppendLine("\t\t{");
            sb.AppendLine(string.Join($",{Environment.NewLine}",
                Enumerable.Range(1, numberOfVisits).Select(i =>
                {
                    var i1 = i;
                    return $"\t\t\t{{ {string.Join(", ", Enumerable.Range(1, numberOfVisits).Select(j => Distance(coordinates[i1], coordinates[j])))} }}";
                })
            ));
            sb.AppendLine("\t\t},");
            sb.AppendLine();
            sb.AppendLine("\t\tSantas = new[]");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\t{string.Join($",{Environment.NewLine}\t\t\t", Enumerable.Range(0, numberOfSantas).Select(s => $"new Santa {{ Id = {s} }}"))}");
            sb.AppendLine("\t\t},");
            sb.AppendLine();
            sb.AppendLine("\t\tVisits = new[]");
            sb.AppendLine("\t\t{");
            sb.AppendLine(string.Join($",{Environment.NewLine}",
                Enumerable.Range(0, numberOfVisits).Select(v =>
                {
                    string desiredString = "new (int from, int to)[0]";
                    int desiredDayIndex = -1;
                    if (numberOfDesired.Sum() > 0)
                    {
                        desiredDayIndex = 0;
                        while (numberOfDesired[desiredDayIndex] == 0)
                        {
                            desiredDayIndex++;
                        }

                        numberOfDesired[desiredDayIndex]--;
                        desiredString = $"new [] {{({desiredDayIndex * 24} * Hour,  {desiredDayIndex * 24} * Hour + workingDayDuration)}}";
                    }

                    var unavailableString = "new (int from, int to)[0]";
                    if (numberOfUnavailable.Sum() > 0)
                    {
                        int unavailableDayIndex = 0;
                        while (unavailableDayIndex < numberOfUnavailable.Length && (numberOfUnavailable[unavailableDayIndex] == 0 || unavailableDayIndex == desiredDayIndex))
                        {
                            unavailableDayIndex++;
                        }

                        if (unavailableDayIndex < numberOfUnavailable.Length)
                        {
                            numberOfUnavailable[unavailableDayIndex]--;
                            unavailableString = $"new [] {{({unavailableDayIndex * 24} * Hour,  {unavailableDayIndex * 24} * Hour + workingDayDuration)}}";
                        }
                    }

                    return $"\t\t\tnew Visit{{Duration = {random.Next(1200, 3600)}, Id={v},WayCostFromHome={Distance(coordinates[0], coordinates[v + 1])}, WayCostToHome={Distance(coordinates[0], coordinates[v + 1])},Unavailable ={unavailableString},Desired = {desiredString}}}";
                })));
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t};");
            sb.AppendLine("\treturn (input, coordinates);");
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// Returns pythagoras distance between two points as int
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public int Distance((int x, int y) p1, (int x, int y) p2)
        {
            return (int)Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
        }
    }
}
