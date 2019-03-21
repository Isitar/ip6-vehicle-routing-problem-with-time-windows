using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Persistence.Entities;

namespace IRuettae.WebApi.Models
{
    public class AlgorithmStarter
    {
        public int Year { get; set; }
        public int StarterId { get; set; }
        public int MaxNumberOfAdditionalSantas { get; set; }
        public List<(DateTime, DateTime)> Days { get; set; }
        public int TimePerChild { get; set; }
        public int Beta0 { get; set; }
        public AlgorithmType Algorithm { get; set; }
        public long TimeLimitMinutes { get; set; }
    }
}