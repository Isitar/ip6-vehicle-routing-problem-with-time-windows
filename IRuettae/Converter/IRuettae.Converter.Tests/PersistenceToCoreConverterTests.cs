using IRuettae.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Converter.Tests
{
    [TestClass]
    public class PersistenceToCoreConverterTests
    {
        private static bool Equals(Core.Models.OptimisationInput v1, Core.Models.OptimisationInput v2)
        {
            if (v1.Visits.Length != v2.Visits.Length)
            {
                return false;
            }
            bool equals = true;
            for (int i = 0; equals && i < v1.Visits.Length; i++)
            {
                equals = true
                     && v1.Visits[i].Id == v2.Visits[i].Id
                     && v1.Visits[i].Duration == v2.Visits[i].Duration
                     && v1.Visits[i].WayCostFromHome == v2.Visits[i].WayCostFromHome
                     && v1.Visits[i].WayCostToHome == v2.Visits[i].WayCostToHome
                     && v1.Visits[i].Desired.SequenceEqual(v2.Visits[i].Desired)
                     && v1.Visits[i].Unavailable.SequenceEqual(v2.Visits[i].Unavailable)
                     ;
            }
            return equals
                && v1.Santas.SequenceEqual(v2.Santas)
                && v1.Days.SequenceEqual(v2.Days)
                && v1.RouteCosts.Cast<int>().SequenceEqual(v2.RouteCosts.Cast<int>())
                ;
        }

        [TestMethod()]
        public void ConvertTest()
        {
            // test with two days, two visits, two santas without breaks

            var workingDays = new List<(DateTime Start, DateTime End)>
            {
                (new DateTime(2017, 12, 08, 17, 0, 0), new DateTime(2017, 12, 08, 22, 0, 0)),
                (new DateTime(2017, 12, 09, 17, 0, 0), new DateTime(2017, 12, 09, 22, 0, 0)),
            };
            var startVisit = new Persistence.Entities.Visit
            {
                Id = 0,
            };
            var visits = new List<Persistence.Entities.Visit>
            {
                new Persistence.Entities.Visit()
                {
                    Id = 1,
                    Duration = 60,
                    Desired = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period()
                        {
                            Start = new DateTime(2017, 12, 08, 18, 0, 0),
                            End = new DateTime(2017, 12, 08, 19, 0, 0),
                        }
                    },
                    Unavailable = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period()
                        {
                            Start = new DateTime(2017, 12, 09, 17, 0, 0),
                            End = new DateTime(2017, 12, 09, 22, 0, 0),
                        }
                    }
                },
                new Persistence.Entities.Visit()
                {
                    Id = 3,
                    Duration = 15,
                    Unavailable = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period()
                        {
                            Start = new DateTime(2017, 12, 08, 17, 0, 0),
                            End = new DateTime(2017, 12, 08, 22, 0, 0),
                        },
                        new Persistence.Entities.Period()
                        {
                            Start = new DateTime(2017, 12, 09, 20, 0, 0),
                            End = new DateTime(2017, 12, 09, 22, 0, 0),
                        },
                    }
                }
            };
            var santas = new List<Persistence.Entities.Santa>
            {
                new Persistence.Entities.Santa()
                {
                    Name = "Mr Santa Test"
                }
            };
            List<Persistence.Entities.Visit> breaks = null;

            // ways
            var w01 = new Persistence.Entities.Way()
            {
                From = startVisit,
                To = visits[0],
                Duration = 200
            };
            var w02 = new Persistence.Entities.Way()
            {
                From = startVisit,
                To = visits[1],
                Duration = 400
            };
            var w10 = new Persistence.Entities.Way()
            {
                From = visits[0],
                To = startVisit,
                Duration = 300
            };
            var w20 = new Persistence.Entities.Way()
            {
                From = visits[1],
                To = startVisit,
                Duration = 500
            };
            var w12 = new Persistence.Entities.Way()
            {
                From = visits[0],
                To = visits[1],
                Duration = 600
            };
            var w21 = new Persistence.Entities.Way()
            {
                From = visits[1],
                To = visits[0],
                Duration = 700
            };

            // add ways to visits
            {
                startVisit.ToWays = new List<Persistence.Entities.Way>()
                {
                    w01,
                    w02,
                };
                startVisit.FromWays = new List<Persistence.Entities.Way>()
                {
                    w10,
                    w20,
                };
                visits[0].ToWays = new List<Persistence.Entities.Way>()
                {
                    w10,
                    w12,
                };
                visits[0].FromWays = new List<Persistence.Entities.Way>()
                {
                    w01,
                    w21,
                };
                visits[1].ToWays = new List<Persistence.Entities.Way>()
                {
                    w20,
                    w21,
                };
                visits[1].FromWays = new List<Persistence.Entities.Way>()
                {
                    w02,
                    w12,
                };
            }

            var hour = 3600;
            var expected = new Core.Models.OptimisationInput()
            {
                Santas = new Core.Models.Santa[1]
                {
                    new Core.Models.Santa()
                    {
                        Id = 0,
                    },
                },
                Visits = new Core.Models.Visit[2]
                {
                    new Core.Models.Visit()
                    {
                        Id = 0,
                        Duration = 60,
                        Desired = new (int,int)[1]
                        {
                            (hour,2*hour)
                        },
                        Unavailable = new (int,int)[1]
                        {
                            (24*hour,29*hour)
                        },
                        WayCostFromHome = w01.Duration,
                        WayCostToHome = w10.Duration,
                    },
                    new Core.Models.Visit()
                    {
                        Id = 1,
                        Duration = 15,
                        Desired = new (int,int)[0],
                        Unavailable = new (int,int)[2]
                        {
                            (0*hour,5*hour),
                            (27*hour,29*hour),
                        },
                        WayCostFromHome = w02.Duration,
                        WayCostToHome = w20.Duration,
                    }
                },
                Days = new(int, int)[2]
                {
                    (0*hour,5*hour),
                    (24*hour,29*hour),
                },
                RouteCosts = new int[2, 2]
                {
                    { 0 , w12.Duration},
                    { w21.Duration, 0},
                }
            };
            var actual = PersistenceToCoreConverter.Convert(workingDays, startVisit, visits, santas, breaks);

            Assert.IsTrue(Equals(expected, actual));
        }
    }
}
