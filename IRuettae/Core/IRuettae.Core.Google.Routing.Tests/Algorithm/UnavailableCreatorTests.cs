using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Google.Routing.Tests.Algorithm
{
    [TestClass()]
    public class UnavailableCreatorTests
    {
        [TestMethod()]
        public void TestCreate_Simple()
        {
            var actual = Testdata1.Create();
            var numberOfSantas = 3;
            new SantaCreator(actual).Create(numberOfSantas);
            new VisitCreator(actual).Create();
            new UnavailableCreator(actual).Create();

            Assert.AreEqual(actual.Visits.Count, actual.Unavailable.Count);

            // visit 1
            {
                Assert.AreEqual(4, actual.Unavailable[0].Count);
                var (from, to) = actual.Unavailable[0].Last();
                Assert.AreEqual(Testdata1.StartDay1 - Testdata1.Duration1, from);
                Assert.AreEqual(Testdata1.EndDay1, to);
            }

            // visit2
            {
                Assert.AreEqual(4, actual.Unavailable[1].Count);
                var (from, to) = actual.Unavailable[1].Last();
                Assert.AreEqual(Testdata1.StartDay2 - Testdata1.Duration2, from);
                Assert.AreEqual(Testdata1.EndDay2, to);
            }

            // normal visits have default unavailables
            for (int i = 0; i < actual.Unavailable.Count; i++)
            {
                if (i == actual.HomeIndex || i == actual.HomeIndexAdditional || actual.Visits[i].IsBreak)
                {
                    // home / breaks
                    continue;
                }

                CheckDefaultUnavailable(actual.Unavailable[i], actual.Visits[i].Duration);
            }

        }

        private void CheckDefaultUnavailable(List<(int startFrom, int startEnd)> unavailable, int duration)
        {
            {
                var (from, to) = unavailable[0];
                Assert.AreEqual(int.MinValue, from);
                Assert.AreEqual(Testdata1.StartDay1 - 1, to);
            }
            {
                var (from, to) = unavailable[1];
                Assert.AreEqual(Testdata1.EndDay1 + 1 - duration, from);
                Assert.AreEqual(Testdata1.StartDay2 - 1, to);
            }
            {
                var (from, to) = unavailable[2];
                Assert.AreEqual(Testdata1.EndDay2 + 1 - duration, from);
                Assert.AreEqual(int.MaxValue, to);
            }
        }
    }
}