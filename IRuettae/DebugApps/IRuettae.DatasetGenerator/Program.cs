using System;
using System.IO;

namespace IRuettae.DatasetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
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
            Console.Write("Method name [text]: ");
            var methodName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Calculating...");
            BigHr();
            var output =
                new DatasetGenerator(width, height, numberOfVisits, numberOfDays, numberOfSantas, desired, unavailable)
                    .Generate(methodName);

            //Console.WriteLine(output);
            BigHr();
            Console.WriteLine("Save? [y/n]");
            if (Console.ReadLine().ToUpper().Equals("Y"))
            {
                File.WriteAllText($"{methodName}.cs", output);
            }
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
