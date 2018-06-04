using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRuettae.WebApp.Models
{
    public class AlgorithmStarterVM
    {

        public IEnumerable<SelectListItem> StarterIds { get; set; }
        [Required]
        [DisplayName("Startpunkt")]
        public int StarterId { get; set; }

        [DisplayName("Arbeitstage")]
        public List<PeriodVM> DaysPeriod { get; set; }


        [DisplayName("Arbeitstage")]
        public List<(DateTime, DateTime)> Days { get; set; }

        [Range(1, int.MaxValue)]
        [DisplayName("Anzahl Tage")]
        public int NumberOfDays { get; set; }
        [DisplayName("Timeslice Dauer [s]")]
        public int TimeSliceDuration { get; set; }
        [Required]
        [DisplayName("Zeit pro weiteres Kind [min]")]
        [Range(1, int.MaxValue)]
        public int TimePerChild { get; set; }
        [Range(1, int.MaxValue)]
        [DisplayName("Zeit für erstes Kind [min]")]
        public int Beta0 { get; set; }

        [Required]
        [DisplayName("Jahr")]
        [Range(2017, Int32.MaxValue)]
        public int Year { get; set; }
    }
}