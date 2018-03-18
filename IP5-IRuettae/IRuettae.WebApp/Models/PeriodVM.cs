using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

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