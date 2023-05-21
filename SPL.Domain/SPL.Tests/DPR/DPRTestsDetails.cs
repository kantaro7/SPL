using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests.DPR
{
    public class DPRTestsDetails
    {
        public string Time { get; set; }
        public decimal Voltage { get; set; }
        public List<DPRTerminals> DPRTerminals { get; set; }
    }
}
