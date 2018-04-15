using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.Tests.Algorithm
{
    [TestClass]
    public class RouteTest
    {
        [TestMethod]
        public void TestEquals()
        {
            var r1 = new Route();
            r1.Waypoints.Add(1);
            r1.Waypoints.Add(2);
            var r2 = new Route();
            r2.Waypoints.Add(1);
            r2.Waypoints.Add(2);

            Assert.IsTrue(r1.Equals(r2));
        }
    }
}
