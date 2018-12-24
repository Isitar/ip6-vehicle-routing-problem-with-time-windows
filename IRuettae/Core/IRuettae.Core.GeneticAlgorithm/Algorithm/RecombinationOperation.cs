using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class RecombinationOperation
    {
        private readonly double orderBasedCrossoverProbability;
        private Random random;

        /// <summary>
        /// Factor to be multiplied with population size
        /// to get minimum number of positions to retain in OrderBasedCrossover
        /// </summary>
        private const double MinimumNumberOfPositionsFactor = 1d / 4d;
        /// <summary>
        /// Factor to be multiplied with population size
        /// to get maximum number of positions to retain in OrderBasedCrossover
        /// </summary>
        private const double MaximumNumberOfPositionsFactor = 3d / 4d;

        /// <summary>
        ///
        /// </summary>
        /// <param name="random">not null</param>
        /// <param name="orderBasedCrossoverProbability">between 0 and 1</param>
        public RecombinationOperation(Random random, double orderBasedCrossoverProbability)
        {
            this.random = random;
            this.orderBasedCrossoverProbability = orderBasedCrossoverProbability;
        }

        /// <summary>
        /// Precondition: parent1 and parent2 contain the same alleles.
        /// Postcondition: child contains the same alleles as parent1.
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <returns></returns>
        public Genotype Recombinate(Genotype parent1, Genotype parent2)
        {
            if (random.NextDouble() < orderBasedCrossoverProbability)
            {
                return OrderBasedCrossover(parent1, parent2);
            }
            else
            {
                return EdgeRecombinationCrossover(parent1, parent2);
            }
        }

        /// <summary>
        /// order-based crossover operator (OX2)
        /// Precondition: parent1 and parent2 contain the same alleles.
        /// Postcondition: child contains the same alleles as parent1.
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <returns></returns>
        public Genotype OrderBasedCrossover(Genotype parent1, Genotype parent2)
        {
            var count = parent1.Count;
            if (count <= 1)
            {
                return parent1;
            }

            var minNumberOfPositions = (int)Math.Max(1, count * MinimumNumberOfPositionsFactor);
            var maxNumberOfPositions = (int)Math.Max(1, count * MaximumNumberOfPositionsFactor);
            var numberOfPositions = random.Next(minNumberOfPositions, maxNumberOfPositions);

            // select alleles
            var selectedAllelesP2 = new HashSet<int>();
            for (int i = 0; i < numberOfPositions; i++)
            {
                if (!selectedAllelesP2.Add(parent2[random.Next(0, count)]))
                {
                    // already selected, try again
                    i--;
                }
            }

            // copy alleles from parent1 which are not selected
            var emptyIndices = new List<int>(count - numberOfPositions);
            var child = new int[count];
            for (int i = 0; i < count; i++)
            {
                var allele = parent1[i];
                if (!selectedAllelesP2.Contains(allele))
                {
                    child[i] = allele;
                }
                else
                {
                    emptyIndices.Add(i);
                }
            }

            // add the missing alleles in the order they appear in parent2
            var nextPositionParent2 = 0;
            var missingAlleles = parent2.Intersect(selectedAllelesP2).ToArray();
            foreach (var i in emptyIndices)
            {
                // find next allele
                child[i] = missingAlleles[nextPositionParent2];
                nextPositionParent2++;
            }

            return new Genotype(child);
        }

        /// <summary>
        /// edge recombination operator (ER)
        /// Precondition: parent1 and parent2 contain the same alleles.
        /// Postcondition: child contains the same alleles as parent1.
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <returns></returns>
        public static Genotype EdgeRecombinationCrossover(Genotype parent1, Genotype parent2)
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
        private static int FindNextAllele(Dictionary<int, List<int>> neighbours, int previousAllele)
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
                nextAllele = neighbours.Keys.First(allele => allele != previousAllele);
            }

            return nextAllele.Value;
        }

        /// <summary>
        /// Removes every occurence of allele in every list of neighbours.
        /// Precondition: the neighbours in neighbours[x] are unique
        /// Postcondition: neighbours[allele] still exists
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="allele"></param>
        /// <returns></returns>
        private static void DeleteNeighbour(Dictionary<int, List<int>> neighbours, int allele)
        {
            foreach (var key in neighbours.Keys)
            {
                // slightly faster deletion than with remove
                var list = neighbours[key];
                var index = list.IndexOf(allele);
                if (index < 0)
                {
                    continue;
                }

                // overwrite index with last
                var lastIndex = list.Count - 1;
                list[index] = list[lastIndex];

                // delete last
                list.RemoveAt(lastIndex);
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
        private static Dictionary<int, List<int>> GetNeighbours(Genotype parent1, Genotype parent2)
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

            // convert sets to list
            // needed to improve performance of removing elements
            var ret = new Dictionary<int, List<int>>(neighbours.Count);
            foreach (var keyValuePair in neighbours)
            {
                ret[keyValuePair.Key] = new List<int>(keyValuePair.Value);
            }

            return ret;
        }
    }
}
