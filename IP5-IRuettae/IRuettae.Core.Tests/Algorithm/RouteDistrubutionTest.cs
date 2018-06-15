using System;
using System.Collections.Generic;
using IRuettae.Core.Algorithm;
using IRuettae.Core.Algorithm.RouteDistribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.Tests.Algorithm
{
    [TestClass]
    public class RouteDistributionTest
    {
        [TestMethod]
        public void TestSolve()
        {
            const int santas = 2;
            const int days = 2;

            // [santa][route,day] cost of route
            var input = new Core.Algorithm.RouteDistribution.SolverInputData
            {
                RouteCost = new int[santas][,]
            };
            input.RouteCost[0] = new int[,]
            {
                { 3, 1 },
                { 1, 3 },
                { 5, 5 },
                { 5, 5 },
            };
            input.RouteCost[1] = new int[,]
            {
                { 5, 5 },
                { 5, 5 },
                { 1, 1 },
                { 2, 5 },
            };

            // [santa,day] route that is used
            var expected = new RouteDistribution(santas, days)
            {
                Distribution = new int?[,]
                {
                    { 1, 0 },
                    { 3, 2 },
                }
            };

            var solver = new Solver(input);
            Assert.AreEqual(ResultState.Optimal, solver.Solve());
            var actual = solver.GetResult();

            Assert.AreEqual(expected, actual);
        }
    }
}
