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
                    }
                },
                Days = new(int, int)[]
                {
                    (1,2),
                    (2,3),
                }
            };

            int numberOfVisits = 5;
            int numberOfIndividuals = 10;
            int maxNumberOfSantas = 4;
            int numberOfSeparators = input.Days.Length * maxNumberOfSantas - 1;
            var actual = PopulationGenerator.Generate(input, numberOfIndividuals, maxNumberOfSantas);

            Assert.AreEqual(numberOfIndividuals, actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                var individual = actual[i];
                Assert.AreEqual(numberOfVisits + numberOfSeparators, individual.Count);
                Assert.AreEqual(numberOfVisits + numberOfSeparators, individual.Distinct().Count());
                Assert.AreEqual(numberOfSeparators, individual.Where(e => e < 0).Count());
                Assert.IsTrue(individual.Contains(1));
                Assert.IsTrue(individual.Contains(2));
                Assert.IsTrue(individual.Contains(3));
                Assert.IsTrue(individual.Contains(4));
                Assert.IsTrue(individual.Contains(5));
            }
        }
    }
}