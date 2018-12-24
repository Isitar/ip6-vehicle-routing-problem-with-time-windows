using System.Collections.Generic;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    /// <summary>
    /// Class to recalculate the cost of Genotypes
    /// </summary>
    public class CostCalculator
    {
        private readonly OptimizationResult result;
        private readonly Decoder decoder;

        public CostCalculator(Decoder decoder, OptimizationResult temporaryResult)
        {
            this.decoder = decoder;
            this.result = temporaryResult;
        }

        public void RecalculatateCost(IEnumerable<Genotype> population)
        {
            foreach (var individual in population)
            {
                result.Routes = decoder.Decode(individual);
                individual.Cost = result.Cost();
            }
        }

        public void RecalculatateCost(Genotype individual)
        {
            result.Routes = decoder.Decode(individual);
            individual.Cost = result.Cost();
        }
    }
}
