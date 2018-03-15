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
            References(x => x.From).Cascade.All();
            References(x => x.To).Cascade.All();
        }
    }
}
