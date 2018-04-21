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
            int numberOfSantas = 3;

            VisitState d = VisitState.Default;
            VisitState n = VisitState.NotAvailable;
            VisitState[][,] visits = {
                new VisitState[6, 2] {
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                },
                new VisitState[6, 2] {
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                },
            };

            // example from: http://www.or.deis.unibo.it/algottm/files/8_ATSP.pdf
            var inf = int.MaxValue;
            int[,] distances = {
                { 0, inf, inf, 2, 1, inf },
                { 7, 0, 1, inf, inf, inf },
                { 6, 5, 0, 3, 6, inf },
                { 5, 3, 9, 0, 7, inf },
                { inf, inf, 8, inf, 0, 1 },
                { 3, inf, inf, 7, 9, 0 },
            };

            int[] visitLength =
            {
                1,
                2,
                3,
                1,
                2,
                3,
            };

            var actual = Starter.Optimise(new SolverInputData(numberOfSantas, visitLength, visits, 5, distances));
            var expected = new Route(numberOfSantas)
            {
                Waypoints = new List<int>[2] {
                    new List<int>()
                {
                    0, 3, 1, 2, 4, 5,
                },new List<int>()
                {
                    0, 3, 1, 2, 4, 5,
                }
                }
            };

            Assert.AreEqual(expected, actual);
        }
    }
}
