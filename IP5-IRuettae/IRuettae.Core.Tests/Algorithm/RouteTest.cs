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
            var r1 = new Route(2, 2);
            {
                r1.Waypoints[0, 0].Add(1);
                r1.Waypoints[0, 0].Add(2);
                r1.Waypoints[0, 1].Add(3);
                r1.Waypoints[0, 1].Add(4);
                r1.Waypoints[1, 0].Add(5);
                r1.Waypoints[1, 0].Add(6);
                r1.Waypoints[1, 1].Add(7);
                r1.Waypoints[1, 1].Add(8);
            }
            var r2 = new Route(2, 2);
            {
                r2.Waypoints[0, 0].Add(1);
                r2.Waypoints[0, 0].Add(2);
                r2.Waypoints[0, 1].Add(3);
                r2.Waypoints[0, 1].Add(4);
                r2.Waypoints[1, 0].Add(5);
                r2.Waypoints[1, 0].Add(6);
                r2.Waypoints[1, 1].Add(7);
                r2.Waypoints[1, 1].Add(8);
            }

            Assert.IsTrue(r1.Equals(r2));
        }
    }
}
