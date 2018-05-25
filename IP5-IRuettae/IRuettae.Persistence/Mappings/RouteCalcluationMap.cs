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
            Map(x => x.SantaJson);
            Map(x => x.VisitsJson);

            Map(x => x.ClusteringOptimisationFunction);
            Map(x => x.ClustringMipGap);
            Map(x => x.ClusteringResult);

            Map(x => x.TimeSliceDuration);
            Map(x => x.SchedulingMipGap);
            Map(x => x.SchedulingResult);

            Map(x => x.Result);
            
            Map(x => x.State);
            Map(x => x.StateText);
            Map(x => x.EndTime);
        }
    }
}
