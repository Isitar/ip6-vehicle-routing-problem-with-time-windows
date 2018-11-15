using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    /// <summary>
    /// Class to hold a list of alleles
    /// </summary>
    public class Genotype : List<int>
    {
        public Genotype() : base()
        {
        }
        public Genotype(int capacity) : base(capacity)
        {
        }
        public Genotype(IEnumerable<int> collection) : base(collection)
        {
        }
    }
}
