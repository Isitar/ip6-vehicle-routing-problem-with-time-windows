using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public class RouteCalculation : BaseEntity
    {
        // starter Properties
        public virtual DateTime StartTime { get; set; }
        public virtual int Year { get; set; }
        public virtual List<(DateTime, DateTime)> Days { get; set; }
        public virtual int TimePerChild { get; set; }
        public virtual int TimePerChildOffset { get; set; }
        public virtual int StarterVisitId { get; set; }


        // info for history purpuse
        public virtual int NumberOfSantas { get; set; }
        public virtual int NumberOfVisits { get; set; }
        public virtual string SantaJson { get; set; }
        public virtual string VisitsJson { get; set; }

        // Phase 1
        public virtual ClusteringOptimizationGoals ClusteringOptimizationFunction { get; set; }
        public virtual double ClustringMipGap { get; set; }
        public virtual long ClusteringTimeLimit { get; set; }
        public virtual string ClusteringResult { get; set; }

        // Phase 2
        public virtual int TimeSliceDuration { get; set; }
        public virtual double SchedulingMipGap { get; set; }
        public virtual long SchedulingTimeLimit { get; set; }
        public virtual string SchedulingResult { get; set; }


        // Running & Result
        public virtual string Result { get; set; }
        public virtual RouteCalculationState State { get; set; }
        public virtual string StateText { get; set; }
        public virtual DateTime EndTime { get; set; }

        // Metrics
        public virtual int NumberOfRoutes { get; set; }
        public virtual double TotalWaytime { get; set; }
        public virtual double TotalVisitTime { get; set; }
        public virtual double WaytimePerSanta { get; set; }
        public virtual double DesiredSeconds { get; set; }
        public virtual double LongestRouteTime { get; set; }
        public virtual double LongestRouteDistance { get; set; }
        public virtual double LongestDay { get; set; }
        public virtual DateTime LatestVisit { get; set; }

    }
}
