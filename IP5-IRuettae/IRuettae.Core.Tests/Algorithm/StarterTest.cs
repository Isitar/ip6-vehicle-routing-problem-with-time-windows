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
            var t = true;
            bool[][,] santas = {
                new bool[,] {
                    { t, t, t, t, t, t },
                    { t, t, t, t, t, t },
                },
                new bool[,] {
                    { t, t, t, t, t, t, t },
                    { t, t, t, t, t, t, t },
                },
            };

            VisitState d = VisitState.Default;
            //VisitState n = VisitState.NotAvailable;
            VisitState[][,] visits = {
                new VisitState[,] {
                    { d, d, d, d, d, d },
                    { d, d, d, d, d, d },
                    { d, d, d, d, d, d },
                    { d, d, d, d, d, d },
                    { d, d, d, d, d, d },
                },
                new VisitState[,] {
                    { d, d, d, d, d, d, d },
                    { d, d, d, d, d, d, d },
                    { d, d, d, d, d, d, d },
                    { d, d, d, d, d, d, d },
                    { d, d, d, d, d, d, d },
                },
            };

            var X = int.MaxValue;
            int[,] distances = {
                { 0, 1, 1, 1, 1},
                { 1, 0, 1, X, X},
                { 1, X, 0, X, X},
                { 1, X, X, 0, 1},
                { 1, X, X, X, 0},
            };

            int[] visitLength =
            {
                0, 1, 2, 2, 2,
            };

            return new SolverInputData(santas, visitLength, visits, 5, distances);
        }

        [TestMethod]
        public void TestSolve()
        {
            var model = GetModel();
            var actual = Starter.Optimise(model);
            var expected = new Route(model.Santas.Length, model.Santas[0].GetLength(0))
            {
                Waypoints = new List<Waypoint>[,] {
                    { // santa 1
                        new List<Waypoint>()
                        { // day 1
                            new Waypoint { visit = 0, startTime = 0 },
                            new Waypoint { visit = 1, startTime = 2 },
                            new Waypoint { visit = 2, startTime = 4 },
                            new Waypoint { visit = 0, startTime = 7 },
                        },
                        new List<Waypoint>()
                        { // day 2
                            new Waypoint { visit = 0, startTime = 0 },
                            new Waypoint { visit = 3, startTime = 2 },
                            new Waypoint { visit = 4, startTime = 5 },
                            new Waypoint { visit = 0, startTime = 7 },
                        },
                    },
                    { // santa 2
                        new List<Waypoint>()
                        { // day 1
                        },
                        new List<Waypoint>()
                        { // day 2
                        },
                    }
                }
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestModel()
        {
            Assert.IsTrue(GetModel().IsValid());
        }

        [TestMethod]
        public void TestIsFeasible()
        {
            var model = GetModel();
            // shouldn't throw an exception
            Starter.Optimise(model);
        }

    }
}
