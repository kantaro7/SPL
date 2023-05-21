using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.DPR
{
    public class DPRTestsDetailsDto
    {
        public string Time { get; set; }
        public decimal Voltage { get; set; }
        public List<DPRTerminalsDto> DPRTerminals { get; set; }
    }
}
