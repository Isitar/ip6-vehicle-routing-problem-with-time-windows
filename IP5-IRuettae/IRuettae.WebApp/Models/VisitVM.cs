using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using IRuettae.Persistence.Entities;

namespace IRuettae.WebApp.Models
{
    public class VisitVM
    {
        [Required]
        [DisplayName("Strasse")]
        public string Street { get; set; }

        [Required]
        [DisplayName("Postleizahl")]
        public int Zip { get; set; }

        [Required]
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