using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRuettae.WebApp.Models
{
    [DisplayName("Pause")]
    public class BreakVM : VisitVM
    {
        [DisplayName("Samichlaus Id")]
        public virtual long SantaId { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public override double Duration { get => base.Duration; set => base.Duration = value; }

        [Range(0, int.MaxValue)]
        public override int NumberOfChildren { get => base.NumberOfChildren; set => base.NumberOfChildren = value; }
    }
}