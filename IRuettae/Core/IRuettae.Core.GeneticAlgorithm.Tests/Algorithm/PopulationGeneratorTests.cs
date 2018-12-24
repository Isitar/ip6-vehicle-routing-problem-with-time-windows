using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.GeneticAlgorithm.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Tests
{
    [TestClass()]
    public class PopulationGeneratorTests
    {
        [TestMethod()]
        public void TestGenerate()
        {
            var input = new OptimizationInput()
            {
                Santas = new Santa[]
                {
                    new Santa
                    {
                        Id = 100,
                    },
                    new Santa
                    {
                        Id = 200,
                    },
                },
                Visits = new Visit[]
                {
                    new Visit
                    {
                        Id = 1,
                    },
                    new Visit
                    {
                        Id = 2,
                    },
                    new Visit
                    {
                        Id = 3,
                    },
                    new Visit
                    {
                        Id = 4,
                    },
                    new Visit
                    {
                        Id = 5,
                        IsBreak = true,
                        SantaId = 100,
                    }
                },
                Days = new(int, int)[]
                {
                    (1,2),
                    (2,3),
                }
            };

            int numberOfVisits = 4;
            int numberOfBreaks = 1;
            int numberOfIndividuals = 10;
            int maxNumberOfSantas = 5;
            int numberOfSeparators = input.Days.Length * maxNumberOfSantas - 1;
            int expectedNumberOfGenes = numberOfVisits + numberOfSeparators + numberOfBreaks * input.Days.Length;
            var (actual, mapping) = new PopulationGenerator(new Random()).Generate(input, numberOfIndividuals, maxNumberOfSantas);

            Assert.AreEqual(numberOfIndividuals, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                var individual = actual[i];
                Assert.AreEqual(expectedNumberOfGenes, individual.Count);
                Assert.AreEqual(expectedNumberOfGenes, individual.Distinct().Count());
                Assert.AreEqual(numberOfSeparators, individual.Where(e => e < 0).Count());
                Assert.IsTrue(individual.Contains(1));
                Assert.IsTrue(individual.Contains(2));
                Assert.IsTrue(individual.Contains(3));
                Assert.IsTrue(individual.Contains(4));
                Assert.IsTrue(individual.Contains(5));
                // cloned break must map to original break
                Assert.IsTrue(individual
                    .Where(allel => !PopulationGenerator.IsSeparator(allel) && !new[] { 1, 2, 3, 4, 5 }.Contains(allel))
                    .Select(allel => mapping[allel])
                    .First() == 5);
            }
        }
    }
}