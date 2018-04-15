using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.Test.Algorithm
{
    [TestClass]
    public class StarterTest
    {
        [TestMethod]
        public void TestSolve()
        {
            // example from: http://www.or.deis.unibo.it/algottm/files/8_ATSP.pdf
            var inf = int.MaxValue;
            int[,] distances = new int[6, 6] {
                { 0, inf, inf, 2, 1, inf },
                { 7, 0, 1, inf, inf, inf },
                { 6, 5, 0, 3, 6, inf },
                { 5, 3, 9, 0, 7, inf },
                { inf, inf, 8, inf, 0, 1 },
                { 3, inf, inf, 7, 9, 0 },
            };

            var actual = Starter.Optimise(distances);
            var expected = new Route()
            {
                Waypoints = new List<int>()
                {
                    0, 3, 1, 2, 4, 5,
                }
            };

            Assert.AreEqual(expected, actual);
        }
    }
}
