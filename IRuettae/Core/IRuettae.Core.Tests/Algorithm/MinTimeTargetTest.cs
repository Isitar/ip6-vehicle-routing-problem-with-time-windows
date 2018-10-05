using System;
using IRuettae.Core.ILP.Algorithm;
using System.Linq;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.Tests.Algorithm
{
    [TestClass]
    public class MinTimeTargetTest
    {
        public SolverInputData GetModel()
        {
            // 1 Santa, 5 timeslots
            // A <-> B : 1
            // A <-> C : 1
            // B <-> C : 1


            // Visit duration:
            // A : 0
            // B : 1
            // C : 1


            const bool t = true;
            //const bool f = false;
            bool[][,] santas =
            {
                new[,]
                {
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
                {1, 0, 1},
                {1, 2, 0},
            };

            int[] visitLength =
            {
                0, 1, 1,
            };

            return new SolverInputData(santas, visitLength, visits, distances, new[] { 0L, 1L, 2L }, new[] { 0L }, new[] { DateTime.Now });
        }


        [TestMethod]
        public void TestMinTimeOnly()
        {
            var model = GetModel();
            var result = Starter.Optimise(model, TargetBuilderType.MinTimeOnly);

            var firstWaypoint = result.Waypoints[0, 0].First();
            var lastWaypoint = result.Waypoints[0, 0].Last();
            var duration = lastWaypoint.StartTime - firstWaypoint.StartTime;

            Assert.AreEqual(6, duration);
        }
    }
}
