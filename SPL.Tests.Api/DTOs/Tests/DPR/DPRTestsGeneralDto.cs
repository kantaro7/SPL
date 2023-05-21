using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.DPR
{
    public class DPRTestsGeneralDto
    {
        public int Interval { get; set; }
        public int TotalTime { get; set; }
        public int DescMayPc { get; set; } 
        public int DescMayMv { get; set; } 
        public int IncMaxPc { get; set; } 
        public int Tolerance { get; set; }
        public string KeyTest { get; set; }
        public DPRTestsDto DPRTest { get; set; }

        public string MeasurementType { get; set; }



    }
}
