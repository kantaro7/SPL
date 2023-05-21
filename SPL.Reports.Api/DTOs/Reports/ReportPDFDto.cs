using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports
{
    public class ReportPDFDto
{
        public long IdLoad { get; set; }
        public string File { get; set; }
        public string ReportName { get; set; }
    }
}
