using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.PEE
{
    public class PEETestsDetails
    {
     
        public string CoolingType { get; set; }
        public decimal VoltageRMS { get; set; }
        public decimal CurrentRMS { get; set; }
        public decimal PowerKW { get; set; }
        public decimal Kwaux_gar { get; set; }
        public decimal Mvaaux_gar { get; set; }

    }
}
