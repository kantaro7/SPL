using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.TDP
{
    public class TDPTestsDto
    {
        public List<TDPTestsDetailsDto> TDPTestsDetails { get; set; }
        public TDPCalibrationMeasurementDto TDPTestsDetailsCalibration { get; set; }


    }
}
