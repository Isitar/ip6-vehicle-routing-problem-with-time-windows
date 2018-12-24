using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class EvolutionOperation
    {
        private readonly BinaryTournamentSelection selectionOperation = new BinaryTournamentSelection(RandomFactory.Instance);
        private readonly MutationOperation mutationOperation = new MutationOperation(RandomFactory.Instance);
        private readonly RecombinationOperation recombinationOperation;
        private readonly GenAlgStarterData starterData;

        public EvolutionOperation(GenAlgStarterData starterData)
        {
            this.starterData = starterData;
            this.recombinationOperation = new RecombinationOperation(RandomFactory.Instance, starterData.OrderBasedCrossoverProbability);
        }

        public void Evolve(List<Genotype> population)
        {
            // Order by Cost
            population.Sort((i1, i2) => i1.Cost.CompareTo(i2.Cost));

            // Evolve
            var parents = selectionOperation.SelectParents(population, population.Count - 1);
            var newPopulation = parents.Select(p => recombinationOperation.Recombinate(p.Item1, p.Item2)).ToList();

            // Mutate
            mutationOperation.Mutate(newPopulation, starterData.MutationProbability);

            // Elitism
            newPopulation.Add(population[0]);

            // Swap
            population.Clear();
            population.AddRange(newPopulation);
        }
    }
}
