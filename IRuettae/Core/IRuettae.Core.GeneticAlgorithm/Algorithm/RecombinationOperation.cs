using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class RecombinationOperation
    {
        /// <summary>
        /// Precondition: parent1 and parent2 contain the same alleles.
        /// Postcondition: child contains the same alleles as parent1.
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <returns></returns>
        public static Genotype Recombinate(Genotype parent1, Genotype parent2)
        {
            var count = parent1.Count;
            if (count <= 1)
            {
                return parent1;
            }

            var neighbours = GetNeighbours(parent1, parent2);

            var previousAllele = parent1[0];
            var child = new Genotype(count)
            {
                previousAllele
            };

            while (child.Count < count)
            {
                DeleteNeighbour(neighbours, previousAllele);
                var insertAllele = FindNextAllele(neighbours, previousAllele);
                neighbours.Remove(previousAllele);
                child.Add(insertAllele);
                previousAllele = insertAllele;
            }

            return child;
        }

        /// <summary>
        /// Returns the allele which should follow the given allele.
        /// Precondition: neighbours has a non-null value for key=previousAllele.
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="previousAllele"></param>
        /// <returns></returns>
        private static int FindNextAllele(Dictionary<int, ISet<int>> neighbours, int previousAllele)
        {
            var lowestNumberOfNeighbours = int.MaxValue;
            int? nextAllele = null;
            foreach (var allele in neighbours[previousAllele])
            {
                var numberOfNeighbours = neighbours[allele].Count;
                if (numberOfNeighbours < lowestNumberOfNeighbours)
                {
                    lowestNumberOfNeighbours = numberOfNeighbours;
                    nextAllele = allele;
                }
            }

            if (!nextAllele.HasValue)
            {
                // take first allele
                nextAllele = neighbours.Keys.Where(allele => allele != previousAllele).First();
            }

            return nextAllele.Value;
        }

        /// <summary>
        /// Removes every occurence of allele in every list of neighbours.
        /// Post condition: neighbours[allele] still exists
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="allele"></param>
        /// <returns></returns>
        private static void DeleteNeighbour(Dictionary<int, ISet<int>> neighbours, int allele)
        {
            foreach (var key in neighbours.Keys)
            {
                neighbours[key].Remove(allele);
            }
        }

        /// <summary>
        /// Returns a mapping from each allele to a list of its neighbours.
        /// A neighbour is an allele which is right before or right after the allele.
        /// It does not matter if a neighbour occures in parent1 or parent2.
        /// Precondition: parent1 and parent2 contain the same alleles.
        /// Precondition: parent1 contains at least 2 alleles.
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <returns></returns>
        private static Dictionary<int, ISet<int>> GetNeighbours(Genotype parent1, Genotype parent2)
        {
            // allele to neighbours
            var neighbours = new Dictionary<int, ISet<int>>();
            foreach (var allele in parent1)
            {
                neighbours[allele] = new SortedSet<int>();
            }

            void addNeightbour(int pos, int posNeighbour)
            {
                neighbours[parent1[pos]].Add(parent1[posNeighbour]);
                neighbours[parent2[pos]].Add(parent2[posNeighbour]);
            }

            // first allele
            addNeightbour(0, 1);

            // middle alleles
            var count = parent1.Count;
            for (int i = 1; i < count - 1; i++)
            {
                // before
                addNeightbour(i, i - 1);
                // after
                addNeightbour(i, i + 1);
            }

            // last allele
            addNeightbour(count - 1, count - 2);

            return neighbours;
        }
    }
}
