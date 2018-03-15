using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentNHibernate.Cfg.Db;
using IRuettae.Persistence.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;

namespace IRuettae.Persistence.Tests
{
    [TestClass]
    public class NHibernateConfigurationTest
    {
        [TestMethod]
        public void TestDBAccess()
        {
            var sessionFactory = NHibernateConfiguration.CreateSessionFactory(MySQLConfiguration.Standard.ConnectionString("Server=localhost;Database=iRuettae_UnitTests;Uid=root;Pwd=root;"), true);
            
            Assert.IsNotNull(sessionFactory);
            var session = sessionFactory.OpenSession();
            Assert.IsNotNull(session);
            Assert.IsTrue(session.IsConnected);
            Assert.IsTrue(session.IsOpen);

            using (var transaction = session.BeginTransaction())
            {
                var visit = new Visit
                {
                    ExternalReference = "someKey",
                    NumberOfChildrean = 10,
                    Street = "somestreet",
                    Year = 2018,
                    Zip = 5600,
                    Desired = new List<Period> {new Period {Start = DateTime.Now }},
                    Unavailable = new List<Period> {new Period {End = DateTime.Today}}
                };

                visit = session.Merge(visit);

                var visit2 = new Visit
                {
                    ExternalReference = "someKey2",
                    NumberOfChildrean = 3,
                    Street = "some other street",
                    Year = 2018,
                    Zip = 5000,
                    Desired = new List<Period>(),
                    Unavailable = new List<Period>()
                };
                visit2 = session.Merge(visit2);

                var way = new Way
                {
                    Distance = 100,
                    Duration = 200,
                    From = visit,
                    To = visit2
                };
                var way2 = new Way
                {
                    Distance = 200,
                    Duration = 100,
                    From = visit2,
                    To = visit
                };

                way = session.Merge(way);
                way2 = session.Merge(way2);
                transaction.Commit();
            }

            using (var transaction = session.BeginTransaction())
            {
                
                var managedVisits = session.Query<Visit>().ToList();
                var managedWays = session.Query<Way>().ToList();
                var managedPeriods = session.Query<Period>().ToList();
                Assert.AreEqual(2, managedPeriods.Count);
                Assert.AreEqual(2, managedVisits.Count);
                Assert.AreEqual(2, managedWays.Count);
                var visit = managedVisits.First();
                Assert.AreEqual(visit.Desired.First().Id, managedPeriods.FirstOrDefault(p => p.Start != null)?.Id);
                Assert.AreEqual(visit.Unavailable.First().Id, managedPeriods.FirstOrDefault(p => p.Start == null)?.Id);
                Assert.AreEqual(1, managedWays.Count(w => w.From.Id == visit.Id));
                Assert.AreEqual(1, managedWays.Count(w => w.To.Id == visit.Id));
                transaction.Commit();
            }


        }
    }
}
