﻿using System;
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
            var r1 = new Route(2);
            r1.Waypoints[0].Add(1);
            r1.Waypoints[0].Add(2);
            r1.Waypoints[1].Add(3);
            r1.Waypoints[1].Add(4);
            var r2 = new Route(2);
            r2.Waypoints[0].Add(1);
            r2.Waypoints[0].Add(2);
            r2.Waypoints[1].Add(3);
            r2.Waypoints[1].Add(4);

            Assert.IsTrue(r1.Equals(r2));
        }
    }
}
