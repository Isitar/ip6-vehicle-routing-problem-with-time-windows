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
                generator.GetBytes(bytes);
                Assert.AreEqual(expected, BitConverter.ToInt32(bytes, 0));
            }
        }
    }
}
