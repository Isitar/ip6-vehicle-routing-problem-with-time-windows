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
        public SolverInputData GetModel()
        {
            bool[][,] santas = {
                new bool[,] {
                    { true, true },
                    { true, true },
                    { true, true },
                    { true, true },
                    { true, true },
                    { true, true },
                },
                new bool[,] {
                    { true, true, true },
                    { true, true, true },
                    { true, true, true },
                    { true, true, true },
                    { true, true, true },
                    { true, true, true },
                },
            };

            VisitState d = VisitState.Default;
            VisitState n = VisitState.NotAvailable;
            VisitState[][,] visits = {
                new VisitState[,] {
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                    { d, d },
                },
                new VisitState[,] {
                    { d, d, d },
                    { d, d, d },
                    { d, d, d },
                    { d, d, d },
                    { d, d, d },
                    { d, d, d },
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
                1, 2, 3, 1, 2, 3,
            };

            return new SolverInputData(santas, visitLength, visits, 5, distances);
        }

        [TestMethod]
        public void TestSolve()
        {
            var model = GetModel();
            var actual = Starter.Optimise(model);
            var expected = new Route(model.Santas[0].GetLength(0))
            {
                Waypoints = new List<int>[] {
                    new List<int>()
                    {
                        0, 4, 5,
                    },
                    new List<int>()
                    {
                        0, 3, 1, 2,
                    },
                }
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestModel()
        {
            Assert.IsTrue(GetModel().IsValid());
        }

    }
}
