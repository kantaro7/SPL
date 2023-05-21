using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.PEE
{
    public class PEETestsDetailsDto
    {
     
        public string CoolingType { get; set; }
        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        //[Range(0, 999999.999)]
        public decimal VoltageRMS { get; set; }
        public decimal CurrentRMS { get; set; }
        public decimal PowerKW { get; set; }
        public decimal Kwaux_gar { get; set; }
        public decimal Mvaaux_gar { get; set; }

    }
}
