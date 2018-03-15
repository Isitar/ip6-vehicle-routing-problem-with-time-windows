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
    public class NHibernateConfiguration
    {
        public static ISessionFactory CreateSessionFactory(IPersistenceConfigurer persistenceConfigurer = null, bool recreateDataBase = false)
        {
            // hack to test
            if (persistenceConfigurer == null)
            {
                persistenceConfigurer =
                    MySQLConfiguration.Standard.ConnectionString(
                        "Server=localhost;Database=iRuettae;Uid=root;Pwd=root;");
            }

            return Fluently.Configure()
                .Database(persistenceConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<VisitMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<WayMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PeriodMap>())
                .ExposeConfiguration(cfg =>
                {
                    if (recreateDataBase)
                    {
                        var export = new SchemaExport(cfg);
                        export.Drop(false, true);
                        export.Create(false, true);
                    }
                    else
                    {
                        new SchemaUpdate(cfg).Execute(false, true);
                    }
                })
                .BuildSessionFactory();
        }
    }
}
