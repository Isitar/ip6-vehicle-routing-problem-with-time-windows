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


            var input = new OptimizationInput()
            {
                Visits = new[]
                {
                    new Visit()
                    {
                        Id = 0,
                        Duration = 4,
                        Desired = new (int from, int to)[0],
                        Unavailable = new (int from, int to)[0],
                        WayCostToHome = 3,
                        WayCostFromHome = 3
                    },
                    new Visit()
                    {
                        Id = 1,
                        Duration = 5,
                        Desired = new (int from, int to)[0],
                        Unavailable = new (int from, int to)[0],
                        WayCostToHome = 2,
                        WayCostFromHome = 2,
                    },
                },
                Santas = new[] { new Santa { Id = 0 }, },
                Days = new (int from, int to)[] { (0, 100) },
                RouteCosts = new[,]
                {
                    {0, 1},
                    {1, 0},
                },
            };

            var solver = new Solver(input);
            var output = solver.Solve(1000L, null, null);
            Assert.IsNotNull(output);
        }
    }
}