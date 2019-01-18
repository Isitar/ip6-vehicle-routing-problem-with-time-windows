using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Tests.Algorithm;

namespace IRuettae.Core.Google.Routing.Tests.Algorithm
{
    [TestClass()]
    public class StartEndCreatorTests
    {
        [TestMethod()]
        public void TestCreate_Simple()
        {
            var actual = Testdata1.Create();
            var numberOfSantas = 3;
            new SantaCreator(actual).Create(numberOfSantas);
            new VisitCreator(actual).Create();

            new StartEndCreator(actual).Create();

            var length = actual.SantaIds.Length;
            Assert.AreEqual(length, actual.SantaStartIndex.Length);
            Assert.AreEqual(length, actual.SantaEndIndex.Length);

            var home = actual.HomeIndex;
            var homeAdditional = actual.HomeIndexAdditional;

            // starts
            var expectedStarts = new int[]
            {
                home,home,homeAdditional,
                home,home,homeAdditional,
            };
            Enumerable.SequenceEqual(expectedStarts, actual.SantaStartIndex);

            // ends
            var expectedEnds = new int[]
            {
                home,home,home,
                home,home,home,
            };
            Enumerable.SequenceEqual(expectedEnds, actual.SantaEndIndex);

        }
    }
}