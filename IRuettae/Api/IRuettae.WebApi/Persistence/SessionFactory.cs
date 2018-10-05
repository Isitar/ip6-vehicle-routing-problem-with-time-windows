using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FluentNHibernate.Cfg.Db;
using IRuettae.Persistence;
using NHibernate;

namespace IRuettae.WebApi.Persistence
{
    /// <summary>
    /// Singleton class, used for session generation.
    /// </summary>
    public sealed class SessionFactory
    {
        private SessionFactory()
        {
            var connectionStringName = ConfigurationManager.AppSettings["Database"];
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            factory = NHibernateConfiguration.CreateSessionFactory(
                    MySQLConfiguration.Standard.ConnectionString(connectionString));
        }
        private static readonly Lazy<SessionFactory> lazy = new Lazy<SessionFactory>(() => new SessionFactory());

        public static SessionFactory Instance => lazy.Value;

        private static ISessionFactory factory;
        public ISession OpenSession()
        {
            return factory.OpenSession();
        }
    }
}