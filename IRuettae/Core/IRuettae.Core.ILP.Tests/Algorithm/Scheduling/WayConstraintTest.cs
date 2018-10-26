using System;
using IRuettae.Core.ILP.Algorithm;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.ILP.Tests.Algorithm.Scheduling
{
    [TestClass]
    public class WayConstraintTest
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
                    {t, t, t, t, t, t},
                }
            };

            VisitState d = VisitState.Default;
            VisitState[][,] visits =
            {
                new[,]
                {
                    {d, d, d, d, d, d},
                    {d, d, d, d, d, d},
                    {d, d, d, d, d, d},
                },

            };

            //var X = int.MaxValue;
            int[,] distances =
            {
                {0, 1, 1},
                {1, 0, 2},
                {1, 2, 0},
            };

            int[] visitLength =
            {
                0, 1, 1,
            };

            return new SolverInputData(santas, visitLength, visits, distances, new[] { 0, 1, 2 }, new[] { 0 });
        }


        [TestMethod]
        public void TestRespectingWay()
        {
            var model = GetModel();
            var calculatedRoute = Starter.Optimize(model);
            Assert.IsNotNull(calculatedRoute);

            var waypoints = calculatedRoute.Waypoints[0, 0];
            Assert.AreEqual(4, waypoints.Count);
            Assert.AreEqual(0, waypoints[0].Visit);

            // Order may change
            if (waypoints[1].Visit == 1)
            {
                Assert.AreEqual(1, waypoints[1].Visit);
                Assert.AreEqual(2, waypoints[2].Visit);
            }
            else
            {
                Assert.AreEqual(2, waypoints[1].Visit);
                Assert.AreEqual(1, waypoints[2].Visit);
            }

            Assert.AreEqual(-1, waypoints[0].StartTime);
            Assert.AreEqual(1, waypoints[1].StartTime);
            Assert.AreEqual(4, waypoints[2].StartTime);

        }
    }
}
