using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRuettae.WebApp.Models
{
    [DisplayName("Samichlaus")]
    public class SantaVM
    {

        [DisplayName("Id")]
        public virtual long Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public virtual string Name { get; set; }

        
        [DisplayName("Pausen")]
        public virtual IList<BreakVM> Breaks { get; set; }
    }
}