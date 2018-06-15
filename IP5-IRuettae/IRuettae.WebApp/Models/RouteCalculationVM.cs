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
        public virtual long Id { get; set; }
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


        [DisplayName("Anzahl benötigte Chläuse")]
        public virtual int NumberOfSantas { get; set; }

        [DisplayName("Anzahl Routen")]
        public virtual int NumberOfRoutes { get; set; }
        [DisplayName("Anzahl Besuche")]
        public virtual int NumberOfVisits { get; set; }
        [DisplayName("")]
        public virtual string SantaJson { get; set; }
        [DisplayName("")]
        public virtual string VisitsJson { get; set; }

        // Phase 1
        [DisplayName("Clustering Zielfunktion")]
        public virtual ClusteringOptimisationGoals ClusteringOptimisationFunction { get; set; }
        [DisplayName("Clustering MIP GAP")]
        public virtual double ClustringMipGap { get; set; }
        [DisplayName("Clustering Resultat")]
        public virtual string ClusteringResult { get; set; }

        // Phase 2
        [DisplayName("Zeitgenauigkeit [s]")]
        public virtual int TimeSliceDuration { get; set; }
        [DisplayName("Scheduling MIP GAP")]
        public virtual double SchedulingMipGap { get; set; }
        [DisplayName("Scheduling Resultat")]
        public virtual string SchedulingResult { get; set; }


        // Running & Result
        [DisplayName("Resultat")]
        public virtual string Result { get; set; }
        [DisplayName("Status")]
        public virtual RouteCalculationState State { get; set; }
        [DisplayName("StatusText")]
        public virtual string StateText { get; set; }
        [DisplayName("Endzeit")]
        public virtual DateTime EndTime { get; set; }

        // Metrics
        [DisplayName("Totale Wegzeit")]
        public virtual double TotalWaytime { get; set; }

        [DisplayName("Totale Besuchszeit")]
        public virtual double TotalVisitTime { get; set; }

        [DisplayName("Ø Wegzeit pro Chlaus")]
        public virtual double WaytimePerSanta { get; set; }
        [DisplayName("Wunschzeit erfüllt")]
        public virtual double DesiredSeconds { get; set; }
        [DisplayName("Längste Route (Zeitlich)")]
        public virtual double LongestRouteTime { get; set; }
        [DisplayName("Längste Route (Distanz)")]
        public virtual double LongestRouteDistance { get; set; }

        [DisplayName("Längster Tag")]
        public virtual double LongestDay { get; set; }

        [DisplayName("Spätester Besuch")]
        public virtual DateTime LatestVisit { get; set; }


        public enum RouteCalculationState
        {
            Creating,
            Ready,
            RunningPhase1,
            RunningPhase2,
            RunningPhase3,
            Cancelled,
            Finished,
        }

        public enum ClusteringOptimisationGoals
        {
            OverallMinTime,
            MinTimePerSanta,
            MinAvgTimePerSanta
        }

    }
}
