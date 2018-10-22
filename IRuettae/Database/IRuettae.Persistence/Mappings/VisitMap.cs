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
            Map(x => x.NumberOfChildren);
            Map(x => x.Street);
            Map(x => x.OriginalStreet);
            Map(x => x.City);
            Map(x => x.Year);
            Map(x => x.Zip);
            Map(x => x.Duration);
            Map(x => x.VisitType);

            Map(x => x.Lat);
            Map(x => x.Long);

            Map(x => x.DeltaWayDistance);
            Map(x => x.DeltaWayDuration);

            // there should not be to many data
            HasMany(x => x.Desired).KeyColumn("desired_visit_id").Not.LazyLoad().Cascade.AllDeleteOrphan();
            // there should not be to many data
            HasMany(x => x.Unavailable).KeyColumn("unavailable_visit_id").Not.LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.FromWays).KeyColumn("From_id").Inverse().ForeignKeyCascadeOnDelete().Cascade.AllDeleteOrphan();
            HasMany(x => x.ToWays).KeyColumn("To_id").Inverse().ForeignKeyCascadeOnDelete().Cascade.AllDeleteOrphan();

            HasOne(x => x.Santa).Cascade.All();
        }
    }
}
