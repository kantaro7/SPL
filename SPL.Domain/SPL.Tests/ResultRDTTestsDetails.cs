using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests
{
    public class ResultRDTTestsDetails
    {
        public List<ErrorColumns> MessageErrors { get; set; }
        public List<decimal> NominalValue { get; set; }
        public List<decimal> DeviationPhasesA { get; set; }
        public List<decimal> DeviationPhasesB { get; set; }
        public List<decimal> DeviationPhasesC { get; set; }

        public List<decimal> HVVolts { get; set; }
        public List<decimal> LVVolts { get; set; }
    }
}
