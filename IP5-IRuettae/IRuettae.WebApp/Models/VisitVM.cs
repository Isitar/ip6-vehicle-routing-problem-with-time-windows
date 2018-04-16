using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace IRuettae.WebApp.Models
{
    public class VisitVM
    {
        [DisplayName("Id")]
        public virtual long Id { get; set; }

        [Required]
        [DisplayName("Strasse")]
        public virtual string Street { get; set; }

        [DisplayName("Echte Strasse")]
        public virtual string OriginalStreet { get; set; }

        [Required]
        [Range(1000, 9999)]
        [DisplayName("Postleizahl")]
        public virtual int Zip { get; set; }

        [Required]
        [DisplayName("Ort")]
        public virtual string City { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [DisplayName("Anzahl Kinder")]
        public virtual int NumberOfChildren { get; set; }

        [DisplayName("Wunschzeit")]
        public virtual List<PeriodVM> Desired { get; set; }

        [DisplayName("Nicht verfügbar")]
        public virtual List<PeriodVM> Unavailable { get; set; }

        [DisplayName("Jahr")]
        [Range(2017, int.MaxValue)]
        public virtual int Year { get; set; }

        [DisplayName("Abweichungsdistanz [m]")]
        public virtual int DeltaWayDistance { get; set; }
        [DisplayName("Abweichungsdauer [s]")]
        public virtual int DeltaWayDuration { get; set; }

        [DisplayName("Alternative Adresse benötigt")]
        public virtual bool AlternativeAddressNeeded { get; set; }

        
        [DisplayName("Dauer [s]")]
        public virtual double Duration { get; set; }

        public VisitVM()
        {
            Desired = new List<PeriodVM>();
            Unavailable = new List<PeriodVM>();
            Year = DateTime.Now.Year;
        }
    }
}