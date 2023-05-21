using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.RCT
{
    public class RCTTestsDetails
    {
        public string Phase { get; set; }
        public string Terminal { get; set; }
        public string Position { get; set; }
        public decimal Resistencia { get; set; }
        public string Unit { get; set; }

        public decimal Temperature { get; set; }
    }
}
