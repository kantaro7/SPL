using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.FPA
{
   public class FPAGasContentDto
    {

        public string ASTM { get; set; }
        public decimal Measurement { get; set; }
        public decimal Limit1 { get; set; }
        public string Limit2 { get; set; }
    }
}
