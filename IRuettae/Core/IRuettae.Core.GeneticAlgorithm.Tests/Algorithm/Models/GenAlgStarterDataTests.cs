using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.GeneticAlgorithm.Tests.Algorithm.Models
{
    [TestClass]
    public class GenAlgStarterDataTests
    {
        /// <summary>
        /// Some default percentage
        /// </summary>
        const double p = 0.2;

        /// <summary>
        /// Some default number
        /// </summary>
        const int n = 10;

        [TestMethod]
        public void TestIsValid_Default()
        {
            var data = GenAlgStarterData.GetDefault(new OptimizationInput()
            {
                Santas = new Santa[5],
                Visits = new Visit[10],
            });
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_Simple()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_ZeroSantas()
        {
            var data = new GenAlgStarterData(0, n, n, p, p, p, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_NegativeSantas()
        {
            var data = new GenAlgStarterData(-5, n, n, p, p, p, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_SmallestGenerations()
        {
            var data = new GenAlgStarterData(n, 0, n, p, p, p, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_NegativeGenerations()
        {
            var data = new GenAlgStarterData(n, -5, n, p, p, p, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooSmallPopulationSize()
        {
            var data = new GenAlgStarterData(n, n, 1, p, p, p, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_SmallestPopulationSize()
        {
            var data = new GenAlgStarterData(n, n, 2, p, p, p, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_NegativePopulationSize()
        {
            var data = new GenAlgStarterData(n, n, -2, p, p, p, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooSmallElitism()
        {
            var data = new GenAlgStarterData(n, n, n, 0, p, p, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_SmallElitism()
        {
            // this will lead to elitism of at least one element
            // even if percentage is lower
            var data = new GenAlgStarterData(n, n, n, 0.01, p, p, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_BiggestElitism()
        {
            var data = new GenAlgStarterData(n, n, n, 0.99, 0, 0, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooBigElitism()
        {
            var data = new GenAlgStarterData(n, n, n, 1.1, p, p, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_SmallestDirectMutation()
        {
            var data = new GenAlgStarterData(n, n, n, p, 0, p, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_BigDirectMutation()
        {
            var data = new GenAlgStarterData(n, n, n, 0.1, 0.9, 0.0, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooBigDirectMutation()
        {
            // 0.99 is too big as elitism needs at least 1 individual
            var data = new GenAlgStarterData(n, n, n, 0.01, 0.99, p, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_SmallestRandom()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, 0, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_BigRandom()
        {
            var data = new GenAlgStarterData(n, n, n, 0.1, 0.0, 0.9, p, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooBigRandom()
        {
            // 0.99 is too big as elitism needs at least 1 individual
            var data = new GenAlgStarterData(n, n, n, 0.01, p, 0.99, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_NegativeRandom()
        {
            var data = new GenAlgStarterData(n, n, n, 0.01, p, -0.01, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooBigSum()
        {
            // 34 + 34 + 34 = 101
            // -> Population size would increase
            var data = new GenAlgStarterData(n, n, 100, 0.34, 0.34, 0.34, p, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_SmallestOX()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, 0.0, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_BiggestOX()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, 1.0, p, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_NegativeOX()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, -0.01, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooBigOX()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, 1.01, p, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_SmallestMutationProbability()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, 0.0, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_BiggestMutationProbability()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, 1.0, p);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_NegativeMutationProbability()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, -0.01, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooBigMutationProbability()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, 1.01, p);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_SmallestPositionMutationProbability()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, p, 0.0);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_BiggestPositionMutationProbability()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, p, 1.0);
            Assert.IsTrue(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_NegativePositionMutationProbability()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, p, -0.01);
            Assert.IsFalse(data.IsValid());
        }

        [TestMethod]
        public void TestIsValid_TooBigPositionMutationProbability()
        {
            var data = new GenAlgStarterData(n, n, n, p, p, p, p, p, 1.01);
            Assert.IsFalse(data.IsValid());
        }
    }
}
