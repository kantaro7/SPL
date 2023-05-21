using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.ISZ
{
    public class ISZTestsDetails
    {
        public string Position1 { get; set; }
        public string Position2 { get; set; }
        public decimal Voltage1 { get; set; }
        public decimal Voltage2 { get; set; }


        public decimal VoltsVRMS { get; set; }
        public decimal CurrentsIRMS { get; set; }

        public decimal PowerKW { get; set; }
        public decimal PowerKWVoltage1 { get; set; }


        public decimal PercentageRo { get; set; }
        public decimal PercentagejXo { get; set; }
        public decimal PercentageZo { get; set; }

        public decimal ZBase { get; set; }
        public decimal ZOhms { get; set; }
        public Complex PercentageComplex { get; set; }

        public decimal FactorCorr { get; set; }


    }
}
