using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static IRuettae.WebApp.Models.RouteCalculationVM;

namespace IRuettae.WebApp.Models
{
    public class AlgorithmStarterVM
    {
        [Required]
        [DisplayName("Jahr")]
        [Range(2017, int.MaxValue)]
        public int Year { get; set; }

        public IEnumerable<SelectListItem> StarterIds { get; set; }
        [Required]
        [DisplayName("Startpunkt")]
        public int StarterId { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayName("Maximale Anzahl zusätzlicher Chläuse")]
        public int MaxNumberOfAdditionalSantas { get; set; }

        [DisplayName("Arbeitstage")]
        public List<PeriodVM> DaysPeriod { get; set; }

        [DisplayName("Arbeitstage")]
        public List<(DateTime, DateTime)> Days { get; set; }

        [Required]
        [DisplayName("Zeit für jedes weitere Kind [min]")]
        [Range(1, int.MaxValue)]
        public int TimePerChild { get; set; }

        [DisplayName("Zeit für erstes Kind [min]")]
        [Range(1, int.MaxValue)]
        public int Beta0 { get; set; }


        [Required]
        [DisplayName("Algorithmus")]
        public AlgorithmType Algorithm { get; set; }
        public IEnumerable<SelectListItem> AlgorithmTypes { get; set; }

        [Required]
        [DisplayName("Zeitlimit [min]")]
        [Range(1, int.MaxValue)]
        public int TimeLimitMinutes { get; set; }
    }
}