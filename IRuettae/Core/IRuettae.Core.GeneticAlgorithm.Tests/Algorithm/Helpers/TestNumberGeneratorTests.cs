using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.GeneticAlgorithm.Tests.Algorithm.Helpers
{
    [TestClass]
    public class TestNumberGeneratorTests
    {
        [TestMethod]
        public void TestGetBytes()
        {
            var generator = new TestNumberGenerator();
            var bytes = new byte[4];

            for (int expected = 0; expected < 1000; expected++)
            {
                generator.NextBytes(bytes);
                Assert.AreEqual(expected, BitConverter.ToInt32(bytes, 0));
            }
        }

        [TestMethod]
        public void TestNext()
        {
            var generator = new TestNumberGenerator();
            Assert.AreEqual(int.MinValue, generator.Next());
            Assert.AreEqual(int.MinValue + 1, generator.Next());
        }

        [TestMethod]
        public void TestNextMax()
        {
            var generator = new TestNumberGenerator();

            int max = 1000;
            for (int expected = 0; expected < max; expected++)
            {
                Assert.AreEqual(expected, generator.Next(1000));
            }

            Assert.AreEqual(0, generator.Next(1000));
        }

        [TestMethod]
        public void TestNextMinMax()
        {
            var generator = new TestNumberGenerator();

            int min = -100;
            int max = 100;
            for (int expected = 0; expected < max - min; expected++)
            {
                Assert.AreEqual(expected + min, generator.Next(min, max));
            }

            Assert.AreEqual(min, generator.Next(min, max));
        }
    }
}
