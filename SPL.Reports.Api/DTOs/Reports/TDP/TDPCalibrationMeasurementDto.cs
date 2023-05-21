using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.TDP
{
    public class TDPCalibrationMeasurementDto
    {

        public int Calibration1 { get; set; }
        public int Calibration2 { get; set; }
        public int Calibration3 { get; set; }
        public int Calibration4 { get; set; }
        public int Calibration5 { get; set; }
        public int Calibration6 { get; set; }

        public int Measured1 { get; set; }
        public int Measured2 { get; set; }
        public int Measured3 { get; set; }
        public int Measured4 { get; set; }
        public int Measured5 { get; set; }
        public int Measured6 { get; set; }
        public string Grades { get; set; }

    }
}
