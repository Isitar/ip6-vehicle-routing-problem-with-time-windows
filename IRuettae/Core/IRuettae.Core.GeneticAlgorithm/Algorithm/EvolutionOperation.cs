using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class EvolutionOperation
    {
        private GenAlgStarterData starterData;

        public EvolutionOperation(GenAlgStarterData starterData)
        {
            this.starterData = starterData;
        }

        internal void Evolve(List<Genotype> population)
        {
            throw new NotImplementedException();
        }
    }
}
