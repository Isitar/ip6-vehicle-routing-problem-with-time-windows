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

        [DisplayName("Anzahl Routen")]
        public virtual int NumberOfRoutes { get; set; }
        [DisplayName("Anzahl Besuche")]
        public virtual int NumberOfVisits { get; set; }

        // Algorithm specific data
        [DisplayName("Algorithmus")]
        public virtual AlgorithmType Algorithm { get; set; }


        // Running & Result
        [DisplayName("Resultat")]
        public virtual string Result { get; set; }
        [DisplayName("Status")]
        public virtual RouteCalculationState State { get; set; }
        [DisplayName("Fortschritt")]
        [DisplayFormat(DataFormatString = "{0:P0}")]
        public virtual double Progress { get; set; }
        [DisplayName("Status Text")]
        public virtual List<dynamic> StateText { get; set; }
        [DisplayName("Statustext")]
        public virtual string StateTextDisplay { get; set; }
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

        [DisplayName("Längste Route")]
        public virtual double LongestDay { get; set; }

        [DisplayName("Spätester Besuch")]
        public virtual DateTime LatestVisit { get; set; }


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

    }
}
