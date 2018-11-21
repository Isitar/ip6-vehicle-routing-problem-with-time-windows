using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Algorithm;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.GeneticAlgorithm.Tests.Algorithm
{
    [TestClass()]
    public class RecombinationOperationTests
    {
        [TestMethod()]
        public void TestRecombinate()
        {
            Genotype parent1 = new Genotype()
            {
                5,2,3,1,4,
            };
            Genotype parent2 = new Genotype()
            {
                4,5,2,1,3,
            };

            var actual = RecombinationOperation.Recombinate(parent1, parent2);
            var expected = new Genotype()
            {
                5,4,1,2,3
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TestRecombinate_Same()
        {
            Genotype parent1 = new Genotype()
            {
                1,2,3,4,5,
            };
            Genotype parent2 = new Genotype()
            {
                1,2,3,4,5,
            };

            var actual = RecombinationOperation.Recombinate(parent1, parent2);
            var expected = new Genotype()
            {
                1,2,3,4,5,
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TestRecombinate_Same2()
        {
            Genotype parent1 = new Genotype()
            {
                5,4,3,2,1,
            };
            Genotype parent2 = new Genotype()
            {
                5,4,3,2,1,
            };

            var actual = RecombinationOperation.Recombinate(parent1, parent2);
            var expected = new Genotype()
            {
                5,4,3,2,1,
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TestRecombinate_Reverse1()
        {
            Genotype parent1 = new Genotype()
            {
                1,2,3,4,5,
            };
            Genotype parent2 = new Genotype()
            {
                5,4,3,2,1,
            };

            var actual = RecombinationOperation.Recombinate(parent1, parent2);
            var expected = new Genotype()
            {
                1,2,3,4,5,
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TestRecombinate_Reverse2()
        {
            Genotype parent1 = new Genotype()
            {
                5,4,3,2,1,
            };
            Genotype parent2 = new Genotype()
            {
                1,2,3,4,5,
            };

            var actual = RecombinationOperation.Recombinate(parent1, parent2);
            var expected = new Genotype()
            {
                5,4,3,2,1,
            };

            Assert.AreEqual(expected, actual);
        }
    }
}
