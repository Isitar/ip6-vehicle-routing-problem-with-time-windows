using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    public class GenAlgStarterData
    {
        public int PopulationSize { get; set; } = 10;
        public int MaxNumberOfSantas { get; set; }
        public long MaxNumberOfGenerations { get; set; } = long.MaxValue;
        public double MutationProbability { get; set; } = 0.1;
        public double OrderBasedCrossoverProbability { get; set; } = 0.5;

        // Todo: remove
        static int callCounter = 0;
        const int runs = 3;

        /// <summary>
        ///Create default regarding the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static GenAlgStarterData GetDefault(OptimizationInput input)
        {
            var populationSize = 10;
            if (input.Visits.Length >= 50)
            {
                populationSize = 2;
            }

            var oCProbability = new double[]
            {
                0, 1
            };

            return new GenAlgStarterData()
            {
                //PopulationSize = populationSizes[(callCounter++ / runs) % populationSizes.Length],
                MaxNumberOfSantas = input.Santas.Length,
                OrderBasedCrossoverProbability = oCProbability[(callCounter++ / runs) % oCProbability.Length],
            };
        }

        public override string ToString()
        {
            return
                $"PopulationSize={PopulationSize}{Environment.NewLine}" +
                $"MaxNumberOfSantas={MaxNumberOfSantas}{Environment.NewLine}" +
                $"MaxNumberOfGenerations={MaxNumberOfGenerations}{Environment.NewLine}" +
                $"MutationProbability={MutationProbability}{Environment.NewLine}" +
                $"OrderBasedCrossoverProbability={OrderBasedCrossoverProbability}{Environment.NewLine}";
        }
    }
}
