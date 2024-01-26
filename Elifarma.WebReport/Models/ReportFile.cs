using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Elifarma.WebReport.Models
{
    public class ReportFile
    {
        public string idreport { get; set; }
        public DateTime reportdate { get; set; }
        public string reportname { get; set; }
        public int userid { get; set; }
        public string reportlocation { get; set; }
        public int fieldstatus { get; set; }
        [NotMapped]
        public string contentfile { get; set; }
    }
}