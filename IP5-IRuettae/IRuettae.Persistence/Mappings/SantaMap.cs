using FluentNHibernate.Mapping;
using IRuettae.Persistence.Entities;

namespace IRuettae.Persistence.Mappings
{
    public class SantaMap : ClassMap<Santa>
    {
        public SantaMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);

            HasMany(x => x.Breaks).KeyColumn("santa_id").Not.LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
