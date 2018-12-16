using System;
using System.IO;

namespace IRuettae.DatasetGenerator
{
    class Program
    {
        static void GenerateAllDatasets()
        {
            File.WriteAllText("DatasetFactory.DataSet1.cs", new DatasetGenerator(4000, 4000, 10, 2, 1, new[] { 0, 0 }, new[] { 0, 0 }, -1).Generate("DataSet1"));
            File.WriteAllText("DatasetFactory.DataSet2.cs", new DatasetGenerator(4000, 4000, 10, 2, 1, new[] { 5, 5 }, new[] { 0, 0 }, -1).Generate("DataSet2"));
            File.WriteAllText("DatasetFactory.DataSet3.cs", new DatasetGenerator(4000, 4000, 10, 2, 1, new[] { 0, 0 }, new[] { 5, 5 }, -1).Generate("DataSet3"));
            File.WriteAllText("DatasetFactory.DataSet4.cs", new DatasetGenerator(4000, 4000, 20, 2, 2, new[] { 0, 0 }, new[] { 0, 0 }, -1).Generate("DataSet4"));
            File.WriteAllText("DatasetFactory.DataSet5.cs", new DatasetGenerator(4000, 4000, 20, 2, 2, new[] { 10, 10 }, new[] { 0, 0 }, -1).Generate("DataSet5"));
            File.WriteAllText("DatasetFactory.DataSet6.cs", new DatasetGenerator(4000, 4000, 20, 2, 2, new[] { 0, 0 }, new[] { 10, 10 }, -1).Generate("DataSet6"));
            File.WriteAllText("DatasetFactory.DataSet9.cs", new DatasetGenerator(4000, 4000, 50, 2, 5, new[] { 15, 15 }, new[] { 11, 11 }, -1).Generate("DataSet9"));
            File.WriteAllText("DatasetFactory.DataSet10.cs", new DatasetGenerator(4000, 4000, 100, 2, 10, new[] { 35, 35 }, new[] { 20, 20 }, -1).Generate("DataSet10"));
            File.WriteAllText("DatasetFactory.DataSet11.cs", new DatasetGenerator(4000, 4000, 200, 2, 20, new[] { 75, 75 }, new[] { 40, 40 }, -1).Generate("DataSet11"));
            File.WriteAllText("DatasetFactory.DataSet12.cs", new DatasetGenerator(4000, 4000, 1000, 2, 100, new[] { 300, 300 }, new[] { 150, 150 }, -1).Generate("DataSet12"));

        }

        static void Main(string[] args)
        {
            GenerateAllDatasets();
            BigHr();
            Console.WriteLine("Program written to generate code for different datasets.");
            Console.WriteLine();

            Console.WriteLine("Please input only correct values, no exception handling here!");
            SmallHr();

            Console.Write("Map-Width [number]: ");
            int width = int.Parse(Console.ReadLine());
            Console.Write("Map-Height [number]: ");
            int height = int.Parse(Console.ReadLine());
            Console.Write("Visits [number]: ");
            int numberOfVisits = int.Parse(Console.ReadLine());
            Console.Write("Days [number]: ");
            int numberOfDays = int.Parse(Console.ReadLine());
            Console.Write("Santas [number]: ");
            int numberOfSantas = int.Parse(Console.ReadLine());
            var desired = new int[numberOfDays];
            for (int i = 0; i < numberOfDays; i++)
            {
                Console.Write($"Number of visits desired on day {i} [number]: ");
                desired[i] = int.Parse(Console.ReadLine());
            }
            var unavailable = new int[numberOfDays];
            for (int i = 0; i < numberOfDays; i++)
            {
                Console.Write($"Number of visits unavailable on day {i} [number]: ");
                unavailable[i] = int.Parse(Console.ReadLine());
            }

            Console.Write("Fixed working day duration (empty = gets calculated): ");
            var workingDayDurationInput = Console.ReadLine();
            int workingDayDuration = string.IsNullOrWhiteSpace(workingDayDurationInput)
                ? -1
                : int.Parse(workingDayDurationInput);

            Console.Write("Generate breaks (empty = false): ");
            var generateBreaksInput = Console.ReadLine();
            bool generateBreaks = !string.IsNullOrWhiteSpace(generateBreaksInput) && bool.Parse(generateBreaksInput);

            Console.Write("Method name [text]: ");
            var methodName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Calculating...");
            BigHr();
            var output =
                new DatasetGenerator(width, height, numberOfVisits, numberOfDays, numberOfSantas, desired, unavailable, workingDayDuration, generateBreaks)
                    .Generate(methodName);

            //Console.WriteLine(output);
            BigHr();

            File.WriteAllText($"DatasetFactory.{methodName}.cs", output);

            Console.WriteLine("Press any key to exit...");
            Console.Read();
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
