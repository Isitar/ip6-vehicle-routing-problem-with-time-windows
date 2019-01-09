using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.GeneticAlgorithmTuning
{
    class Program
    {
        // settings
        const int NumberOfRuns = 3;

        static void Main(string[] args)
        {
            var pso = new ParticleSwarmOptimization(RunGeneticAlgorithm, CreateVariables, BoundVariables, CreateVelocity, BoundVelocity, new string[] { "ElitismPercentage", "DirectMutationPercentage", "RandomPercentage", "OrderBasedCrossoverProbability", "mutationProbability", "PositionMutationProbability", "PopulationSize" });
            string startTime = $"{DateTime.Now:yy-MM-dd-HH-mm-ss}";
            for (int i = 0; i < NumberOfRuns; i++)
            {
                pso.Run();
                System.IO.File.Move("pso-log.txt", $"{startTime}_pso-log{i}.txt");
            }
            Console.ReadLine();
        }

        // design variable settings
        const int NumberOfVars = 7;
        const int MaxPopulationSize = 5000;
        const int MinPopulationSize = 2;
        const double MinPercentage = 0.0;
        const double MaxPercentage = 1.0;

        // b_lo
        private static readonly double[] min = new double[NumberOfVars]
        {
            MinPercentage,
            MinPercentage,
            MinPercentage,
            MinPercentage,
            MinPercentage,
            MinPercentage,
            MinPopulationSize,
        };
        // b_up
        private static readonly double[] max = new double[NumberOfVars]
        {
            MaxPercentage,
            MaxPercentage,
            MaxPercentage,
            MaxPercentage,
            MaxPercentage,
            MaxPercentage,
            MaxPopulationSize,
        };

        /// <summary>
        /// Can be called concurrent.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static double RunGeneticAlgorithm(double[] parameters)
        {
            // settings
            var timeLimitMilliseconds = 500;
            var numberOfRuns = 10;
            var maxNumberOfGenerations = long.MaxValue;

            // create solver
            var (input, _) = DatasetFactory.DatasetGATuning();
            var starterData = new GenAlgStarterData(input.NumberOfSantas(), maxNumberOfGenerations, (int)parameters[6], parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
            var solver = new GenAlgSolver(input, starterData);

            // run numberOfRuns times and return average
            return Enumerable.Range(0, numberOfRuns)
                .Select(v => solver.Solve(timeLimitMilliseconds, null, null).Cost())
                .Average();
        }

        /// <summary>
        /// Returns a uniform distributed random set of valid variables.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        static double[] CreateVariables(Random random)
        {
            var variables = new double[NumberOfVars];
            for (int i = 0; i < variables.Length; i++)
            {
                variables[i] = min[i] + (max[i] - min[i]) * random.NextDouble();
            }
            BoundVariables(variables);
            return variables;
        }

        /// <summary>
        /// Bounds the given variables so that they are valid.
        /// </summary>
        /// <param name="variables"></param>
        static void BoundVariables(double[] variables)
        {
            // enforce absolute bounds
            for (int i = 0; i < variables.Length; i++)
            {
                variables[i] = Math.Min(max[i], Math.Max(min[i], variables[i]));
            }

            // fix wrong percentages
            {
                var elitismPercentage = variables[0];
                var directMutationPercentage = variables[1];
                var randomPercentage = variables[2];
                var sum = elitismPercentage + directMutationPercentage + randomPercentage;

                if (sum > 1)
                {
                    // scale so that sum=1
                    var factor = 1 / sum;

                    variables[0] = variables[0] * factor;
                    variables[1] = variables[1] * factor;
                    variables[2] = variables[2] * factor;
                }
            }

            // elitism percentage must lead to at least 1 element
            var elitismElements = (int)(variables[0] * (int)variables[6]);
            if (elitismElements < 1)
            {
                variables[0] = Math.Max(1.1 / (int)variables[6], variables[0]);
                // recursive call to make sure the population doesn't grow
                BoundVariables(variables);
            }
        }

        /// <summary>
        /// Returns a uniform distributed random set of valid velocities.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        static double[] CreateVelocity(Random random)
        {
            var velocity = new double[NumberOfVars];
            for (int i = 0; i < velocity.Length; i++)
            {
                var difference = Math.Abs(max[i] - min[i]);
                var negDifference = -difference;
                velocity[i] = negDifference + (difference - negDifference) * random.NextDouble();
            }
            // call to BoundVelocity not needed
            return velocity;
        }

        /// <summary>
        /// Bounds the given velocities so that they are valid.
        /// </summary>
        /// <param name="variables"></param>
        static void BoundVelocity(double[] velocity)
        {
            for (int i = 0; i < velocity.Length; i++)
            {
                var difference = Math.Abs(max[i] - min[i]);
                var negDifference = -difference;
                velocity[i] = Math.Min(difference, Math.Max(negDifference, velocity[i]));
            }
        }
    }
}

