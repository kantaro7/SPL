using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.FPA { 
   public class FPADielectricStrength
    {
    
        public string Electrodes { get; set; }
        public decimal OpeningMm { get; set; }
        public string ASTM { get; set; }
        public decimal OneSt { get; set; }
        public decimal TwoNd { get; set; }
        public decimal ThreeRd { get; set; }
        public decimal FourTh { get; set; }

        public decimal FiveTh { get; set; }
        public decimal Average { get; set; }
        public decimal MinLimit1 { get; set; }
        public string MinLimit2 { get; set; }
    }
}
