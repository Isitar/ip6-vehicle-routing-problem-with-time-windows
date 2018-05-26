using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Core.Algorithm;

namespace IRuettae.WebApi.Models
{
    public class SchedulingResult
    {
        public Route Route { get; set; }
        public DateTime StartingTime { get; set; }
    }
}