using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using System.Security.Cryptography;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class PopulationGenerator
    {
        /// <summary>
        /// Returns generated genotypes and mapping from allele to visitId
        /// </summary>
        /// <param name="input"></param>
        /// <param name="numberOfIndividuals"></param>
        /// <param name="maxNumberOfSantas"></param>
        /// <returns></returns>
        public static (Genotype[], Dictionary<int, int>) Generate(OptimizationInput input, int numberOfIndividuals, int maxNumberOfSantas)
        {
            var numberOfSeparators = input.Days.Length * maxNumberOfSantas - 1;
            var alleleToVisitIdMapping = CreateAlleles(input, numberOfSeparators);

            var elements = new Genotype();
            elements.AddRange(alleleToVisitIdMapping.Keys);
            elements.AddRange(Enumerable.Range(-numberOfSeparators, numberOfSeparators));

            var population = new Genotype[numberOfIndividuals];
            for (int i = 0; i < numberOfIndividuals; i++)
            {
                elements.Shuffle();
                population[i] = new Genotype(elements);
            }
            return (population, alleleToVisitIdMapping);
        }

        private static Dictionary<int, int> CreateAlleles(OptimizationInput input, int numberOfSeparators)
        {
            var alleleToVisitIdMapping = new Dictionary<int, int>();
            // normal visits
            foreach (var visitId in input.Visits.Where(v => !v.IsBreak).Select(v => v.Id))
            {
                alleleToVisitIdMapping.Add(visitId, visitId);
            }
            // breaks
            var nextVisitId = input.Visits.Select(v => v.Id).Append(0).Max() + 1;
            foreach (var breakId in input.Visits.Where(v => !v.IsBreak).Select(v => v.Id))
            {
                alleleToVisitIdMapping.Add(breakId, breakId);
                foreach (var _ in input.Days.Skip(1))
                {
                    alleleToVisitIdMapping.Add(nextVisitId++, breakId);
                }
            }
            return alleleToVisitIdMapping;
        }

        public static bool IsSeparator(int allele)
        {
            return allele < 0;
        }
    }
}
