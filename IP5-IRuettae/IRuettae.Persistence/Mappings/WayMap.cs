using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using IRuettae.Persistence.Entities;

namespace IRuettae.Persistence.Mappings
{
    public class WayMap : ClassMap<Way>
    {
        public WayMap()
        {
            Id(x => x.Id);
            Map(x => x.Distance);
            Map(x => x.Duration);
            References(x => x.From)
                .Column("from_visit_id")
                .Cascade.All();
            References(x => x.To)
                .Column("to_visit_id")
                .Cascade.All();

        }
    }
}
