using System;
using IRuettae.Core.ILP.Algorithm;
using System.Linq;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.ILP.Algorithm.Models;

namespace IRuettae.Core.ILP.Tests.Algorithm.Scheduling
{
    [TestClass]
    public class MultipleSantaTest
    {
        public SolverInputData GetModel()
        {
            const bool t = true;
            //const bool f = false;
            bool[][,] santas =
            {
                new[,]
                {
                    {t, t, t, t, t, t, t, t},
                    {t, t, t, t, t, t, t, t},
                }
            };

            VisitState d = VisitState.Default;
            VisitState[][,] visits =
            {
                new[,]
                {
                    {d, d, d, d, d, d, d, d},
                    {d, d, d, d, d, d, d, d},
                    {d, d, d, d, d, d, d, d},
                },

            };

            //var X = int.MaxValue;
            int[,] distances =
            {
                {0, 1, 1},
                {1, 0, 10},
                {1, 10, 0},
            };

            int[] visitLength =
            {
                0, 1, 1,
            };

            return new SolverInputData(santas, visitLength, visits, distances, new[] { 0, 1, 2 }, new[] { 0, 1 });
        }


        [TestMethod]
        public void TestMultipleSanta()
        {
            var model = GetModel();
            var result = Starter.Optimize(model, SchedulingOptimizationGoals.MinTimeOnly);

            Assert.AreEqual(2, result.Waypoints.GetLength(0));
            Assert.AreEqual(3, result.Waypoints[0, 0].Count);
            Assert.AreEqual(3, result.Waypoints[1, 0].Count);
        }
    }
}
