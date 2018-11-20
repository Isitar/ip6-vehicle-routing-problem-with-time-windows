using System;
using IRuettae.Core.ILP.Algorithm;
using System.Linq;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.ILP.Algorithm.Models;

namespace IRuettae.Core.ILP.Tests.Algorithm.Scheduling
{
    [TestClass]
    public class TryDesiredTargetTest
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
                }
            };

            VisitState d = VisitState.Default;
            VisitState X = VisitState.Desired;
            VisitState[][,] visits =
            {
                new[,]
                {
                    {d, d, d, d, d, d, d, d},
                    {d, X, d, d, d, d, d, d},
                    {d, d, d, d, X, d, d, d},
                },

            };

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

            return new SolverInputData(santas, visitLength, visits, distances, new[] { 0, 1, 2 }, new[] { 0 }, new int[0], 1, new[] { 0 });
        }


        //[TestMethod]
        //public void TestTryDesiredOnly()
        //{
        //    var model = GetModel();
        //    var solver = new SchedulingILPSolver(model, SchedulingOptimizationGoals.TryDesiredOnly);
        //    var resultState = solver.Solve(0, 60000);
        //    var result = solver.GetResult();
            

        //    var wp = result.Waypoints[0, 0];

        //    Assert.AreEqual(1, wp[1].StartTime);
        //    Assert.AreEqual(4, wp[2].StartTime);

        //    var firstWaypoint = wp.First();
        //    var lastWaypoint = wp.Last();
        //    var duration = lastWaypoint.StartTime - firstWaypoint.StartTime;

        //    // one longer than neccessary
        //    Assert.AreEqual(7, duration);
        //}
    }
}
