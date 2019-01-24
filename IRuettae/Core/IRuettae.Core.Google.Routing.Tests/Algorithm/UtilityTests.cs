using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.Google.Routing.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Tests.Algorithm;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Google.Routing.Algorithm;

namespace IRuettae.Core.Google.Routing.Tests.Algorithm
{
    [TestClass()]
    public class UtilityTests
    {
        [TestMethod()]
        public void TestGetRealDesiredLength()
        {
            var data = new RoutingData(TestDataset1.Create());

            var durationDay1 = TestDataset1.EndDay1 - TestDataset1.StartDay1;
            var durationDay2 = TestDataset1.EndDay2 - TestDataset1.StartDay2;
            var tests = new List<(int expected, (int from, int to) desired)>
            {
                (0,(TestDataset1.StartDay1,TestDataset1.StartDay1)),
                (durationDay1,(TestDataset1.StartDay1,TestDataset1.EndDay1)),
                (durationDay1,(TestDataset1.StartDay1,TestDataset1.EndDay1+100)),
                (durationDay1-100,(TestDataset1.StartDay1,TestDataset1.EndDay1-100)),
                (durationDay1,(TestDataset1.StartDay1-100,TestDataset1.EndDay1)),
                (durationDay1-100,(TestDataset1.StartDay1+100,TestDataset1.EndDay1)),
                (Math.Max(durationDay1, durationDay2),(TestDataset1.StartDay1,TestDataset1.EndDay2)),
                (100,(TestDataset1.StartDay2+100,TestDataset1.StartDay2+200)),
            };


            foreach (var (expected, desired) in tests)
            {
                Assert.AreEqual(expected, Routing.Algorithm.Utility.GetRealDesiredLength(data, desired));
            }
        }
    }
}