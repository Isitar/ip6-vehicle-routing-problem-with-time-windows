using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Tests.Algorithm;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Tests
{
    [TestClass()]
    public class RoutingSolverTests
    {
        [TestMethod()]
        public void TestSolve_Simple()
        {
            var input = TestDataset1.Create();
            var solver = new RoutingSolver(input, new RoutingSolverStarterData(2, SolvingMode.Default));

            var actual = solver.Solve(10, null, null);

            CheckValid(actual);
            CheckAllVisited(actual);
            CheckNoWrongBreaks(actual);
            CheckNoAdditionalSantas(actual);
        }

        [TestMethod()]
        public void TestSolve_AdditionalSanta()
        {
            var input = TestDataset1.Create();
            var solver = new RoutingSolver(input, new RoutingSolverStarterData(3, SolvingMode.Default));

            var actual = solver.Solve(10, null, null);

            CheckValid(actual);
            CheckAllVisited(actual);
            CheckNoWrongBreaks(actual);
            CheckNoAdditionalSantas(actual);
        }

        private void CheckValid(OptimizationResult actual)
        {
            Assert.IsTrue(actual.IsValid(), actual.Validate());
        }

        private void CheckAllVisited(OptimizationResult actual)
        {
            Assert.AreEqual(0, actual.NumberOfNotVisitedFamilies());
        }

        private void CheckNoWrongBreaks(OptimizationResult actual)
        {
            // SantaId2 has break with visitId=4
            var wrongBreaks = actual.NonEmptyRoutes.Any(r => r.SantaId != TestDataset1.SantaId2 && r.Waypoints.Any(wp => wp.VisitId == 4));
            Assert.IsFalse(wrongBreaks, "there are breaks in the wrong route");
        }

        private void CheckNoAdditionalSantas(OptimizationResult actual)
        {
            Assert.AreEqual(0, actual.NumberOfAdditionalSantas());
        }
    }
}