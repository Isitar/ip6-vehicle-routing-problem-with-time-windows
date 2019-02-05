using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Tests
{
    [TestClass()]
    public class UtilityTests
    {
        [TestMethod()]
        public void TestIntersectionLength()
        {
            Assert.AreEqual(5, Utility.IntersectionLength(10, 20, 15, 20));
            Assert.AreEqual(5, Utility.IntersectionLength(15, 20, 10, 20));
            Assert.AreEqual(1, Utility.IntersectionLength(-10, 20, 1, 2));
            Assert.AreEqual(0, Utility.IntersectionLength(-20, -10, 1, 2));
            Assert.AreEqual(0, Utility.IntersectionLength(1, 1, 1, 2));
            Assert.AreEqual(10, Utility.IntersectionLength(10, 20, 10, 20));
            Assert.AreEqual(0, Utility.IntersectionLength(20, 10, 0, 50));
        }
    }
}