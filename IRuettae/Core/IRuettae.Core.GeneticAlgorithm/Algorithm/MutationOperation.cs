using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class MutationOperation
    {
        private const double probabilityPositionMutation = 0.5;
        private const double probabilityInversionMutation = 1.0 - probabilityPositionMutation;
        private readonly RandomNumberGenerator rng;

        public MutationOperation(RandomNumberGenerator rng)
        {
            this.rng = rng;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="population"></param>
        /// <param name="probability">between 0 and 1</param>
        public void Mutate(List<Genotype> population, double probability)
        {
            for (int i = 0; i < population.Count; i++)
            {
                if (rng.NextProbability() > probability)
                {
                    // no mutation
                    continue;
                }

                var individual = population[i];
                if (rng.NextProbability() > probabilityPositionMutation)
                {
                    PositionMutate(individual);
                }
                else
                {
                    InversionMutate(individual);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="individual">not null</param>
        private void PositionMutate(Genotype individual)
        {
            var mutationSize = GetMutationSize(1, individual.Count / 10d);
            while (mutationSize-- > 0)
            {
                var position1 = rng.NextInt(0, individual.Count - 1);
                var position2 = rng.NextInt(0, individual.Count - 1);

                // swap
                var temp = individual[position1];
                individual[position1] = individual[position2];
                individual[position2] = temp;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="individual">not null</param>
        private void InversionMutate(Genotype individual)
        {
            var count = individual.Count;
            var inversionSize = Math.Min(count, GetMutationSize(2, count / 4d));
            var inversionStart = rng.NextInt(0, count - inversionSize);

            var mutationSubset = individual.GetRange(inversionStart, inversionSize);
            var afterSubset = individual.GetRange(inversionStart + inversionSize, count - (inversionStart + inversionSize));

            // remove everything after inversionStart
            individual.RemoveRange(inversionStart, count - inversionStart);

            // readd reverse
            mutationSubset.Reverse();
            individual.AddRange(mutationSubset);

            // readd subset after mutation
            individual.AddRange(afterSubset);
        }

        /// <summary>
        /// Returns a random, guassian distributed number which is at least min.
        /// Source: https://stackoverflow.com/questions/218060/random-gaussian-variables
        /// </summary>
        /// <param name="min"></param>
        /// <param name="stdev">not 0</param>
        /// <returns></returns>
        private int GetMutationSize(int min, double stdev)
        {
            double u1;
            do
            {
                u1 = rng.NextProbability();
            } while (u1 == 0.0);

            double u2;
            do
            {
                u2 = rng.NextProbability();
            } while (u2 == 0.0);

            var randomNormal = Math.Cos(2d * Math.PI * u1) * Math.Sqrt(-2d * Math.Log(u2));

            return min + (int)Math.Abs(stdev * randomNormal);
        }
    }
}