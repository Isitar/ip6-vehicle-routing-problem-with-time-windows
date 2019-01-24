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
            var input = TestDataset1.Create();
            var numberOfDays = input.Days.Length;

            var numberOfSantas = 2;
            var actual = Converter.Convert(input, numberOfSantas);

            Assert.AreEqual(numberOfDays * numberOfSantas, actual.SantaIds.Length);
            Assert.AreEqual(TestDataset1.SantaId1, actual.SantaIds[0]);
            Assert.AreEqual(TestDataset1.SantaId2, actual.SantaIds[1]);
            Assert.AreEqual(TestDataset1.SantaId1, actual.SantaIds[2]);
            Assert.AreEqual(TestDataset1.SantaId2, actual.SantaIds[3]);
        }

        [TestMethod()]
        public void TestConvert_Additional()
        {
            var input = TestDataset1.Create();
            var actual = Converter.Convert(input, NumberOfSantas);

            CheckSantas(actual);
            CheckVisits(actual);
            CheckUnavailable(actual);
            CheckDesired(actual);
            CheckStartEnd(actual);
            CheckNumberOfSantas(actual);
            CheckNumberOfVisits(actual);
            CheckOverallStart(actual);
            CheckOverallEnd(actual);
            CheckGetDayFromSanta(actual);
        }

        private void CheckSantas(RoutingData actual)
        {
            Assert.AreEqual(TestDataset1.NumberOfDays * NumberOfSantas, actual.SantaIds.Length);
            Assert.AreEqual(TestDataset1.SantaId1, actual.SantaIds[0]);
            Assert.AreEqual(TestDataset1.SantaId2, actual.SantaIds[1]);
            Assert.AreEqual(TestDataset1.SantaId2 + 1, actual.SantaIds[2]);
            Assert.AreEqual(TestDataset1.SantaId1, actual.SantaIds[3]);
            Assert.AreEqual(TestDataset1.SantaId2, actual.SantaIds[4]);
            Assert.AreEqual(TestDataset1.SantaId2 + 1, actual.SantaIds[5]);
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
            Assert.AreEqual(1, actual.Visits[4].Desired.Length);
            Assert.AreEqual(TestDataset1.StartDay1 + TestDataset1.BreakDesiredStart, actual.Visits[4].Desired[0].from);
            Assert.AreEqual(TestDataset1.StartDay1 + TestDataset1.BreakDesiredEnd, actual.Visits[4].Desired[0].to);

            // test break on day2
            Assert.AreEqual(4, actual.Visits[5].SantaId);
            Assert.AreEqual(1, actual.Visits[5].Desired.Length);
            Assert.AreEqual(TestDataset1.StartDay2 + TestDataset1.BreakDesiredStart, actual.Visits[5].Desired[0].from);
            Assert.AreEqual(TestDataset1.StartDay2 + TestDataset1.BreakDesiredEnd, actual.Visits[5].Desired[0].to);

            // homes
            var home0 = 6;
            var home1 = 7;
            Assert.AreEqual(-1, actual.Visits[home0].Id);
            Assert.AreEqual(-1, actual.Visits[home1].Id);
            Assert.AreEqual(home0, actual.HomeIndex[0]);
            Assert.AreEqual(home1, actual.HomeIndex[1]);

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
                Assert.AreEqual(TestDataset1.StartDay1 - TestDataset1.Duration1, from);
                Assert.AreEqual(TestDataset1.EndDay1, to);
            }

            // visit2
            {
                Assert.AreEqual(4, actual.Unavailable[1].Length);
                var (from, to) = actual.Unavailable[1].Last();
                Assert.AreEqual(TestDataset1.StartDay2 - TestDataset1.Duration2, from);
                Assert.AreEqual(TestDataset1.EndDay2, to);
            }

            // normal visits have default unavailables
            for (int i = 0; i < actual.Unavailable.Length; i++)
            {
                if (actual.HomeIndex.Contains(i) || actual.Visits[i].IsBreak)
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
                Assert.AreEqual(TestDataset1.StartDay1 - 1, to);
            }
            {
                var (from, to) = unavailable[1];
                Assert.AreEqual(TestDataset1.EndDay1 + 1 - duration, from);
                Assert.AreEqual(TestDataset1.StartDay2 - 1, to);
            }
            {
                var (from, to) = unavailable[2];
                Assert.AreEqual(TestDataset1.EndDay2 + 1 - duration, from);
                Assert.AreEqual(int.MaxValue, to);
            }
        }

        private void CheckDesired(RoutingData actual)
        {
            Assert.AreEqual(actual.Visits.Length, actual.Desired.Length);

            // visit 1
            {
                // desired outside business hours must be removed
                Assert.AreEqual(0, actual.Desired[0].Length);
            }

            // visit2
            {
                Assert.AreEqual(1, actual.Desired[1].Length);
                var (from, to) = actual.Desired[1].Last();
                Assert.AreEqual(TestDataset1.StartDay1, from);
                Assert.AreEqual(TestDataset1.EndDay1 - TestDataset1.Duration2, to);
            }

            // visit3
            {
                Assert.AreEqual(1, actual.Desired[2].Length);
                var (from, to) = actual.Desired[2].Last();
                Assert.IsTrue(from < to);
                Assert.AreEqual(TestDataset1.StartDay1 - TestDataset1.Duration3 / 2, from);
                Assert.AreEqual(TestDataset1.StartDay1, to);
            }

            // visit5, day1
            {
                Assert.AreEqual(1, actual.Desired[4].Length);

                var expectedFrom = TestDataset1.StartDay1 + TestDataset1.BreakDesiredEnd - TestDataset1.Duration5;
                var expectedTo = TestDataset1.StartDay1 + TestDataset1.BreakDesiredStart;
                var (from, to) = actual.Desired[4][0];
                Assert.IsTrue(from < to);
                Assert.AreEqual(expectedFrom, from);
                Assert.AreEqual(expectedTo, to);
            }

            // visit5, day2
            {
                Assert.AreEqual(1, actual.Desired[5].Length);

                var expectedFrom = TestDataset1.StartDay2 + TestDataset1.BreakDesiredEnd - TestDataset1.Duration5;
                var expectedTo = TestDataset1.StartDay2 + TestDataset1.BreakDesiredStart;
                var (from, to) = actual.Desired[5][0];
                Assert.IsTrue(from < to);
                Assert.AreEqual(expectedFrom, from);
                Assert.AreEqual(expectedTo, to);
            }
        }

        private void CheckStartEnd(RoutingData actual)
        {
            var length = actual.SantaIds.Length;
            Assert.AreEqual(length, actual.SantaStartIndex.Length);
            Assert.AreEqual(length, actual.SantaEndIndex.Length);

            // starts/ends are the same
            var home0 = actual.HomeIndex[0];
            var home1 = actual.HomeIndex[1];
            var expected = new int[]
            {
                home0,home0,home0,
                home1,home1,home1,
            };
            Enumerable.SequenceEqual(expected, actual.SantaStartIndex);
            Enumerable.SequenceEqual(expected, actual.SantaEndIndex);
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
            Assert.AreEqual(TestDataset1.StartDay1, actual.OverallStart);
        }

        public void CheckOverallEnd(RoutingData actual)
        {
            Assert.AreEqual(TestDataset1.EndDay2, actual.OverallEnd);
        }

        private void CheckGetDayFromSanta(RoutingData actual)
        {
            // first 3 santas are on first day
            Assert.AreEqual(0, actual.GetDayFromSanta(0));
            Assert.AreEqual(0, actual.GetDayFromSanta(1));
            Assert.AreEqual(0, actual.GetDayFromSanta(2));

            // next 3 santas are on second day
            Assert.AreEqual(1, actual.GetDayFromSanta(3));
            Assert.AreEqual(1, actual.GetDayFromSanta(4));
            Assert.AreEqual(1, actual.GetDayFromSanta(5));
        }
    }
}
