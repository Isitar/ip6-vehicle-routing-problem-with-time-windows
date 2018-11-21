using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        // Algorithm specific data
        [DisplayName("Algorithmus")]
        public virtual AlgorithmType Algorithm { get; set; }


        // Running & Result
        [DisplayName("Status")]
        public virtual RouteCalculationState State { get; set; }
        [DisplayName("Fortschritt")]
        [DisplayFormat(DataFormatString = "{0:P0}")]
        public virtual double Progress { get; set; }
        [DisplayName("Statustext")]
        public virtual List<dynamic> StateText { get; set; }
        [DisplayName("Endzeit")]
        public virtual DateTime EndTime { get; set; }

        // Metrics
        [DisplayName("Kosten")]
        public virtual int Cost { get; set; }

        [DisplayName("Anzahl nicht besuchter Familien")]
        public virtual int NumberOfNotVisitedFamilies { get; set; }

        [DisplayName("Anzahl ausgelassener Pausen")]
        public virtual int NumberOfMissingBreaks { get; set; }

        [DisplayName("Anzahl zusätzlich benötigter Chläuse")]
        public virtual int NumberOfAdditionalSantas { get; set; }

        [DisplayName("Arbeitszeit der zusätzlich benötigten Chläusen")]
        public virtual int AdditionalSantaWorkTime { get; set; }

        [DisplayName("Besuchszeiten ausserhalb der verfügbaren Zeit")]
        public virtual int VisitTimeInUnavailable { get; set; }

        [DisplayName("Besuchszeiten innerhalb der Wunschzeit")]
        public virtual int VisitTimeInDesired { get; set; }

        [DisplayName("Arbeitszeit der Chläuse")]
        public virtual int SantaWorkTime { get; set; }

        [DisplayName("Längster Tag")]
        public virtual int LongestDay { get; set; }

        [DisplayName("Anzahl benötigter Chläuse")]
        public virtual int NumberOfNeededSantas { get; set; }

        [DisplayName("Anzahl Routen")]
        public virtual int NumberOfRoutes { get; set; }

        [DisplayName("Anzahl Besuche")]
        public virtual int NumberOfVisits { get; set; }


        [DisplayName("Totale Wegzeit")]
        public virtual int TotalWayTime { get; set; }

        [DisplayName("Totale Besuchszeit")]
        public virtual int TotalVisitTime { get; set; }


        [DisplayName("Durchschnittliche Wegzeit")]
        public virtual int AverageWayTimePerRoute { get; set; }


        [DisplayName("Durchschnittliche Zeit pro Route")]
        public virtual int AverageDurationPerRoute { get; set; }


        public enum RouteCalculationState
        {
            Creating,
            Ready,
            Running,
            Cancelled,
            Finished,
        }

        public enum AlgorithmType
        {
            ILP,
        }

        public bool IsFinished => State == RouteCalculationState.Finished;
    }
}
