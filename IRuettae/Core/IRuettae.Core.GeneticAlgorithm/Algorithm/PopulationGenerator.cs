using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.Models;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class PopulationGenerator
    {
        private readonly Random random;

        public PopulationGenerator(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// Returns generated genotypes and mapping from allele to visitId.
        /// The returned Genotypes are already repaired.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="numberOfIndividuals"></param>
        /// <param name="maxNumberOfSantas"></param>
        /// <returns>Population and the AlleleToVisitIdMapping</returns>
        public (List<Genotype>, Dictionary<int, int>) Generate(OptimizationInput input, int numberOfIndividuals, int maxNumberOfSantas)
        {
            var numberOfSeparators = input.Days.Length * maxNumberOfSantas - 1;
            var alleleToVisitIdMapping = CreateAlleles(input);

            var elements = new Genotype();

            // empty
            // Z: 18,19,20,21,22,23,24
            // M: 25,26,27,28,29,30
            // A: 31,3,4,5,2,32,1,0
            // W: 11,6,7,8,10,9
            // G: 13,12,15,14,16,17
            elements.AddRange(new int[] { 33, -1, 18, 19, 20, 21, 22, 23, 24, -1, 25, 26, 27, 28, 29, 30, -1, 31, 3, 4, 5, 2, 32, 1, 0, -1, 11, 6, 7, 8, 10, 9, -1, 13, 12, 15, 14, 16, 17 });
            //elements.AddRange(alleleToVisitIdMapping.Keys);
            //elements.AddRange(Enumerable.Range(-numberOfSeparators, numberOfSeparators));

            var repairOperation = new RepairOperation(input, alleleToVisitIdMapping);
            var population = new List<Genotype>(numberOfIndividuals);
            for (int i = 0; i < numberOfIndividuals; i++)
            {
                //elements.Shuffle(random);
                var genotype = new Genotype(elements);
                //repairOperation.Repair(genotype);
                population.Add(genotype);
            }
            return (population, alleleToVisitIdMapping);
        }

        private static Dictionary<int, int> CreateAlleles(OptimizationInput input)
        {
            var alleleToVisitIdMapping = new Dictionary<int, int>();
            // normal visits
            foreach (var visitId in input.Visits.Where(v => !v.IsBreak).Select(v => v.Id))
            {
                alleleToVisitIdMapping.Add(visitId, visitId);
            }
            // breaks
            var nextVisitId = input.Visits.Select(v => v.Id).Append(0).Max() + 1;
            foreach (var breakId in input.Visits.Where(v => v.IsBreak).Select(v => v.Id))
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
