using FluentNHibernate.Mapping;
using IRuettae.Persistence.Entities;

namespace IRuettae.Persistence.Mappings
{
    public class RouteCalculationLogMap : ClassMap<RouteCalculationLog>
    {
        public RouteCalculationLogMap()
        {
            Id(x => x.Id);
            Map(x => x.CreationDate);
            Map(x => x.Log);
        }
    }
}
