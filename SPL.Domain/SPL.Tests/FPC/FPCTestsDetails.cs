using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests.FPC
{
    public class FPCTestsDetails
    {
        public int Row { get; set; }
        public string WindingA { get; set; }
        public string WindingB { get; set; }
        public string WindingC { get; set; }
        public string WindingD { get; set; }
        public decimal Current { get; set; }
        public decimal Power { get; set; }

        public decimal PercentageA { get; set; }
        public decimal PercentageB { get; set; }
        public decimal PercentageC { get; set; }
        public decimal Capacitance { get; set; }
        public string Id { get; set; }
    }
}
