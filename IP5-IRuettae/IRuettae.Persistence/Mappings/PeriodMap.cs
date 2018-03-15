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
        }
    }
}
