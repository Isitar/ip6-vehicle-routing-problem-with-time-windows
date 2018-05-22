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

            //     5 - 4
            //     \_3_/
            //       |
            //       |
            //       2
            //       |
            //       |
            //       1

            int[] dayDuration = new int[1] { 17 };

            var solverInputData = new SolverInputData(santas, visitsDuration, visitStates, distances, dayDuration);
            var result = Starter.Optimise(solverInputData);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Waypoints[0, 0].Count);

        }

        [TestMethod]
        public void TestConnectedMultipleSantaGraph()
        {
            const int numberOfDays = 2;
            const int numberOfVisit = 9;
            const int numberOfSantas = 1;
            bool[,] santas = new bool[numberOfDays, numberOfSantas] { { true }, { true } };
            int[] visitsDuration = new int[numberOfVisit] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            VisitState[,] visitStates = new VisitState[numberOfDays, numberOfVisit] {
                {VisitState.Default, VisitState.Default,VisitState.Default,VisitState.Default,VisitState.Default,VisitState.Default, VisitState.Default,VisitState.Default,VisitState.Default},
                {VisitState.Default, VisitState.Default,VisitState.Default,VisitState.Default,VisitState.Default,VisitState.Default, VisitState.Default,VisitState.Default,VisitState.Default}
            };

            int[,] distances = new int[numberOfVisit, numberOfVisit]
            {
                {0, 2, 4, 5, 5, 2, 4, 5, 5}, // v0
                {2, 0, 2, 3, 3, 4, 6, 7, 7},// v1
                {4, 2, 0, 1, 1, 6, 8, 9, 9},// v2
                {5, 3, 1, 0, 1, 7, 9, 10,10},// v3
                {5, 3, 1, 1, 0, 7, 9, 10,10},// v4
                {2, 4, 6, 7, 7, 0, 2, 3, 3},// v5
                {4, 6, 8, 9, 9, 2, 0, 1, 1},// v6
                {5, 7, 9,10,10, 3, 1, 0, 1},// v7
                {5, 7, 9,10,10, 3, 1, 1, 0},// v8
            };

            //     4 - 3
            //     \_2_/
            //       |
            //       |
            //       1
            //       |
            //       |
            //       0 V0
            //       |
            //       |
            //       5
            //       |
            //       |
            //      _6_
            //     /   \
            //     8 - 7

            int[] dayDuration = new int[numberOfDays] { 17, 17 };

            var solverInputData = new SolverInputData(santas, visitsDuration, visitStates, distances, dayDuration);
            var result = Starter.Optimise(solverInputData);
            Assert.IsNotNull(result);
            //Assert.AreEqual(5, result.Waypoints[0, 0].Count);

        }

    }
}
