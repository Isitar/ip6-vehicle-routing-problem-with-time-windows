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
            var data = Testdataset1.Create();
            var numberOfSantas = 3;
            new SantaCreator(data).Create(numberOfSantas);
            new VisitCreator(data).Create();

            var wayEvaluator = new TimeEvaluator(data);

            var home = data.HomeIndex;
            var homeAdditional = data.HomeIndexAdditional;

            // normals ways
            Assert.AreEqual(Testdataset1.W01, wayEvaluator.Run(home, 0));
            Assert.AreEqual(Testdataset1.W12 + Testdataset1.Duration1, wayEvaluator.Run(0, 1));
            Assert.AreEqual(Testdataset1.W20 + Testdataset1.Duration2, wayEvaluator.Run(1, home));
            Assert.AreEqual(Testdataset1.W03, wayEvaluator.Run(home, 2));
            Assert.AreEqual(Testdataset1.W30 + Testdataset1.Duration3, wayEvaluator.Run(2, home));
            Assert.AreEqual(Testdataset1.W04, wayEvaluator.Run(home, 3));
            Assert.AreEqual(Testdataset1.W40 + Testdataset1.Duration4, wayEvaluator.Run(3, home));
            Assert.AreEqual(Testdataset1.W40 + Testdataset1.Duration4, wayEvaluator.Run(3, homeAdditional));

            // breaks (day1)
            Assert.AreEqual(Testdataset1.W50 + Testdataset1.Duration5, wayEvaluator.Run(4, home));
            Assert.AreEqual(Testdataset1.W50 + Testdataset1.Duration5, wayEvaluator.Run(4, homeAdditional));
            Assert.AreEqual(Testdataset1.W05, wayEvaluator.Run(home, 4));
            Assert.AreEqual(Testdataset1.W05, wayEvaluator.Run(homeAdditional, 4));
            Assert.AreEqual(Testdataset1.W45 + Testdataset1.Duration4, wayEvaluator.Run(3, 4));

            // breaks (day2)
            Assert.AreEqual(Testdataset1.W50 + Testdataset1.Duration5, wayEvaluator.Run(5, home));
            Assert.AreEqual(Testdataset1.W50 + Testdataset1.Duration5, wayEvaluator.Run(5, homeAdditional));
            Assert.AreEqual(Testdataset1.W05, wayEvaluator.Run(home, 5));
            Assert.AreEqual(Testdataset1.W05, wayEvaluator.Run(homeAdditional, 5));
            Assert.AreEqual(Testdataset1.W45 + Testdataset1.Duration4, wayEvaluator.Run(3, 5));

            // between home
            Assert.AreEqual(0, wayEvaluator.Run(home, home));
            Assert.AreEqual(0, wayEvaluator.Run(home, homeAdditional));
            Assert.AreEqual(0, wayEvaluator.Run(homeAdditional, home));
            Assert.AreEqual(0, wayEvaluator.Run(homeAdditional, homeAdditional));
        }
    }
}