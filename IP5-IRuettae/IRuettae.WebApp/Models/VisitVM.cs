using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IRuettae.WebApp.Models
{
    public class VisitVM
    {
        [Required]
        [DisplayName("Strasse")]
        public string Street { get; set; }

        [Required]
        [Range(1000,9999)]
        [DisplayName("Postleizahl")]
        public int Zip { get; set; }

        [Required]
        [Range(1,Int32.MaxValue)]
        [DisplayName("Anzahl Kinder")]
        public int NumberOfChildren { get; set; }

        [DisplayName("Wunschzeit")]
        public List<PeriodVM> Desired { get; set; }
        [DisplayName("Nicht verfügbar")]
        public List<PeriodVM> Unavailable { get; set; }

        [DisplayName("Jahr")]
        public int Year { get; set; }
        public VisitVM()
        {
            Desired = new List<PeriodVM>();
            Unavailable = new List<PeriodVM>();
            Year = DateTime.Now.Year;
        }
    }
}