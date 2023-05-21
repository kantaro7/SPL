using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.DPR
{
    public class DPRTerminalsDto
    {
        public string Terminal { get; set; }
        public int? pC { get; set; }
        public int? µV { get; set; }
    }
}
