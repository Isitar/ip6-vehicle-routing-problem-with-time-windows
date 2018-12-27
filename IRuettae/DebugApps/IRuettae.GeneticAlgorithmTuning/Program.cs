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
        private const int PopulationSize = 10;

        static void Main(string[] args)
        {
            var pso = new ParticleSwarmOptimization(RunGeneticAlgorithm, ScaleParameters, new string[] { "elitismPercentage", "directMutationPercentage", "randomPercentage", "orderBasedCrossoverProbability", "mutationProbability", "positionMutationProbability", "recombinationProbability" });
            pso.Run();
            Console.ReadLine();
        }

        static double RunGeneticAlgorithm(double[] parameters)
        {
            var (input, _) = DatasetFactory.DatasetGATuning();

            var starterData = new GenAlgStarterData(input.NumberOfSantas(), long.MaxValue, PopulationSize, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);

            var timeLimitMilliseconds = 250;

            var solver = new GenAlgSolver(input, starterData);

            var numberOfRuns = 10;
            // run numberOfRuns times and return average
            return Enumerable.Range(0, numberOfRuns).Select(v => solver.Solve(timeLimitMilliseconds, null, null).Cost()).Average();
        }

        static void ScaleParameters(double[] parameters)
        {
            // Percentages
            {
                var minParameter = new[] {
                    parameters[0],
                    parameters[1],
                    parameters[2],
                    parameters[6],
                }.Min();

                if (minParameter < 0)
                {
                    // shift to that minimal percentage = 0
                    parameters[0] -= minParameter;
                    parameters[1] -= minParameter;
                    parameters[2] -= minParameter;
                    parameters[6] -= minParameter;
                }

                var elitismPercentage = parameters[0];
                var directMutationPercentage = parameters[1];
                var randomPercentage = parameters[2];
                var recombinationPercentage = parameters[6];
                var sum = (elitismPercentage + directMutationPercentage + randomPercentage + recombinationPercentage);

                if (sum != 0)
                {
                    // scale so that sum=1

                    var factor = 1 / sum;

                    parameters[0] = parameters[0] * factor;
                    parameters[1] = parameters[1] * factor;
                    parameters[2] = parameters[2] * factor;
                    parameters[6] = parameters[6] * factor;
                }
                else
                {
                    // set everything to 25% as somehow all values got zero
                    parameters[0] = 0.25;
                    parameters[1] = 0.25;
                    parameters[2] = 0.25;
                    parameters[6] = 0.25;
                }

                if (parameters[0] * PopulationSize < 1)
                {
                    // elitsm percentage needs to be high enough
                    // so that p*PopulationSize > 0
                    parameters[0] += 1d / PopulationSize;
                    ScaleParameters(parameters);
                }
            }

            // each must be between 0 and 1
            parameters[3] = Math.Min(1, Math.Max(0, parameters[3]));
            parameters[4] = Math.Min(1, Math.Max(0, parameters[4]));
            parameters[5] = Math.Min(1, Math.Max(0, parameters[5]));
        }
    }
}
