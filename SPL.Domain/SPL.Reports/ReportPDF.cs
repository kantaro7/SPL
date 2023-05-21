using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports
{
    public class ReportPDF
    {
        public long IdLoad { get; set; }
        public byte[] File { get; set; }
        public string ReportName { get; set; }
    }
}
