using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    /// <summary>
    /// Class to hold a list of alleles
    /// The size and the alleles should not change,
    /// but the order of the alleles can change.
    /// </summary>
    public class Genotype : List<int>
    {
        public int Cost { get; set; }

        public Genotype() : base()
        {
        }

        public Genotype(int capacity) : base(capacity)
        {
        }

        public Genotype(IEnumerable<int> collection) : base(collection)
        {
        }

        /// <summary>
        /// Returns the number of Routes which are encoded in this genotype
        /// </summary>
        /// <returns></returns>
        public int CountRoutes()
        {
            return this.Where(PopulationGenerator.IsSeparator).Count() + 1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this.SequenceEqual((Genotype)obj);
        }

        public override int GetHashCode()
        {
            if (Count > 0)
            {
                return this[0];
            }
            return 0;
        }
    }
}
