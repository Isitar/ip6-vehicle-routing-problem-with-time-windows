using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using IRuettae.Persistence.Entities;

namespace IRuettae.Persistence.Mappings
{
    public class VisitMap : ClassMap<Visit>
    {
        public VisitMap()
        {
            Id(x => x.Id);
            Map(x => x.Key);
            Map(x => x.NumberOfChildrean);
            Map(x => x.Street);
            Map(x => x.Year);
            Map(x => x.Zip);
            HasMany(x => x.Desired);
            HasMany(x => x.Unavailable);
        }
    }
}
