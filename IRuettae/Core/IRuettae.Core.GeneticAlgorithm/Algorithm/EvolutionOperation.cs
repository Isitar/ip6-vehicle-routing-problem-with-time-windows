using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class EvolutionOperation
    {
        private readonly BinaryTournamentSelection selectionOperation = new BinaryTournamentSelection(RandomFactory.Instance);
        private readonly MutationOperation mutationOperation;
        private readonly RecombinationOperation recombinationOperation;
        private readonly GenAlgStarterData starterData;

        public EvolutionOperation(GenAlgStarterData starterData)
        {
            this.starterData = starterData;
            this.recombinationOperation = new RecombinationOperation(RandomFactory.Instance, starterData.OrderBasedCrossoverProbability);
            this.mutationOperation = new MutationOperation(RandomFactory.Instance, starterData.PositionMutationProbability);
        }

        public void Evolve(List<Genotype> population)
        {
            // Order by Cost
            population.Sort((i1, i2) => i1.Cost.CompareTo(i2.Cost));

            // result
            var newPopulation = new List<Genotype>(population.Count);

            // Elitism
            {

                // number of best individuals that should get taken directly to the next generation
                // must be at least one to save best solution
                var numberOfEliteIndividuals = (int)Math.Max(1, population.Count * starterData.ElitismPercentage);
                newPopulation.AddRange(population.Take(numberOfEliteIndividuals));
            }

            // Direct mutation
            {
                var numberOfDirectMutation = (int)(population.Count * starterData.DirectMutationPercentage);
                var selection = selectionOperation.SelectIndividuals(population, numberOfDirectMutation);

                // mutate (always)
                mutationOperation.Mutate(selection, 1.0);

                newPopulation.AddRange(selection);
            }

            // Random
            {
                var numberOfRandom = (int)(population.Count * starterData.RandomPercentage);
                var random = RandomFactory.Instance;
                for (int i = 0; i < numberOfRandom; i++)
                {
                    var temp = new Genotype(population[0]);
                    temp.Shuffle(random);
                    newPopulation.Add(temp);
                }
            }

            // Fill up with recombination & mutation
            {
                var parents = selectionOperation.SelectParents(population, population.Count - newPopulation.Count);
                var recombinatedPopulation = parents.Select(p => recombinationOperation.Recombinate(p.Item1, p.Item2)).ToList();

                // Mutate with probability
                mutationOperation.Mutate(recombinatedPopulation, starterData.MutationProbability);

                newPopulation.AddRange(recombinatedPopulation);
            }

            // Swap
            population.Clear();
            population.AddRange(newPopulation);
        }
    }
}
