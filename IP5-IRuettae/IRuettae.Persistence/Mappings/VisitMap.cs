using FluentNHibernate.Mapping;
using IRuettae.Persistence.Entities;

namespace IRuettae.Persistence.Mappings
{
    public class VisitMap : ClassMap<Visit>
    {
        public VisitMap()
        {
            Id(x => x.Id);
            Map(x => x.ExternalReference);
            Map(x => x.NumberOfChildrean);
            Map(x => x.Street);
            Map(x => x.Year);
            Map(x => x.Zip);
            HasMany(x => x.Desired).Cascade.AllDeleteOrphan();
            HasMany(x => x.Unavailable).Cascade.AllDeleteOrphan();
        }
    }
}
