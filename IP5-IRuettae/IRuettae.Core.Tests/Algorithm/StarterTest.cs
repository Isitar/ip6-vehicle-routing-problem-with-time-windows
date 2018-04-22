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

            var inf = int.MaxValue;
            int[,] distances = {
                { inf, 1, inf, 1, inf},
                { inf, inf, 1, inf, inf},
                { 1, inf, inf, inf, inf},
                { inf, inf, inf, 1, inf},
                { 1, inf, inf, inf, inf},
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
                Waypoints = new List<int>[,] {
                    { // santa 1
                        new List<int>()
                        { // day 1
                            0, 1, 2,
                        },
                        new List<int>()
                        { // day 2
                            0, 3, 4,
                        },
                    },
                    { // santa 2
                        new List<int>()
                        { // day 1
                        },
                        new List<int>()
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
