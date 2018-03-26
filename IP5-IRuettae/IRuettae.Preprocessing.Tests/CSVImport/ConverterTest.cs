using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Preprocessing.CSVImport;
using IRuettae.Persistence.Entities;
using System.Linq;

namespace IRuettae.Preprocessing.Tests.CSVImport
{
    static class Util
    {
        class Compare : IEqualityComparer<Persistence.Entities.Period>
        {
            public bool Equals(Persistence.Entities.Period p1, Persistence.Entities.Period p2)
            {
                return p1.Start == p2.Start && p1.End == p2.End;
            }
            public int GetHashCode(Persistence.Entities.Period codeh)
            {
                return 0;
            }
        }

        public static bool Match(Visit v1, Visit v2)
        {
            return true
                && v1.Id == v2.Id
                && v1.ExternalReference == v2.ExternalReference
                && v1.ExternalReference == v2.ExternalReference
                && v1.Year == v2.Year
                && v1.Street == v2.Street
                && v1.Zip == v2.Zip
                && v1.NumberOfChildren == v2.NumberOfChildren
                && v1.ExternalReference == v2.ExternalReference
                && v1.ExternalReference == v2.ExternalReference
                && v1.Desired.SequenceEqual(v2.Desired, new Compare())
                && v1.Unavailable.SequenceEqual(v2.Unavailable, new Compare())
               ;
        }
    }

    [TestClass]
    public class ConverterTest
    {

        [TestMethod]
        public void TestToDatabase()
        {
            var input = new List<ImportModel>
            {
                new ImportModel(
                    "2017001", "Isenbühlweg 16", 5524, 5,
                    new List<Preprocessing.CSVImport.Period>
                    {
                        new Preprocessing.CSVImport.Period(new DateTime(2017,12,09,18,00,00), new DateTime(2017,12,09,19,30,00)),
                        new Preprocessing.CSVImport.Period(new DateTime(2017,12,09,20,00,00), new DateTime(2017,12,09,21,30,00))
                    },
                    new List<Preprocessing.CSVImport.Period>
                    {
                        new Preprocessing.CSVImport.Period(new DateTime(2017,12,08,17,00,00), new DateTime(2017,12,08,20,00,00))
                    }
                ),
                new ImportModel(
                    "abc", "Teststrasse 1", 1235, 1,
                    new List<Preprocessing.CSVImport.Period>
                    {
                        new Preprocessing.CSVImport.Period(new DateTime(2017,12,08,17,00,00), new DateTime(2017,12,08,20,00,00))
                    },
                    new List<Preprocessing.CSVImport.Period>
                    {
                        new Preprocessing.CSVImport.Period(new DateTime(2017,12,09,18,00,00), new DateTime(2017,12,09,19,30,00)),
                        new Preprocessing.CSVImport.Period(new DateTime(2017,12,09,20,00,00), new DateTime(2017,12,09,21,30,00))
                    }
                ),
            };

            var expected = new List<Visit>
            {
                new Visit{
                    ExternalReference = "2017001",
                    Year = 2017,
                    Street = "Isenbühlweg 16",
                    Zip = 5524,
                    NumberOfChildren = 5,
                    Desired = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period{
                            Start = new DateTime(2017,12,09,18,00,00),
                            End = new DateTime(2017,12,09,19,30,00)
                        },
                        new Persistence.Entities.Period{
                            Start = new DateTime(2017,12,09,20,00,00),
                            End = new DateTime(2017,12,09,21,30,00)
                        }
                    },
                    Unavailable = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period{
                            Start = new DateTime(2017,12,08,17,00,00),
                            End = new DateTime(2017,12,08,20,00,00)
                        }
                    }
                },
                new Visit
                {
                    ExternalReference = "abc",
                    Year = DateTime.Now.Year,
                    Street = "Teststrasse 1",
                    Zip = 1235,
                    NumberOfChildren = 1,
                    Desired = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period{
                            Start = new DateTime(2017, 12, 08, 17, 00, 00),
                            End = new DateTime(2017, 12, 08, 20, 00, 00)
                        }
                    },
                    Unavailable = new List<Persistence.Entities.Period>
                    {
                        new Persistence.Entities.Period{
                            Start = new DateTime(2017,12,09,18,00,00),
                            End = new DateTime(2017,12,09,19,30,00)
                        },
                        new Persistence.Entities.Period{
                            Start = new DateTime(2017,12,09,20,00,00),
                            End = new DateTime(2017,12,09,21,30,00)
                        }
                    }
                }
            };

            var actual = new List<Visit>(Converter.ToDatabase(input));

            Assert.IsNotNull(actual.FirstOrDefault(i => Util.Match(i, expected[0])));
            Assert.IsNotNull(actual.FirstOrDefault(i => Util.Match(i, expected[1])));
            Assert.AreEqual(2, actual.Count);
        }
    }
}
