using System.Runtime.Remoting.Metadata.W3cXsd2001;
using FluentNHibernate.Mapping;
using IRuettae.Persistence.Entities;

namespace IRuettae.Persistence.Mappings
{
    public class RouteCalculationMap : ClassMap<RouteCalculation>
    {
        public RouteCalculationMap()
        {
            Id(x => x.Id);
            Map(x => x.Year);
            Map(x => x.Days);
            Map(x => x.TimePerChild);
            Map(x => x.TimePerChildOffset);
            Map(x => x.StarterVisitId);

            Map(x => x.NumberOfSantas);
            Map(x => x.NumberOfVisits);
            Map(x => x.SantaJson).CustomSqlType("LONGTEXT");
            Map(x => x.VisitsJson).CustomSqlType("LONGTEXT");

            Map(x => x.Algorithm);
            Map(x => x.AlgorithmData).CustomSqlType("LONGTEXT");

            Map(x => x.Result).CustomSqlType("LONGTEXT");
            Map(x => x.State);
            Map(x => x.Progress);
            HasMany(x => x.StateText).KeyColumn("routecalculation_id").Not.LazyLoad().Cascade.AllDeleteOrphan();
            Map(x => x.EndTime);
            Map(x => x.StartTime);

            Map(x => x.NumberOfRoutes);
            Map(x => x.TotalWaytime);
            Map(x => x.TotalVisitTime);
            Map(x => x.WaytimePerSanta);
            Map(x => x.DesiredSeconds);
            Map(x => x.LongestRouteTime);
            Map(x => x.LongestRouteDistance);
            Map(x => x.LongestDay);
            Map(x => x.LatestVisit);
        }
    }
}
