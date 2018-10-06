using System;
using System.Collections.Generic;
using IRuettae.Persistence.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Converter.Tests
{
    [TestClass]
    public class PersistenceToCoreConverterTest
    {
        [TestMethod]
        public void TestSimpleExample()
        {
            // example with 4 visits and 2 santas. no breaks, starting 6. december 2017
            // desired from 18:00 - 19:00, unavailable from 20:00 - 22:00 1 working day from 17:00 until 22:00


            var v1 = new Visit
            {
                City = "Lenzburg",
                DeltaWayDistance = 0,
                DeltaWayDuration = 0,
                Desired = new List<Period>
                {
                    new Period
                    {
                        Start = new DateTime(2017, 12, 6, 18, 0, 0),
                        End = new DateTime(2017, 12, 6, 19, 0, 0)
                    }
                },
                Unavailable = new List<Period>
                {
                    new Period
                    {
                        Start = new DateTime(2017, 12, 6, 20, 0, 0),
                        End = new DateTime(2017, 12, 6, 22, 0, 0)
                    }
                },
                NumberOfChildren = 2,
                VisitType = VisitType.Visit,
                Year = 2017
            };

            var v2 = new Visit
            {
                City = "Lenzburg",
                DeltaWayDistance = 0,
                DeltaWayDuration = 0,
                Desired = new List<Period>
                {
                    new Period
                    {
                        Start = new DateTime(2017, 12, 6, 18, 0, 0),
                        End = new DateTime(2017, 12, 6, 19, 0, 0)
                    }
                },
                Unavailable = new List<Period>
                {
                    new Period
                    {
                        Start = new DateTime(2017, 12, 6, 20, 0, 0),
                        End = new DateTime(2017, 12, 6, 22, 0, 0)
                    }
                },
                NumberOfChildren = 2,
                VisitType = VisitType.Visit,
                Year = 2017
            };

            var v3 = new Visit
            {
                City = "Lenzburg",
                DeltaWayDistance = 0,
                DeltaWayDuration = 0,
                Desired = new List<Period>
                {
                    new Period
                    {
                        Start = new DateTime(2017, 12, 6, 18, 0, 0),
                        End = new DateTime(2017, 12, 6, 19, 0, 0)
                    }
                },
                Unavailable = new List<Period>
                {
                    new Period
                    {
                        Start = new DateTime(2017, 12, 6, 20, 0, 0),
                        End = new DateTime(2017, 12, 6, 22, 0, 0)
                    }
                },
                NumberOfChildren = 2,
                VisitType = VisitType.Visit,
                Year = 2017
            };

            var v4 = new Visit
            {
                City = "Lenzburg",
                DeltaWayDistance = 0,
                DeltaWayDuration = 0,
                Desired = new List<Period>
                {
                    new Period
                    {
                        Start = new DateTime(2017, 12, 6, 18, 0, 0),
                        End = new DateTime(2017, 12, 6, 19, 0, 0)
                    }
                },
                Unavailable = new List<Period>
                {
                    new Period
                    {
                        Start = new DateTime(2017, 12, 6, 20, 0, 0),
                        End = new DateTime(2017, 12, 6, 22, 0, 0)
                    }
                },
                NumberOfChildren = 2,
                VisitType = VisitType.Visit,
                Year = 2017
            };

            v1.FromWays = new List<Way>(4)
            {
                new Way() {Distance = 0, Duration = 0, From = v1, To = v1},
                new Way() {Distance = 10, Duration = 20, From = v1, To = v2},
                new Way() {Distance = 11, Duration = 21, From = v1, To = v3},
                new Way() {Distance = 12, Duration = 22, From = v1, To = v4},
            };

            v1.ToWays = new List<Way>(4)
            {
                new Way() {Distance = 0, Duration = 0, To = v1, From = v1},
                new Way() {Distance = 10, Duration = 20, To = v1, From = v2},
                new Way() {Distance = 11, Duration = 21, To = v1, From = v3},
                new Way() {Distance = 12, Duration = 22, To = v1, From = v4},
            };

            var visits = new List<Visit>
            {
                
            };
        }
    }
}
