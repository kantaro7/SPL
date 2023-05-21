using SPL.Domain.SPL.Artifact.Nozzles;
using SPL.Domain.SPL.Artifact.PlateTension;
using SPL.Domain.SPL.Configuration;
using SPL.Domain.SPL.Reports.ROD;
using SPL.Tests.Api.DTOs.Tests.ROD;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.PCI
{
    public class PCITestsDetailsDto
    {
        public string Position { get; set; }

        public decimal Current { get; set; }
        public decimal Voltage { get; set; }
     

      

        public decimal PorcentajeR { get; set; }
        public decimal PorcentajeX { get; set; }
        public decimal PorcentajeZ { get; set; }

        public decimal LossesTotal { get; set; }
        public decimal LossesCorrected { get; set; }

        public decimal Lostkv { get; set; }
        public decimal CurrentIrms { get; set; }
        public decimal VoltagekVrms { get; set; }
        public decimal PowerKW { get; set; }
        public decimal VoltageAVG { get; set; }
        public decimal Wfe_Sen { get; set; }
        public decimal Wfe_20 { get; set; }
        public decimal Vnon_pri { get; set; }
        public decimal Inon_pi { get; set; }
        public decimal Vnon_sec { get; set; }
        public decimal Inon_sec { get; set; }
        public decimal Wcu_cor { get; set; }
        public decimal I2r_pri { get; set; }
        public decimal I2r_sec { get; set; }
        public decimal I2r_lv { get; set; }
        public decimal I2r_tot { get; set; }
        public decimal Wind { get; set; }
        public decimal I2r_tot_corr { get; set; }
        public decimal Wind_cor { get; set; }

        public PlateTensionDto PlateTensionsSec { get; set; }
        public RODTestsGeneralDto RODTestsGeneralsSec { get; set; }

    }
}
