﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Tests.Algorithm;

namespace IRuettae.Core.Google.Routing.Algorithm.Tests
{
    [TestClass()]
    public class VisitCreatorTests
    {
        [TestMethod()]
        public void TestCreate_Simple()
        {
            var actual = Dataset1.Create();
            var santaCreator = new SantaCreator(actual);
            var numberOfSantas = 3;
            santaCreator.Create(numberOfSantas);
            var visitCreator = new VisitCreator(actual);

            var numberOfDays = actual.Input.Days.Length;
            visitCreator.Create();

            Assert.AreEqual(6, actual.Visits.Count);
            for (int i = 0; i < actual.Input.Visits.Length; i++)
            {
                Assert.AreEqual(actual.Input.Visits[i].Id, actual.Visits[i].Id);
            }

            // test break on day1
            Assert.AreEqual(1, actual.Visits[4].SantaId);
            Assert.AreEqual(int.MinValue, actual.Visits[4].Unavailable[0].from);
            Assert.AreEqual(Dataset1.StartDay1 - 1, actual.Visits[4].Unavailable[0].to);
            Assert.AreEqual(Dataset1.EndDay1 + 1, actual.Visits[4].Unavailable[1].from);
            Assert.AreEqual(int.MaxValue, actual.Visits[4].Unavailable[1].to);
            Assert.AreEqual(1, actual.Visits[4].Desired.Length);
            Assert.AreEqual(Dataset1.StartDay1 + Dataset1.BreakDesiredStart, actual.Visits[4].Desired[0].from);
            Assert.AreEqual(Dataset1.StartDay1 + Dataset1.BreakDesiredEnd, actual.Visits[4].Desired[0].to);

            // test break on day2
            Assert.AreEqual(4, actual.Visits[5].SantaId);
            Assert.AreEqual(int.MinValue, actual.Visits[5].Unavailable[0].from);
            Assert.AreEqual(Dataset1.StartDay2 - 1, actual.Visits[5].Unavailable[0].to);
            Assert.AreEqual(Dataset1.EndDay2 + 1, actual.Visits[5].Unavailable[1].from);
            Assert.AreEqual(int.MaxValue, actual.Visits[5].Unavailable[1].to);
            Assert.AreEqual(1, actual.Visits[5].Desired.Length);
            Assert.AreEqual(Dataset1.StartDay2 + Dataset1.BreakDesiredStart, actual.Visits[5].Desired[0].from);
            Assert.AreEqual(Dataset1.StartDay2 + Dataset1.BreakDesiredEnd, actual.Visits[5].Desired[0].to);
        }
    }
}