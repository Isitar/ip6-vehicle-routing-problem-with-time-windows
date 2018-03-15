using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using IRuettae.Persistence.Entities;

namespace IRuettae.Persistence.Mappings
{
    public class PeriodMap : ClassMap<Period>
    {
        public PeriodMap()
        {
            Id(x => x.Id);
            Map(x => x.Start);
            Map(x => x.End);
            References(x => x.Visit)
                .Column("visit_id")
                .Cascade.All();
        }
    }
}
