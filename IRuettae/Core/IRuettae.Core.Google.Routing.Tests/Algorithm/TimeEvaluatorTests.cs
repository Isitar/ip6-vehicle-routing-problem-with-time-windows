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
    public class TimeEvaluatorTests
    {
        [TestMethod()]
        public void TestRun_Simple()
        {
            var data = Testdata1.Create();
            var santaCreator = new SantaCreator(data);
            var numberOfSantas = 3;
            santaCreator.Create(numberOfSantas);
            var visitCreator = new VisitCreator(data);
            visitCreator.Create();

            var wayEvaluator = new TimeEvaluator(data);

            var home = data.HomeIndex;
            var homeAdditional = data.HomeIndexAdditional;

            // normals ways
            Assert.AreEqual(Testdata1.W01, wayEvaluator.Run(home, 0));
            Assert.AreEqual(Testdata1.W12 + Testdata1.Duration1, wayEvaluator.Run(0, 1));
            Assert.AreEqual(Testdata1.W20 + Testdata1.Duration2, wayEvaluator.Run(1, home));
            Assert.AreEqual(Testdata1.W03, wayEvaluator.Run(home, 2));
            Assert.AreEqual(Testdata1.W30 + Testdata1.Duration3, wayEvaluator.Run(2, home));
            Assert.AreEqual(Testdata1.W04, wayEvaluator.Run(home, 3));
            Assert.AreEqual(Testdata1.W40 + Testdata1.Duration4, wayEvaluator.Run(3, home));
            Assert.AreEqual(Testdata1.W40 + Testdata1.Duration4, wayEvaluator.Run(3, homeAdditional));

            // breaks (day1)
            Assert.AreEqual(Testdata1.W50 + Testdata1.Duration5, wayEvaluator.Run(4, home));
            Assert.AreEqual(Testdata1.W50 + Testdata1.Duration5, wayEvaluator.Run(4, homeAdditional));
            Assert.AreEqual(Testdata1.W05, wayEvaluator.Run(home, 4));
            Assert.AreEqual(Testdata1.W05, wayEvaluator.Run(homeAdditional, 4));
            Assert.AreEqual(Testdata1.W45 + Testdata1.Duration4, wayEvaluator.Run(3, 4));

            // breaks (day2)
            Assert.AreEqual(Testdata1.W50 + Testdata1.Duration5, wayEvaluator.Run(5, home));
            Assert.AreEqual(Testdata1.W50 + Testdata1.Duration5, wayEvaluator.Run(5, homeAdditional));
            Assert.AreEqual(Testdata1.W05, wayEvaluator.Run(home, 5));
            Assert.AreEqual(Testdata1.W05, wayEvaluator.Run(homeAdditional, 5));
            Assert.AreEqual(Testdata1.W45 + Testdata1.Duration4, wayEvaluator.Run(3, 5));

            // between home
            Assert.AreEqual(0, wayEvaluator.Run(home, home));
            Assert.AreEqual(0, wayEvaluator.Run(home, homeAdditional));
            Assert.AreEqual(0, wayEvaluator.Run(homeAdditional, home));
            Assert.AreEqual(0, wayEvaluator.Run(homeAdditional, homeAdditional));
        }
    }
}