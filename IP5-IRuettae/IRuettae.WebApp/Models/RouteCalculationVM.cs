using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.WebApp.Models
{
    public class RouteCalculationVM
    {
        // starter Properties
        [DisplayName("Startzeit")]
        public virtual DateTime StartTime { get; set; }
        [DisplayName("Jahr")]
        public virtual int Year { get; set; }
        [DisplayName("Tage")]
        public virtual List<(DateTime, DateTime)> Days { get; set; }
        [DisplayName("Zeit Pro Kind")]
        public virtual int TimePerChild { get; set; }
        [DisplayName("+ Zeit für erstes Kind")]
        public virtual int TimePerChildOffset { get; set; }
        [DisplayName("Startpunkt")]
        public virtual int StarterVisitId { get; set; }


        [DisplayName("Anzahl Chläuse")]
        public virtual int NumberOfSantas { get; set; }
        [DisplayName("Anzahl Besuche")]
        public virtual int NumberOfVisits{ get; set; }
        [DisplayName("")]
        public virtual string SantaJson { get; set; }
        [DisplayName("")]
        public virtual string VisitsJson { get; set; }

        // Phase 1
        [DisplayName("Clustering Zielfunktion")]
        public virtual int ClusteringOptimisationFunction { get; set; }
        [DisplayName("Clustering MIP GAP")]
        public virtual double ClustringMipGap { get; set; }
        [DisplayName("Clustering Resultat")]
        public virtual string ClusteringResult { get; set; }

        // Phase 2
        [DisplayName("Zeitgenauigkeit")]
        public virtual int TimeSliceDuration { get; set; }
        [DisplayName("Scheduling MIP GAP")]
        public virtual double SchedulingMipGap{ get; set; }
        [DisplayName("Scheduling Resultat")]
        public virtual string SchedulingResult { get; set; }

        // Phase 3



        // Running & Result
        [DisplayName("Resultat")]
        public virtual string Result { get; set; }
        [DisplayName("Status")]
        public virtual int State { get; set; }
        [DisplayName("StatusText")]
        public virtual string StateText { get; set; }
        [DisplayName("Endzeit")]
        public virtual DateTime EndTime { get; set; }
        


    }
}
