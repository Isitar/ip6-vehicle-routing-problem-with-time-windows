using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.LocalSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.LocalSolver.Tests
{
    [TestClass()]
    public class SolverTests
    {
        [TestMethod]
        public void TestRouteCostsCorrect()
        {
            //   B -1- A
            //   2|  /3
            //    |/
            //    H

            const int hour = 3600;
            var input = new OptimizationInput
            {
                Visits = new[]
                {
                    new Visit
                    {
                        Id = 0,
                        Duration = 4 * hour,
                        Desired = new (int from, int to)[0],
                        Unavailable = new (int from, int to)[0],
                        WayCostToHome = 3*hour,
                        WayCostFromHome = 3*hour
                    },
                    new Visit
                    {
                        Id = 1,
                        Duration = 5*hour,
                        Desired = new (int from, int to)[0],
                        Unavailable = new (int from, int to)[0],
                        WayCostToHome = 2*hour,
                        WayCostFromHome = 2*hour,
                    },
                },
                Santas = new[] { new Santa { Id = 0 }, },
                Days = new (int from, int to)[] { (0, 100 * hour) },
                RouteCosts = new[,]
                {
                    {0, hour},
                    {hour, 0},
                },
            };

            var solver = new Solver(input);
            var output = solver.Solve(3000L, null, null);
            Assert.IsNotNull(output);
            Assert.IsNotNull(output.Routes);
            Assert.IsTrue(output.NonEmptyRoutes.Any());

            Assert.AreEqual(1050, output.Cost());
        }

        [TestMethod]
        public void TestRouteDesiredCorrect()
        {
            //   B -1- A
            //   2|  /3
            //    |/
            //    H

            const int hour = 3600;
            var input = new OptimizationInput
            {
                Visits = new[]
                {
                    new Visit
                    {
                        Id = 0, //A
                        Duration = 4 * hour,
                        Desired = new (int from, int to)[0],
                        Unavailable = new (int from, int to)[0],
                        WayCostToHome = 3*hour,
                        WayCostFromHome = 3*hour
                    },
                    new Visit
                    {
                        Id = 1, // B
                        Duration = 5*hour,
                        Desired = new (int from, int to)[] {(2*hour,4*hour)},
                        Unavailable = new (int from, int to)[0],
                        WayCostToHome = 2*hour,
                        WayCostFromHome = 2*hour,
                    },
                },
                Santas = new[] { new Santa { Id = 0 }, },
                Days = new (int from, int to)[] { (0, 100 * hour) },
                RouteCosts = new[,]
                {
                    {0, hour},
                    {hour, 0},
                },
            };

            var solver = new Solver(input);
            var output = solver.Solve(3000L, null, null);
            Assert.IsNotNull(output);
            Assert.IsNotNull(output.Routes);
            Assert.AreEqual(1, output.NonEmptyRoutes.Count());
            Assert.AreEqual(1010, output.Cost());
            Assert.AreEqual(1, output.Routes[0].Waypoints.Skip(1).First().VisitId);
            Assert.AreEqual(0, output.Routes[0].Waypoints.First().StartTime);
            Assert.AreEqual(2*hour, output.Routes[0].Waypoints[1].StartTime);
        }
    }
}