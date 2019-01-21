using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Tests.Algorithm;
using IRuettae.Core.Google.Routing.Algorithm;

namespace IRuettae.Core.Google.Routing.Tests.Models
{
    [TestClass()]
    public class RoutingDataTests
    {
        private RoutingData CreateRoutingData()
        {
            var data = Testdataset1.Create();
            var numberOfSantas = 3;
            new SantaCreator(data).Create(numberOfSantas);
            new VisitCreator(data).Create();
            new UnavailableCreator(data).Create();
            new StartEndCreator(data).Create();
            return data;
        }

        [TestMethod()]
        public void TestNumberOfSantas()
        {
            Assert.AreEqual(6, CreateRoutingData().NumberOfSantas);
        }

        [TestMethod()]
        public void TestNumberOfVisits()
        {
            // 4 normal
            // 2 breaks
            // 2 homes
            Assert.AreEqual(8, CreateRoutingData().NumberOfVisits);
        }

        [TestMethod()]
        public void TestOverallStart()
        {
            Assert.AreEqual(Testdataset1.StartDay1, CreateRoutingData().OverallStart);
        }

        [TestMethod()]
        public void TestOverallEnd()
        {
            Assert.AreEqual(Testdataset1.EndDay2, CreateRoutingData().OverallEnd);
        }
    }
}