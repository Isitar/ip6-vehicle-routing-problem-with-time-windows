using System;
using System.ComponentModel;

namespace IRuettae.WebApp.Models
{
    public class PeriodVM
    {
        
        [DisplayName("Von")]
        public DateTime? Start { get; set; }
        
        [DisplayName("Bis")]
        public DateTime? End { get; set; }
    }
}