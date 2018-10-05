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

            var visits = new List<Visit>
            {
                
            };
        }
    }
}
