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

        [TestMethod()]
        public void TestCreate_Simple()
        {
            var actual = Testdata1.Create();
            var santaCreator = new SantaCreator(actual);

            var numberOfDays = actual.Input.Days.Length;
            var numberOfSantas = 2;
            santaCreator.Create(numberOfSantas);

            Assert.AreEqual(numberOfDays * numberOfSantas, actual.SantaIds.Length);
            Assert.AreEqual(Testdata1.SantaId1, actual.SantaIds[0]);
            Assert.AreEqual(Testdata1.SantaId2, actual.SantaIds[1]);
            Assert.AreEqual(Testdata1.SantaId1, actual.SantaIds[2]);
            Assert.AreEqual(Testdata1.SantaId2, actual.SantaIds[3]);
        }

        [TestMethod()]
        public void TestCreate_Additional()
        {
            var actual = Testdata1.Create();
            var santaCreator = new SantaCreator(actual);

            var numberOfDays = actual.Input.Days.Length;
            var numberOfSantas = 3;
            santaCreator.Create(numberOfSantas);

            Assert.AreEqual(numberOfDays * numberOfSantas, actual.SantaIds.Length);
            Assert.AreEqual(Testdata1.SantaId1, actual.SantaIds[0]);
            Assert.AreEqual(Testdata1.SantaId2, actual.SantaIds[1]);
            Assert.AreEqual(Testdata1.SantaId2 + 1, actual.SantaIds[2]);
            Assert.AreEqual(Testdata1.SantaId1, actual.SantaIds[3]);
            Assert.AreEqual(Testdata1.SantaId2, actual.SantaIds[4]);
            Assert.AreEqual(Testdata1.SantaId2 + 1, actual.SantaIds[5]);
        }
    }
}