using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests
{
    public class ResultRADTestsDetails
    {
        public List<ErrorColumns> MessageErrors { get; set; }
        public List<decimal> AbsorptionIndexs { get; set; }
        public List<decimal> PolarizationIndexs { get; set; }

    }
}
