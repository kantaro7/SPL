using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.TAP
{
    public class TAPTestsDetails
    {
     

        public string WindingEnergized { get; set; }
        public string WindingGrounded { get; set; }
        public decimal VoltageLevel { get; set; }
        public decimal AppliedkV { get; set; }
        public decimal CurrentkV { get; set; }
        public int Time { get; set; }
        public int Capacitance { get; set; }
        public decimal AmpCal { get; set; }
        public decimal CurrentPercentage { get; set; }

    }
}
