using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs.NRA
{
    public class NRATestsDTO
    {
        public string HV { get; set; }
        public string LV { get; set; }
        public string TV { get; set; }
        public string Losses { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Surface { get; set; }
        public decimal KFactor { get; set; }
        public string UmLength { get; set; }
        public string UmSurface { get; set; }
        public string UmHeight { get; set; }
        //********Inicio**************Tomados de SPL_INFO_LABORATORIO[HttpGet("getInformationLaboratories")] SPL.Configuration.Api
        public decimal SV { get; set; }
        public decimal Alfa { get; set; }
        //*********Fin*************Tomados de SPL_INFO_LABORATORIO[HttpGet("getInformationLaboratories")] SPL.Configuration.Api



        //********Inicio**************viene del obtener configuracion
        public decimal Diferencia { get; set; }
        public decimal FactorCoreccion { get; set; }
        //********Fin**************viene del obtener configuracion



        public decimal Guaranteed { get; set; }
        public decimal AvgSoundPressureLevel { get; set; }
        public decimal SoundPowerLevel { get; set; }



        public NRATestsDetailsOctDTO NRATestsDetailsOcts { get; set; }
        public NRATestsDetailsRuiDTO NRATestsDetailsRuis { get; set; }
    }
}
