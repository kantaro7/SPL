using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.TDP
{
    public class TDPTestsDetails
    {
        public string Time { get; set; }
        public decimal Voltage { get; set; }
        public List<TDPTerminals> TDPTerminals { get; set; }
    }
}
