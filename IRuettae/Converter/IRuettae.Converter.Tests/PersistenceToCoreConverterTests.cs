using IRuettae.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Persistence.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Converter.Tests
{
    [TestClass]
    public class PersistenceToCoreConverterTests
    {
        private static bool Equals(Core.Models.OptimizationInput v1, Core.Models.OptimizationInput v2)
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
                         && v1.Visits[i].IsBreak.Equals(v2.Visits[i].IsBreak)
                         && v1.Visits[i].SantaId.Equals(v2.Visits[i].SantaId)
                    ;
            }

            return equals
                   && v1.Santas.SequenceEqual(v2.Santas)
                   && v1.Days.SequenceEqual(v2.Days)
                   && v1.RouteCosts.Cast<int>().SequenceEqual(v2.RouteCosts.Cast<int>())
                ;
        }
        /// <summary>
        /// Checks all fields
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        private static void AssertOptimisationInputEqual(Core.Models.OptimizationInput expected, Core.Models.OptimizationInput actual)
        {

            Assert.AreEqual(expected.Visits.Length, actual.Visits.Length);
            
            for (int i = 0; i < expected.Visits.Length; i++)
            {
                Assert.AreEqual(expected.Visits[i].Id, actual.Visits[i].Id);
                Assert.AreEqual(expected.Visits[i].Duration, actual.Visits[i].Duration);
                Assert.AreEqual(expected.Visits[i].WayCostFromHome, actual.Visits[i].WayCostFromHome);
                Assert.AreEqual(expected.Visits[i].WayCostToHome, actual.Visits[i].WayCostToHome);
                CollectionAssert.AreEqual(expected.Visits[i].Desired, actual.Visits[i].Desired);
                CollectionAssert.AreEqual(expected.Visits[i].Unavailable, actual.Visits[i].Unavailable);
                Assert.AreEqual(expected.Visits[i].IsBreak, actual.Visits[i].IsBreak);
                Assert.AreEqual(expected.Visits[i].SantaId, actual.Visits[i].SantaId);
            }
            
            CollectionAssert.AreEqual(expected.Santas, actual.Santas);
            CollectionAssert.AreEqual(expected.Days, actual.Days);
            CollectionAssert.AreEqual(expected.RouteCosts, actual.RouteCosts);
        }


        [TestMethod]
        public void SimleTestWithoutBreaks()
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
                new Persistence.Entities.Visit
                {
                    Id = 1,
                    Duration = 12,
                    NumberOfChildren = 3,
                    Desired = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period
                        {
                            Start = new DateTime(2017, 12, 08, 18, 0, 0),
                            End = new DateTime(2017, 12, 08, 19, 0, 0),
                        }
                    },
                    Unavailable = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period
                        {
                            Start = new DateTime(2017, 12, 09, 17, 0, 0),
                            End = new DateTime(2017, 12, 09, 22, 0, 0),
                        }
                    }
                },
                new Persistence.Entities.Visit
                {
                    Id = 3,
                    Duration = 15,
                    NumberOfChildren = 1,
                    Unavailable = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period
                        {
                            Start = new DateTime(2017, 12, 08, 17, 0, 0),
                            End = new DateTime(2017, 12, 08, 22, 0, 0),
                        },
                        new Persistence.Entities.Period
                        {
                            Start = new DateTime(2017, 12, 09, 20, 0, 0),
                            End = new DateTime(2017, 12, 09, 22, 0, 0),
                        },
                    }
                }
            };
            var santas = new List<Persistence.Entities.Santa>
            {
                new Persistence.Entities.Santa
                {
                    Name = "Mr Santa Test"
                }
            };
            List<Persistence.Entities.Visit> breaks = null;

            // ways
            var w01 = new Persistence.Entities.Way
            {
                From = startVisit,
                To = visits[0],
                Duration = 200
            };
            var w02 = new Persistence.Entities.Way
            {
                From = startVisit,
                To = visits[1],
                Duration = 400
            };
            var w10 = new Persistence.Entities.Way
            {
                From = visits[0],
                To = startVisit,
                Duration = 300
            };
            var w20 = new Persistence.Entities.Way
            {
                From = visits[1],
                To = startVisit,
                Duration = 500
            };
            var w12 = new Persistence.Entities.Way
            {
                From = visits[0],
                To = visits[1],
                Duration = 600
            };
            var w21 = new Persistence.Entities.Way
            {
                From = visits[1],
                To = visits[0],
                Duration = 700
            };

            // add ways to visits
            {
                startVisit.ToWays = new List<Persistence.Entities.Way>
                {
                    w01,
                    w02,
                };
                startVisit.FromWays = new List<Persistence.Entities.Way>
                {
                    w10,
                    w20,
                };
                visits[0].ToWays = new List<Persistence.Entities.Way>
                {
                    w10,
                    w12,
                };
                visits[0].FromWays = new List<Persistence.Entities.Way>
                {
                    w01,
                    w21,
                };
                visits[1].ToWays = new List<Persistence.Entities.Way>
                {
                    w20,
                    w21,
                };
                visits[1].FromWays = new List<Persistence.Entities.Way>
                {
                    w02,
                    w12,
                };
            }

            var hour = 3600;
            var expected = new Core.Models.OptimizationInput
            {
                Santas = new Core.Models.Santa[1]
                {
                    new Core.Models.Santa
                    {
                        Id = 0,
                    },
                },
                Visits = new Core.Models.Visit[2]
                {
                    new Core.Models.Visit
                    {
                        Id = 0,
                        Duration = 30*60,
                        Desired = new (int, int)[1]
                        {
                            (hour, 2 * hour)
                        },
                        Unavailable = new (int, int)[1]
                        {
                            (24 * hour, 29 * hour)
                        },
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = w01.Duration,
                        WayCostToHome = w10.Duration,
                    },
                    new Core.Models.Visit
                    {
                        Id = 1,
                        Duration = 20*60,
                        Desired = new (int, int)[0],
                        Unavailable = new (int, int)[2]
                        {
                            (0 * hour, 5 * hour),
                            (27 * hour, 29 * hour),
                        },
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = w02.Duration,
                        WayCostToHome = w20.Duration,
                    }
                },
                Days = new (int, int)[2]
                {
                    (0 * hour, 5 * hour),
                    (24 * hour, 29 * hour),
                },
                RouteCosts = new int[2, 2]
                {
                    {0, w12.Duration},
                    {w21.Duration, 0},
                }
            };
            var actual = PersistenceToCoreConverter.Convert(workingDays, startVisit, visits, santas);

            AssertOptimisationInputEqual(expected, actual);
        }

        [TestMethod]
        public void SimleTestWithBreaks()
        {
            // test with two visits, one break and two santas in one day

            var workingDays = new List<(DateTime Start, DateTime End)>
            {
                (new DateTime(2017, 12, 06, 17, 0, 0), new DateTime(2017, 12, 06, 22, 0, 0)),
            };
            var startVisit = new Persistence.Entities.Visit
            {
                Id = 0,
            };


            var santa = new Persistence.Entities.Santa
            {
                Id = 27,
                Name = "Mr Santa Test",
            };

            var santa2 = new Persistence.Entities.Santa
            {
                Id = 22,
                Name = "Mr Santa2 Test",
            };


            var visits = new List<Persistence.Entities.Visit>
            {
                new Persistence.Entities.Visit
                {
                    Id = 1,
                    NumberOfChildren = 3,
                    Desired = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period
                        {
                            Start = new DateTime(2017, 12, 06, 18, 0, 0),
                            End = new DateTime(2017, 12, 06, 19, 0, 0),
                        }
                    },
                    Unavailable = new List<Persistence.Entities.Period>()
                },
                new Persistence.Entities.Visit
                {
                    Id = 3,
                    NumberOfChildren = 4,
                    Unavailable = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period
                        {
                            Start = new DateTime(2017, 12, 06, 18, 0, 0),
                            End = new DateTime(2017, 12, 06, 22, 0, 0),
                        },
                    }
                },
                new Persistence.Entities.Visit
                {
                    Id = 5,
                    Duration = 30 *60,
                    Santa = santa,
                    VisitType = VisitType.Break,
                },
                new Persistence.Entities.Visit
                {
                    Id = 7,
                    Duration = 30 *60,
                    Santa = santa2,
                    VisitType = VisitType.Break,
                }

            };

            var santas = new List<Persistence.Entities.Santa>
            {
                santa,
                santa2
            };

            santa.Breaks = visits.Where(v => v.Id == 5).ToList();
            santa2.Breaks = visits.Where(v => v.Id == 7).ToList();

            // ways
            var w01 = new Persistence.Entities.Way
            {
                From = startVisit,
                To = visits[0],
                Duration = 200
            };
            var w02 = new Persistence.Entities.Way
            {
                From = startVisit,
                To = visits[1],
                Duration = 400
            };
            var w03 = new Persistence.Entities.Way
            {
                From = startVisit,
                To = visits[2],
                Duration = 300
            };
            var w04 = new Persistence.Entities.Way
            {
                From = startVisit,
                To = visits[3],
                Duration = 300
            };

            var w10 = new Persistence.Entities.Way
            {
                From = visits[0],
                To = startVisit,
                Duration = 300
            };
            var w20 = new Persistence.Entities.Way
            {
                From = visits[1],
                To = startVisit,
                Duration = 500
            };
            var w30 = new Persistence.Entities.Way
            {
                From = visits[2],
                To = startVisit,
                Duration = 200
            };
            var w40 = new Persistence.Entities.Way
            {
                From = visits[3],
                To = startVisit,
                Duration = 200
            };


            var w11 = new Persistence.Entities.Way
            {
                From = visits[0],
                To = visits[0],
                Duration = 1
            };
            var w12 = new Persistence.Entities.Way
            {
                From = visits[0],
                To = visits[1],
                Duration = 600
            };
            var w13 = new Persistence.Entities.Way
            {
                From = visits[0],
                To = visits[2],
                Duration = 600
            };
            var w14 = new Persistence.Entities.Way
            {
                From = visits[0],
                To = visits[3],
                Duration = 600
            };


            var w21 = new Persistence.Entities.Way
            {
                From = visits[1],
                To = visits[0],
                Duration = 700
            };
            var w22 = new Persistence.Entities.Way
            {
                From = visits[1],
                To = visits[1],
                Duration = 1
            };
            var w23 = new Persistence.Entities.Way
            {
                From = visits[1],
                To = visits[2],
                Duration = 800
            };
            var w24 = new Persistence.Entities.Way
            {
                From = visits[1],
                To = visits[3],
                Duration = 800
            };


            var w31 = new Persistence.Entities.Way
            {
                From = visits[2],
                To = visits[0],
                Duration = 800
            };
            var w32 = new Persistence.Entities.Way
            {
                From = visits[2],
                To = visits[1],
                Duration = 800
            };
            var w33 = new Persistence.Entities.Way
            {
                From = visits[2],
                To = visits[2],
                Duration = -1
            };
            var w34 = new Persistence.Entities.Way
            {
                From = visits[2],
                To = visits[3],
                Duration = 10
            };


            var w41 = new Persistence.Entities.Way
            {
                From = visits[3],
                To = visits[0],
                Duration = 800
            };
            var w42 = new Persistence.Entities.Way
            {
                From = visits[3],
                To = visits[1],
                Duration = 800
            };
            var w43 = new Persistence.Entities.Way
            {
                From = visits[3],
                To = visits[2],
                Duration = 10
            };
            var w44 = new Persistence.Entities.Way
            {
                From = visits[3],
                To = visits[3],
                Duration = -1
            };

            // add ways to visits
            {
                startVisit.ToWays = new List<Persistence.Entities.Way>
                {
                    w01,
                    w02,
                    w03,
                    w04,
                };
                startVisit.FromWays = new List<Persistence.Entities.Way>
                {
                    w10,
                    w20,
                    w30,
                    w40,
                };
                visits[0].ToWays = new List<Persistence.Entities.Way>
                {
                    w10,
                    w11,
                    w12,
                    w13,
                    w14,
                };
                visits[0].FromWays = new List<Persistence.Entities.Way>
                {
                    w01,
                    w11,
                    w21,
                    w31,
                    w41,
                };
                visits[1].ToWays = new List<Persistence.Entities.Way>
                {
                    w20,
                    w21,
                    w22,
                    w23,
                    w24,
                };
                visits[1].FromWays = new List<Persistence.Entities.Way>
                {
                    w02,
                    w12,
                    w22,
                    w32,
                    w42,
                };

                visits[2].ToWays = new List<Persistence.Entities.Way>
                {
                    w30,
                    w31,
                    w32,
                    w33,
                    w34,
                };
                visits[2].FromWays = new List<Persistence.Entities.Way>
                {
                    w03,
                    w13,
                    w23,
                    w33,
                    w44,
                };

                visits[3].ToWays = new List<Persistence.Entities.Way>
                {
                    w40,
                    w41,
                    w42,
                    w43,
                    w43,
                };
                visits[3].FromWays = new List<Persistence.Entities.Way>
                {
                    w04,
                    w14,
                    w24,
                    w34,
                    w44,
                };
            }

            var hour = 3600;
            var expected = new Core.Models.OptimizationInput
            {
                Santas = new Core.Models.Santa[2]
                {
                    new Core.Models.Santa
                    {
                        Id = 0,
                    },
                    new Core.Models.Santa
                    {
                        Id = 1,
                    },
                },
                Visits = new Core.Models.Visit[4]
                {
                    new Core.Models.Visit
                    {
                        Id = 0,
                        Duration = 30 *60,
                        IsBreak = false,
                        SantaId = -1,
                        Desired = new (int, int)[1]
                        {
                            (1 * hour, 2 * hour)
                        },
                        Unavailable = new (int from, int to)[0],
                        WayCostFromHome = w01.Duration,
                        WayCostToHome = w10.Duration,
                    },
                    new Core.Models.Visit
                    {
                        Id = 1,
                        Duration = 35 *60,
                        IsBreak = false,
                        SantaId = -1,
                        Desired = new (int, int)[0],
                        Unavailable = new (int, int)[]
                        {
                            (1 * hour, 5 * hour)
                        },
                        WayCostFromHome = w02.Duration,
                        WayCostToHome = w20.Duration,
                    },
                    new Core.Models.Visit
                    {
                        Id = 2,
                        Duration = 30 *60,
                        Desired = new (int from, int to)[0],
                        Unavailable = new (int from, int to)[0],
                        IsBreak = true,
                        SantaId = 1,
                        WayCostFromHome = w03.Duration,
                        WayCostToHome = w30.Duration,
                    },
                    new Core.Models.Visit
                    {
                        Id = 3,
                        Duration = 30 *60,
                        Desired = new (int from, int to)[0],
                        Unavailable = new (int from, int to)[0],
                        IsBreak = true,
                        SantaId = 0,
                        WayCostFromHome = w04.Duration,
                        WayCostToHome = w40.Duration,
                    }
                },

                Days = new (int, int)[1]
                {
                    (0 * hour, 5 * hour)
                },
                RouteCosts = new int[4, 4]
                {
                    {0, w12.Duration, w13.Duration, w14.Duration},
                    {w21.Duration, 0, w23.Duration, w24.Duration},
                    {w31.Duration, w32.Duration, 0, w34.Duration},
                    {w41.Duration, w42.Duration, w43.Duration, 0},
                }
            };
            var actual = PersistenceToCoreConverter.Convert(workingDays, startVisit, visits, santas);

            AssertOptimisationInputEqual(expected, actual);
            
        }
    }
}