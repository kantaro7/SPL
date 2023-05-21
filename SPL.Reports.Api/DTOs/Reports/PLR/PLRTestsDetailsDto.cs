using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.FPC
{
    public class PLRTestsDetailsDto
    {
     
        public decimal Time { get; set; }
        public decimal Current { get; set; }
        public decimal Tension { get; set; }
        public decimal Reactance { get; set; }
        public decimal PorcD { get; set; }
    }
}
