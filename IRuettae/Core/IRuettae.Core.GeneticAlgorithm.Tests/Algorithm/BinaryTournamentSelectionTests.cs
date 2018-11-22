using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.GeneticAlgorithm.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Tests.Algorithm.Helpers;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Tests.Algorithm
{
    [TestClass()]
    public class BinaryTournamentSelectionTests
    {
        [TestMethod()]
        public void TestSelect()
        {
            var population = new List<Genotype>()
            {
                new Genotype(new[] { 0 }) { Cost = 0},
                new Genotype(new[] { 1 }) { Cost = 1},
                new Genotype(new[] { 2 }) { Cost = 2},
                new Genotype(new[] { 3 }) { Cost = 3},
                new Genotype(new[] { 4 }) { Cost = 4},
            };
            var selection = new BinaryTournamentSelection(new TestNumberGenerator());

            var expected = new List<(Genotype, Genotype)>
            {
                (population[0],population[2]),
                (population[0],population[1]),
                (population[3],population[0]),
            };
            var actual = selection.Select(population, 3);

            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}