using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Tests.Algorithm;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Tests
{
    [TestClass()]
    public class RoutingSolverTests
    {
        [TestMethod()]
        public void TestSolve_Simple()
        {
            var input = Testdataset1.Create();
            var solver = new RoutingSolver(input, new RoutingSolverStarterData
            {
                MaxNumberOfSantas = 2,
            });

            var actual = solver.Solve(10, null, null);

            Assert.IsTrue(actual.IsValid(), actual.Validate());
        }

        [TestMethod()]
        public void TestSolve_AdditionalSanta()
        {
            var input = Testdataset1.Create();
            var solver = new RoutingSolver(input, new RoutingSolverStarterData
            {
                MaxNumberOfSantas = 3,
            });

            var actual = solver.Solve(10, null, null);

            Assert.IsTrue(actual.IsValid(), actual.Validate());
        }
    }
}