using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;
using System.Linq;

namespace IRuettae.Core.GeneticAlgorithm.Tests.Algorithm.Helpers
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        [TestMethod]
        public void TestSchuffle()
        {
            var actual = new List<int>
            {
                0,1,2,3,4,5
            };

            var random = new TestNumberGenerator();
            actual.Shuffle(random);

            var expected = new List<int>
            {
                4,3,5,2,1,0
            };
            // Note: the result highly depends on TestNumberGenerator
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
