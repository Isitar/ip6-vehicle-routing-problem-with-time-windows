using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    /// <summary>
    /// Deterministic binary tournament selection.
    /// The two individuals for the tournament are selected randomly.
    /// </summary>
    public class BinaryTournamentSelection
    {
        private readonly Random random;

        /// <summary>
        ///
        /// </summary>
        /// <param name="random">not null</param>
        public BinaryTournamentSelection(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// Returns numberOfPairs pairs of genotypes from population which should reproduce.
        /// </summary>
        /// <param name="population">with calculated costs and count > 0</param>
        /// <param name="numberOfPairs"></param>
        /// <returns></returns>
        public List<(Genotype, Genotype)> Select(List<Genotype> population, int numberOfPairs)
        {
            var ret = new List<(Genotype, Genotype)>(numberOfPairs);
            while (numberOfPairs-- > 0)
            {
                var parent1 = GetParent(population);
                var parent2 = GetParent(population);
                ret.Add((parent1, parent2));
            }
            return ret;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="population">count > 0</param>
        /// <returns></returns>
        private Genotype GetParent(List<Genotype> population)
        {
            var candidate1 = GetRandomIndividual(population);
            var candidate2 = GetRandomIndividual(population);
            return candidate1.Cost < candidate2.Cost ? candidate1 : candidate2;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="population">count > 0</param>
        /// <returns></returns>
        private Genotype GetRandomIndividual(List<Genotype> population)
        {
            var index = random.Next(0, population.Count);
            return population[index];
        }
    }
}
