using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.Manual.Tests
{
    [TestClass()]
    public class ManualSolverTests
    {
        // used ways
        private const int w01 = 10;
        private const int w12 = 20;
        private const int w20 = 30;
        private const int w03 = 40;
        private const int w30 = 50;
        private const int w04 = 60;
        private const int w40 = 70;

        // durations
        private const int duration1 = 100;
        private const int duration2 = 200;
        private const int duration3 = 300;
        private const int duration4 = 400;

        // days
        private const int startDay1 = 1000;
        private const int endDay1 = 2000;
        private const int startDay2 = 5000;
        private const int endDay2 = 6000;

        private const int hour = 3600;

        private OptimizationInput GetInput()
        {
            return new OptimizationInput()
            {
                Santas = new Santa[]
                {
                    new Santa
                    {
                        Id = 100,
                    },
                    new Santa
                    {
                        Id = 200,
                    },
                },
                Visits = new Visit[]
                {
                    new Visit
                    {
                        Id = 0,
                        Duration = duration1,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = w01,
                        WayCostToHome = 0,
                    },
                    new Visit
                    {
                        Id = 1,
                        Duration = duration2,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = 0,
                        WayCostToHome = w20,
                    },
                    new Visit
                    {
                        Id = 2,
                        Duration = duration3,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = w03,
                        WayCostToHome = w30,
                    },
                    new Visit
                    {
                        Id = 3,
                        Duration = duration4,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = w04,
                        WayCostToHome = w40,
                    },
                    new Visit // will not be visited
                    {
                        Id = 4,
                        Duration = 100 * hour,
                        IsBreak = true,
                        SantaId = 200,
                    }
                },
                Days = new(int, int)[]
                {
                    (startDay1, endDay1),
                    (startDay2, startDay2),
                },
                RouteCosts = new int[,]
                {
                    // only w12 is really used
                    {0, w12, hour, hour, hour},
                    {hour, 0, hour, hour, hour},
                    {hour, hour, 0,hour, hour},
                    {hour, hour, hour, 0, hour},
                    {hour, hour, hour, hour, 0},
                },
            };
        }

        [TestMethod()]
        public void TestSolve()
        {
            var config = new ManualConfig()
            {
                Routes = new[]{
                    (100, 0, new[] {0, 1}),
                    (200, 0, new int[] {}),
                    (201, 0, new int[] {}),
                    (100, 1, new[] {2}),
                    (200, 1, new int[] {}),
                    (201, 1, new[] {3}),
                }
            };
            var actual = new ManualSolver(GetInput(), config).Solve(0, null, null).Routes;

            Assert.AreEqual(6, actual.Length);

            // route 1
            Assert.AreEqual(4, actual[0].Waypoints.Length);
            Assert.AreEqual(100, actual[0].SantaId);
            Assert.AreEqual(new Waypoint() { VisitId = -1, StartTime = startDay1 }, actual[0].Waypoints[0]);
            Assert.AreEqual(new Waypoint() { VisitId = 0, StartTime = startDay1 + w01 }, actual[0].Waypoints[1]);
            Assert.AreEqual(new Waypoint() { VisitId = 1, StartTime = startDay1 + w01 + duration1 + w12 }, actual[0].Waypoints[2]);
            Assert.AreEqual(new Waypoint() { VisitId = -1, StartTime = startDay1 + w01 + duration1 + w12 + duration2 + w20 }, actual[0].Waypoints[3]);

            // route 2
            Assert.AreEqual(0, actual[1].Waypoints.Length);
            Assert.AreEqual(200, actual[1].SantaId);

            // route 3
            Assert.AreEqual(0, actual[2].Waypoints.Length);
            Assert.AreNotEqual(100, actual[2].SantaId);
            Assert.AreNotEqual(200, actual[2].SantaId);

            // route 4
            Assert.AreEqual(3, actual[3].Waypoints.Length);
            Assert.AreEqual(100, actual[3].SantaId);
            Assert.AreEqual(new Waypoint() { VisitId = -1, StartTime = startDay2 }, actual[3].Waypoints[0]);
            Assert.AreEqual(new Waypoint() { VisitId = 2, StartTime = startDay2 + w03 }, actual[3].Waypoints[1]);
            Assert.AreEqual(new Waypoint() { VisitId = -1, StartTime = startDay2 + w03 + duration3 + w30 }, actual[3].Waypoints[2]);

            // route 5
            Assert.AreEqual(0, actual[4].Waypoints.Length);
            Assert.AreEqual(200, actual[4].SantaId);

            // route 6
            Assert.AreEqual(3, actual[5].Waypoints.Length);
            Assert.AreNotEqual(100, actual[5].SantaId);
            Assert.AreNotEqual(200, actual[5].SantaId);
            Assert.AreEqual(new Waypoint() { VisitId = -1, StartTime = startDay2 }, actual[5].Waypoints[0]);
            Assert.AreEqual(new Waypoint() { VisitId = 3, StartTime = startDay2 + w04 }, actual[5].Waypoints[1]);
            Assert.AreEqual(new Waypoint() { VisitId = -1, StartTime = startDay2 + w04 + duration4 + w40 }, actual[5].Waypoints[2]);
        }
    }
}