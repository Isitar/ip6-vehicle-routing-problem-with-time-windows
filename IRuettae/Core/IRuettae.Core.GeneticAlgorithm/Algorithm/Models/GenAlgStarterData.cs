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
        public int PopulationSize { get; private set; } = 524288;
        // 2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384,32768,65536,131072,262144,524288

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

        /// <summary>
        /// Returns if this is a valid configuration.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (MaxNumberOfSantas < 1)
            {
                return false;
            }
            if (MaxNumberOfGenerations < 0)
            {
                return false;
            }
            // one individual for elitism plus one "normal" individual
            if (PopulationSize < 2)
            {
                return false;
            }

            // check percentages
            if (ElitismPercentage <= 0 || ElitismPercentage >= 1)
            {
                return false;
            }
            if (DirectMutationPercentage < 0 || DirectMutationPercentage > 1)
            {
                return false;
            }
            if (RandomPercentage < 0 || RandomPercentage > 1)
            {
                return false;
            }
            if (RandomPercentage < 0 || RandomPercentage > 1)
            {
                return false;
            }
            if (MutationProbability < 0 || MutationProbability > 1)
            {
                return false;
            }
            if (OrderBasedCrossoverProbability < 0 || OrderBasedCrossoverProbability > 1)
            {
                return false;
            }
            if (PositionMutationProbability < 0 || PositionMutationProbability > 1)
            {
                return false;
            }

            // check if PopulationSize does not change
            var size = (int)Math.Max(1, ElitismPercentage * PopulationSize) + (int)(DirectMutationPercentage * PopulationSize) + (int)(RandomPercentage * PopulationSize);
            if (size < 0 || size > PopulationSize)
            {
                return false;
            }

            return true;
        }

        public override string ToString() => string.Join(Environment.NewLine, GetType().GetProperties().Select(p => $"{p.Name}: {(p.GetIndexParameters().Length > 0 ? "Indexed Property cannot be used" : p.GetValue(this, null))}"));
    }
}
