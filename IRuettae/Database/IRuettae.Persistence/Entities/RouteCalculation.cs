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
        public virtual long TimeLimitMiliseconds { get; set; }

        // Algorithm specific data
        public virtual AlgorithmType Algorithm { get; set; }
        public virtual string AlgorithmData { get; set; }

        // Running & Result
        public virtual string Result { get; set; }
        public virtual RouteCalculationState State { get; set; }
        public virtual double Progress { get; set; }
        public virtual IList<RouteCalculationLog> StateText { get; set; }
        public virtual DateTime EndTime { get; set; }

        public RouteCalculation()
        {
            this.StateText = new List<RouteCalculationLog>();
        }
    }
}
