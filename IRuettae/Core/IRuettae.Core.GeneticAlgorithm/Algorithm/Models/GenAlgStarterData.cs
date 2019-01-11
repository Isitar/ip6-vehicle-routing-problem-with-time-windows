using System;
using System.Linq;
using System.Text;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    public class GenAlgStarterData
    {
        public int MaxNumberOfSantas { get; private set; }
        public long MaxNumberOfGenerations { get; private set; } = long.MaxValue;
        public int PopulationSize { get; private set; } = 102;
        // 2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384,32768,65536,131072,

        public double ElitismPercentage { get; private set; } = 0.357;
        public double DirectMutationPercentage { get; private set; } = 0.378;
        public double RandomPercentage { get; private set; } = 0.0;

        public double OrderBasedCrossoverProbability { get; private set; } = 0.884;
        public double MutationProbability { get; private set; } = 0.0;
        public double PositionMutationProbability { get; private set; } = 0.886;

        /// <summary>
        ///Create default regarding the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static GenAlgStarterData GetDefault(OptimizationInput input)
        {

            var starterData = new GenAlgStarterData()
            {
                MaxNumberOfSantas = input.Santas.Length,
            };

            return starterData;
        }

        private GenAlgStarterData()
        {

        }

        public GenAlgStarterData(int maxNumberOfSantas, long maxNumberOfGenerations, int populationSize, double elitismPercentage, double directMutationPercentage, double randomPercentage, double orderBasedCrossoverProbability, double mutationProbability, double positionMutationProbability)
        {
            MaxNumberOfSantas = maxNumberOfSantas;
            MaxNumberOfGenerations = maxNumberOfGenerations;
            PopulationSize = populationSize;
            ElitismPercentage = elitismPercentage;
            DirectMutationPercentage = directMutationPercentage;
            RandomPercentage = randomPercentage;
            OrderBasedCrossoverProbability = orderBasedCrossoverProbability;
            MutationProbability = mutationProbability;
            PositionMutationProbability = positionMutationProbability;
        }

        public override string ToString() => string.Join(Environment.NewLine, GetType().GetProperties().Select(p => $"{p.Name}: {(p.GetIndexParameters().Length > 0 ? "Indexed Property cannot be used" : p.GetValue(this, null))}"));
    }
}
