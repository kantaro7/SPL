using SPL.Domain.SPL.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.FPB
{
    public class FPBTestsDto
    {

        public string TanDelta { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTanD { get; set; }

        public decimal Tension { get; set; }
        public string UmTension { get; set; }
        public decimal Temperature { get; set; }
        public string UmTemperature { get; set; }

        public DateTime Date { get; set; }
        public decimal AcceptanceValueCap { get; set; }
        public decimal AcceptanceValueFP { get; set; }
        public List<FPBTestsDetailsDto> FPBTestsDetails { get; set; }

    }
}
