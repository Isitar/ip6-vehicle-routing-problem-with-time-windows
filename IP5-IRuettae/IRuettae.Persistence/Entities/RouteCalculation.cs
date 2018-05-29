using System;
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
        public virtual int NumberOfVisits{ get; set; }
        public virtual string SantaJson { get; set; }
        public virtual string VisitsJson { get; set; }
        
        // Phase 1
        public virtual ClusteringOptimisationGoals ClusteringOptimisationFunction { get; set; }
        public virtual double ClustringMipGap { get; set; }
        public virtual string ClusteringResult { get; set; }

        // Phase 2
        public virtual int TimeSliceDuration { get; set; }
        public virtual double SchedulingMipGap{ get; set; }
        public virtual string SchedulingResult { get; set; }


        // Running & Result
        public virtual string Result { get; set; }
        public virtual RouteCalculationState State { get; set; }
        public virtual string StateText { get; set; }
        public virtual DateTime EndTime { get; set; }
        


    }
}
