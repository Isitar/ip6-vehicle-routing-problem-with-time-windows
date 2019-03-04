using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILPIp5Gurobi.Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.ILPIp5Gurobi.Tests.Algorithm
{
    [TestClass]
    public class RouteTest
    {
        [TestMethod]
        public void TestEquals()
        {
            var r1 = new Route(2, 2);
            {
                r1.Waypoints[0, 0].Add(new Waypoint(1, 1));
                r1.Waypoints[0, 0].Add(new Waypoint(2, 1));
                r1.Waypoints[0, 1].Add(new Waypoint(3, 1));
                r1.Waypoints[0, 1].Add(new Waypoint(4, 1));
                r1.Waypoints[1, 0].Add(new Waypoint(5, 1));
                r1.Waypoints[1, 0].Add(new Waypoint(6, 1));
                r1.Waypoints[1, 1].Add(new Waypoint(7, 1));
                r1.Waypoints[1, 1].Add(new Waypoint(8, 1));
            }
            var r2 = new Route(2, 2);
            {
                r2.Waypoints[0, 0].Add(new Waypoint(1, 1));
                r2.Waypoints[0, 0].Add(new Waypoint(2, 1));
                r2.Waypoints[0, 1].Add(new Waypoint(3, 1));
                r2.Waypoints[0, 1].Add(new Waypoint(4, 1));
                r2.Waypoints[1, 0].Add(new Waypoint(5, 1));
                r2.Waypoints[1, 0].Add(new Waypoint(6, 1));
                r2.Waypoints[1, 1].Add(new Waypoint(7, 1));
                r2.Waypoints[1, 1].Add(new Waypoint(8, 1));
            }

            Assert.IsTrue(r1.Equals(r2));
        }

        [TestMethod]
        public void TestEquals_NotEqual()
        {
            var r1 = new Route(2, 2);
            {
                r1.Waypoints[0, 0].Add(new Waypoint(1, 1));
                r1.Waypoints[0, 0].Add(new Waypoint(2, 1));
                r1.Waypoints[0, 1].Add(new Waypoint(3, 1));
                r1.Waypoints[0, 1].Add(new Waypoint(4, 1));
                r1.Waypoints[1, 0].Add(new Waypoint(5, 1));
                r1.Waypoints[1, 0].Add(new Waypoint(6, 1));
                r1.Waypoints[1, 1].Add(new Waypoint(7, 1));
                r1.Waypoints[1, 1].Add(new Waypoint(8, 1));
            }
            var r2 = new Route(2, 2);
            {
                r1.Waypoints[0, 0].Add(new Waypoint(1, 1));
                r1.Waypoints[0, 0].Add(new Waypoint(2, 1));
                r1.Waypoints[0, 1].Add(new Waypoint(3, 1));
                r1.Waypoints[0, 1].Add(new Waypoint(4, 1));
                r1.Waypoints[1, 0].Add(new Waypoint(5, 1));
                r1.Waypoints[1, 0].Add(new Waypoint(6, 1));
                r1.Waypoints[1, 1].Add(new Waypoint(7, 1));
                r1.Waypoints[1, 1].Add(new Waypoint(8, 1000000));
            }

            Assert.IsFalse(r1.Equals(r2));
        }

        [TestMethod]
        public void TestEquals_Empty()
        {
            var r1 = new Route(2, 2);
            var r2 = new Route(2, 2);

            Assert.IsTrue(r1.Equals(r2));
        }
    }
}
