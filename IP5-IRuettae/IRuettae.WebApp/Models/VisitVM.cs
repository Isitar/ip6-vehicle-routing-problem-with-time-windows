using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace IRuettae.WebApp.Models
{
    public class VisitVM
    {
        [Required]
        [DisplayName("Strasse")]
        public string Street { get; set; }

        [Required]
        [Range(1000, 9999)]
        [DisplayName("Postleizahl")]
        public int Zip { get; set; }

        [Required]
        [DisplayName("Ort")]
        public string City { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [DisplayName("Anzahl Kinder")]
        public int NumberOfChildren { get; set; }

        [DisplayName("Wunschzeit")]
        public List<PeriodVM> Desired { get; set; }

        [DisplayName("Nicht verfügbar")]
        public List<PeriodVM> Unavailable { get; set; }

        [DisplayName("Jahr")]
        [Range(2017, int.MaxValue)]
        public int Year { get; set; }
        public VisitVM()
        {
            Desired = new List<PeriodVM>();
            Unavailable = new List<PeriodVM>();
            Year = DateTime.Now.Year;
        }
    }
}