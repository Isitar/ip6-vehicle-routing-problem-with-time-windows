using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Tests.Algorithm
{
    [TestClass()]
    public class SantaCreatorTests
    {
        private const int santaId1 = 100;
        private const int santaId2 = 200;

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

        private RoutingData CreateRoutingData()
        {
            return new RoutingData(new OptimizationInput()
            {
                Santas = new Santa[]
                {
                    new Santa
                    {
                        Id = santaId1,
                    },
                    new Santa
                    {
                        Id = santaId2,
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
                    new Visit
                    {
                        Id = 4,
                        Duration = 100 * hour,
                        IsBreak = true,
                        SantaId = santaId2,
                    }
                },
                Days = new(int, int)[]
                {
                    (startDay1, endDay1),
                    (startDay2, startDay2),
                },
                RouteCosts = new int[,]
                {
                    {0, w12, hour, hour, hour},
                    {hour, 0, hour, hour, hour},
                    {hour, hour, 0,hour, hour},
                    {hour, hour, hour, 0, hour},
                    {hour, hour, hour, hour, 0},
                },
            });
        }

        [TestMethod()]
        public void TestCreate_Simple()
        {
            var actual = CreateRoutingData();
            var santaCreator = new SantaCreator(actual);

            var numberOfSantas = 2;
            santaCreator.Create(numberOfSantas);

            Assert.AreEqual(numberOfSantas, actual.SantaIds.Count);
            Assert.AreEqual(santaId1, actual.SantaIds[0]);
            Assert.AreEqual(santaId2, actual.SantaIds[1]);
        }

        [TestMethod()]
        public void TestCreate_Additional()
        {
            var actual = CreateRoutingData();
            var santaCreator = new SantaCreator(actual);

            var numberOfSantas = 3;
            santaCreator.Create(numberOfSantas);

            Assert.AreEqual(numberOfSantas, actual.SantaIds.Count);
            Assert.AreEqual(santaId1, actual.SantaIds[0]);
            Assert.AreEqual(santaId2, actual.SantaIds[1]);
            Assert.AreEqual(santaId2 + 1, actual.SantaIds[2]);
        }
    }
}