using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing.Algorithm;
using System;
using System.Linq;
using System.Text;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Tests.Algorithm
{
    [TestClass()]
    public class ConverterTests
    {
        private const int NumberOfSantas = 3;

        [TestMethod()]
        public void TestCreate_Simple()
        {
            var input = Testdataset1.Create();
            var numberOfDays = input.Days.Length;

            var numberOfSantas = 2;
            var actual = Converter.Convert(input, numberOfSantas);

            Assert.AreEqual(numberOfDays * numberOfSantas, actual.SantaIds.Length);
            Assert.AreEqual(Testdataset1.SantaId1, actual.SantaIds[0]);
            Assert.AreEqual(Testdataset1.SantaId2, actual.SantaIds[1]);
            Assert.AreEqual(Testdataset1.SantaId1, actual.SantaIds[2]);
            Assert.AreEqual(Testdataset1.SantaId2, actual.SantaIds[3]);
        }

        [TestMethod()]
        public void TestConvert_Additional()
        {
            var input = Testdataset1.Create();
            var actual = Converter.Convert(input, NumberOfSantas);

            CheckSantas(actual);
            CheckVisits(actual);
            CheckUnavailable(actual);
            CheckStartEnd(actual);
            CheckNumberOfSantas(actual);
            CheckNumberOfVisits(actual);
            CheckOverallStart(actual);
            CheckOverallEnd(actual);
        }

        private void CheckSantas(RoutingData actual)
        {
            Assert.AreEqual(Testdataset1.NumberOfDays * NumberOfSantas, actual.SantaIds.Length);
            Assert.AreEqual(Testdataset1.SantaId1, actual.SantaIds[0]);
            Assert.AreEqual(Testdataset1.SantaId2, actual.SantaIds[1]);
            Assert.AreEqual(Testdataset1.SantaId2 + 1, actual.SantaIds[2]);
            Assert.AreEqual(Testdataset1.SantaId1, actual.SantaIds[3]);
            Assert.AreEqual(Testdataset1.SantaId2, actual.SantaIds[4]);
            Assert.AreEqual(Testdataset1.SantaId2 + 1, actual.SantaIds[5]);
        }

        private void CheckVisits(RoutingData actual)
        {
            // 4 normal
            // 2 breaks
            // 2 homes
            Assert.AreEqual(8, actual.Visits.Length);
            for (int i = 0; i < actual.Input.Visits.Length; i++)
            {
                Assert.AreEqual(actual.Input.Visits[i].Id, actual.Visits[i].Id);
            }

            // test break on day1
            Assert.AreEqual(1, actual.Visits[4].SantaId);
            Assert.AreEqual(int.MinValue, actual.Visits[4].Unavailable[0].from);
            Assert.AreEqual(Testdataset1.StartDay1 - 1, actual.Visits[4].Unavailable[0].to);
            Assert.AreEqual(Testdataset1.EndDay1 + 1, actual.Visits[4].Unavailable[1].from);
            Assert.AreEqual(int.MaxValue, actual.Visits[4].Unavailable[1].to);
            Assert.AreEqual(1, actual.Visits[4].Desired.Length);
            Assert.AreEqual(Testdataset1.StartDay1 + Testdataset1.BreakDesiredStart, actual.Visits[4].Desired[0].from);
            Assert.AreEqual(Testdataset1.StartDay1 + Testdataset1.BreakDesiredEnd, actual.Visits[4].Desired[0].to);

            // test break on day2
            Assert.AreEqual(4, actual.Visits[5].SantaId);
            Assert.AreEqual(int.MinValue, actual.Visits[5].Unavailable[0].from);
            Assert.AreEqual(Testdataset1.StartDay2 - 1, actual.Visits[5].Unavailable[0].to);
            Assert.AreEqual(Testdataset1.EndDay2 + 1, actual.Visits[5].Unavailable[1].from);
            Assert.AreEqual(int.MaxValue, actual.Visits[5].Unavailable[1].to);
            Assert.AreEqual(1, actual.Visits[5].Desired.Length);
            Assert.AreEqual(Testdataset1.StartDay2 + Testdataset1.BreakDesiredStart, actual.Visits[5].Desired[0].from);
            Assert.AreEqual(Testdataset1.StartDay2 + Testdataset1.BreakDesiredEnd, actual.Visits[5].Desired[0].to);

            // home 1
            var home1Index = 6;
            Assert.AreEqual(-1, actual.Visits[home1Index].Id);
            Assert.AreEqual(home1Index, actual.HomeIndex);

            // home 2
            var home2Index = 7;
            Assert.AreEqual(-1, actual.Visits[home2Index].Id);
            Assert.AreEqual(home2Index, actual.HomeIndexAdditional);

            // desired/unavailable must not be null
            foreach (var visit in actual.Visits)
            {
                Assert.IsNotNull(visit.Desired);
                Assert.IsNotNull(visit.Unavailable);
            }
        }

        private void CheckUnavailable(RoutingData actual)
        {
            Assert.AreEqual(actual.Visits.Length, actual.Unavailable.Length);

            // visit 1
            {
                Assert.AreEqual(4, actual.Unavailable[0].Length);
                var (from, to) = actual.Unavailable[0].Last();
                Assert.AreEqual(Testdataset1.StartDay1 - Testdataset1.Duration1, from);
                Assert.AreEqual(Testdataset1.EndDay1, to);
            }

            // visit2
            {
                Assert.AreEqual(4, actual.Unavailable[1].Length);
                var (from, to) = actual.Unavailable[1].Last();
                Assert.AreEqual(Testdataset1.StartDay2 - Testdataset1.Duration2, from);
                Assert.AreEqual(Testdataset1.EndDay2, to);
            }

            // normal visits have default unavailables
            for (int i = 0; i < actual.Unavailable.Length; i++)
            {
                if (i == actual.HomeIndex || i == actual.HomeIndexAdditional || actual.Visits[i].IsBreak)
                {
                    // home / breaks
                    continue;
                }

                CheckUnavailableDefault(actual.Unavailable[i], actual.Visits[i].Duration);
            }

        }

        private void CheckUnavailableDefault((int startFrom, int startEnd)[] unavailable, int duration)
        {
            {
                var (from, to) = unavailable[0];
                Assert.AreEqual(int.MinValue, from);
                Assert.AreEqual(Testdataset1.StartDay1 - 1, to);
            }
            {
                var (from, to) = unavailable[1];
                Assert.AreEqual(Testdataset1.EndDay1 + 1 - duration, from);
                Assert.AreEqual(Testdataset1.StartDay2 - 1, to);
            }
            {
                var (from, to) = unavailable[2];
                Assert.AreEqual(Testdataset1.EndDay2 + 1 - duration, from);
                Assert.AreEqual(int.MaxValue, to);
            }

        }

        private void CheckStartEnd(RoutingData actual)
        {
            var length = actual.SantaIds.Length;
            Assert.AreEqual(length, actual.SantaStartIndex.Length);
            Assert.AreEqual(length, actual.SantaEndIndex.Length);

            var home = actual.HomeIndex;
            var homeAdditional = actual.HomeIndexAdditional;

            // starts
            var expectedStarts = new int[]
            {
                home,home,homeAdditional,
                home,home,homeAdditional,
            };
            Enumerable.SequenceEqual(expectedStarts, actual.SantaStartIndex);

            // ends
            var expectedEnds = new int[]
            {
                home,home,home,
                home,home,home,
            };
            Enumerable.SequenceEqual(expectedEnds, actual.SantaEndIndex);
        }

        public void CheckNumberOfSantas(RoutingData actual)
        {
            Assert.AreEqual(6, actual.NumberOfSantas);
        }

        public void CheckNumberOfVisits(RoutingData actual)
        {
            // 4 normal
            // 2 breaks
            // 2 homes
            Assert.AreEqual(8, actual.NumberOfVisits);
        }

        public void CheckOverallStart(RoutingData actual)
        {
            Assert.AreEqual(Testdataset1.StartDay1, actual.OverallStart);
        }

        public void CheckOverallEnd(RoutingData actual)
        {
            Assert.AreEqual(Testdataset1.EndDay2, actual.OverallEnd);
        }
    }
}
