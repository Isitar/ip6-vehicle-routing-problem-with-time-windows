using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using IRuettae.Persistence.Entities;
using IRuettae.Persistence.Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace IRuettae.Persistence
{
    public enum NHibernateConfigurationConfigurationOptions
    {
        None,
        RecreateDB,
        UpdateDB
    }

    public class NHibernateConfiguration
    {
        public static Configuration Config { get; private set; }

        public static ISessionFactory CreateSessionFactory(IPersistenceConfigurer persistenceConfigurer = null, NHibernateConfigurationConfigurationOptions configurationOptions = NHibernateConfigurationConfigurationOptions.UpdateDB)
        {

            return Fluently.Configure()
                .Database(persistenceConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<VisitMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<WayMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PeriodMap>())

                .ExposeConfiguration(cfg =>
                {
                    Config = cfg;
                    switch (configurationOptions)
                    {
                        case NHibernateConfigurationConfigurationOptions.RecreateDB:
                            var export = new SchemaExport(cfg);
                            export.Drop(false, true);
                            export.Create(false, true);
                            break;

                        case NHibernateConfigurationConfigurationOptions.UpdateDB:
                            new SchemaUpdate(cfg).Execute(false, true);
                            break;
                        case NHibernateConfigurationConfigurationOptions.None:
                            break;
                    }
                })
                .BuildSessionFactory();
        }
    }
}
