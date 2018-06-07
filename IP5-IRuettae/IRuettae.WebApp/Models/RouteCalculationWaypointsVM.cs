using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRuettae.WebApp.Models
{
    public class RouteCalculationWaypointVM
    {
        [DisplayName("Besuch")]
        public VisitVM Visit { get; set; }

        [DisplayName("Startzeit")]
        public DateTime VisitStartTime { get; set; }

        [DisplayName("Besuchs Endzeit")]
        public DateTime VisitEndTime { get; set; }
    }
}