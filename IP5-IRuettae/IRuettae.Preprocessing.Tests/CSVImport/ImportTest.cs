using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IRuettae.Preprocessing.CSVImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Preprocessing.Tests.CSVImport
{
    [TestClass]
    public class ImportTest
    {
        private const string testfile =
@"ID;Street;Zip;Children;DesiredFrom;DesiredTo;UnavailableFrom;UnavailableTo
;;;;;;;
2017001;Isenbühlweg 16;5524;5;09.12.2017 18:00;09.12.2017 19:30;08.12.2017 17:00;08.12.2017 20:00
;;;;09.12.2017 20:00;09.12.2017 21:30;;
2017002;Teststrasse 1;1235;1;08.12.2017 17:00;08.12.2017 20:00;09.12.2017 18:00;09.12.2017 19:30
;;;;;;09.12.2017 20:00;09.12.2017 21:30
;;;;;;;";

        [TestMethod]
        public void TestStartImport()
        {
            var expected = new List<ImportModel>
            {
                new ImportModel(
                    "2017001", "Isenbühlweg 16", 5524, 5,
                    new List<Period>
                    {
                        new Period(new DateTime(2017,12,09,18,00,00), new DateTime(2017,12,09,19,30,00)),
                        new Period(new DateTime(2017,12,09,20,00,00), new DateTime(2017,12,09,21,30,00))
                    },
                    new List<Period>
                    {
                        new Period(new DateTime(2017,12,08,17,00,00), new DateTime(2017,12,08,20,00,00))
                    }
                ),
                new ImportModel(
                    "2017002", "Teststrasse 1", 1235, 1,
                    new List<Period>
                    {
                        new Period(new DateTime(2017,12,08,17,00,00), new DateTime(2017,12,08,20,00,00))
                    },
                    new List<Period>
                    {
                        new Period(new DateTime(2017,12,09,18,00,00), new DateTime(2017,12,09,19,30,00)),
                        new Period(new DateTime(2017,12,09,20,00,00), new DateTime(2017,12,09,21,30,00))
                    }
                ),
            };
            var imported = Import.StartImport(testfile).OrderBy(m => m.Id).ToList();
            Assert.IsNotNull(imported.FirstOrDefault(i => i.Equals(expected[0])));
            Assert.IsNotNull(imported.FirstOrDefault(i => i.Equals(expected[1])));
            Assert.AreEqual(2, imported.Count);
        }
    }
}
