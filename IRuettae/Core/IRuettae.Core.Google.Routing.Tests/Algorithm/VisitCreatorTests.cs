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
    public class VisitCreatorTests
    {
        [TestMethod()]
        public void TestCreate_Simple()
        {
            var actual = Testdataset1.Create();
            var numberOfSantas = 3;
            new SantaCreator(actual).Create(numberOfSantas);

            new VisitCreator(actual).Create();

            // 4 normal
            // 2 breaks
            // 2 homes
            Assert.AreEqual(8, actual.Visits.Length);
            for (int i = 0; i < actual.Input.Visits.Length; i++)
            {
                Assert.AreEqual(actual.Input.Visits[i].Id, actual.Visits[i].Id);
            }

            // test break on day1
            Assert.AreEqual(1, actual.Visits[4].SantaId);
            Assert.AreEqual(int.MinValue, actual.Visits[4].Unavailable[0].from);
            Assert.AreEqual(Testdataset1.StartDay1 - 1, actual.Visits[4].Unavailable[0].to);
            Assert.AreEqual(Testdataset1.EndDay1 + 1, actual.Visits[4].Unavailable[1].from);
            Assert.AreEqual(int.MaxValue, actual.Visits[4].Unavailable[1].to);
            Assert.AreEqual(1, actual.Visits[4].Desired.Length);
            Assert.AreEqual(Testdataset1.StartDay1 + Testdataset1.BreakDesiredStart, actual.Visits[4].Desired[0].from);
            Assert.AreEqual(Testdataset1.StartDay1 + Testdataset1.BreakDesiredEnd, actual.Visits[4].Desired[0].to);

            // test break on day2
            Assert.AreEqual(4, actual.Visits[5].SantaId);
            Assert.AreEqual(int.MinValue, actual.Visits[5].Unavailable[0].from);
            Assert.AreEqual(Testdataset1.StartDay2 - 1, actual.Visits[5].Unavailable[0].to);
            Assert.AreEqual(Testdataset1.EndDay2 + 1, actual.Visits[5].Unavailable[1].from);
            Assert.AreEqual(int.MaxValue, actual.Visits[5].Unavailable[1].to);
            Assert.AreEqual(1, actual.Visits[5].Desired.Length);
            Assert.AreEqual(Testdataset1.StartDay2 + Testdataset1.BreakDesiredStart, actual.Visits[5].Desired[0].from);
            Assert.AreEqual(Testdataset1.StartDay2 + Testdataset1.BreakDesiredEnd, actual.Visits[5].Desired[0].to);

            // home 1
            var home1Index = 6;
            Assert.AreEqual(-1, actual.Visits[home1Index].Id);
            Assert.AreEqual(home1Index, actual.HomeIndex);

            // home 2
            var home2Index = 7;
            Assert.AreEqual(-1, actual.Visits[home2Index].Id);
            Assert.AreEqual(home2Index, actual.HomeIndexAdditional);

            // desired/unavailable must not be null
            foreach (var visit in actual.Visits)
            {
                Assert.IsNotNull(visit.Desired);
                Assert.IsNotNull(visit.Unavailable);
            }
        }
    }
}