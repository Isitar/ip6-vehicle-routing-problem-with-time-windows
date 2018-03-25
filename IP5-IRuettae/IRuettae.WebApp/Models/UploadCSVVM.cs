using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRuettae.WebApp.Models
{
    public class UploadCSVVM
    {
        [Required]
        [DisplayName("CSV Datei")]
        public HttpPostedFileBase CSVFile { get; set; }
    }
}