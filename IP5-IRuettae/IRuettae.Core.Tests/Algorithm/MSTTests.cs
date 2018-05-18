using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverInputData = IRuettae.Core.Algorithm.NoTimeSlicing.SolverInputData;

namespace IRuettae.Core.Tests.Algorithm
{
    [TestClass]
    public class MSTTests
    {
        [TestMethod]
        public void TestConnectedGraph()
        {
            bool[,] santas = new bool[1, 1] { { true } };
            int[] visitsDuration = new int[5] { 1, 1, 1, 1, 1 };
            VisitState[,] visitStates = new VisitState[1, 5] {
                {
                    VisitState.Default, VisitState.Default,VisitState.Default,VisitState.Default,VisitState.Default
                }
            };

            int[,] distances = new int[5, 5]
            {
                {0, 2, 4, 5, 5},
                {2, 0, 2, 3, 3},
                {4, 2, 0, 1, 1},
                {5, 3, 1, 0, 1},
                {5, 3, 1, 1, 0}
            };

            int[] dayDuration = new int[1] {12};

            var solverInputData = new SolverInputData(santas, visitsDuration, visitStates, distances, dayDuration);
            var result = Starter.Optimise(solverInputData);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Waypoints[0,0].Count);
            
        }

    }
}
