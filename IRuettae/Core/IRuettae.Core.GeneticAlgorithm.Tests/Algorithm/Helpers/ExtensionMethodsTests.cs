using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;

namespace IRuettae.Core.GeneticAlgorithm.Tests.Algorithm.Helpers
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        [TestMethod]
        public void TestNextInt()
        {
            TestNextInt(0, 5);
            TestNextInt(-5, 0);
            TestNextInt(-100, 100);
            TestNextInt(-100, -100);
            TestNextInt(100, 100);
        }

        private void TestNextInt(int min, int max)
        {
            var generator = new TestNumberGenerator();

            var numberOfCyles = 3;
            while (numberOfCyles-- > 0)
            {
                for (int i = min; i <= max; i++)
                {
                    Assert.AreEqual(i, generator.NextInt(min, max));
                }
            }
        }
    }
}
