using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRuettae.WebApi.Models
{
    public class AlgorithmStarter
    {
        public int StarterId { get; set; }
        public List<(DateTime, DateTime)> Days { get; set; }
        public int TimeSliceDuration { get; set; }
        public int TimePerChild { get; set; }
        public int Beta0 { get; set; }
        public int Year { get; set; }
    }
}