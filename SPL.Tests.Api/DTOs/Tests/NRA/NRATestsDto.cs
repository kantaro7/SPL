namespace SPL.Tests.Api.DTOs.Tests.NRA
{
    using System;
    using System.Collections.Generic;

    public class NRATestsDto
    {
        public string HV { get; set; }
        public string LV { get; set; }
        public string TV { get; set; }
        public string Losses { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Surface { get; set; }
        public decimal KFactor { get; set; }


        //********Inicio**************Tomados de SPL_INFO_LABORATORIO[HttpGet("getInformationLaboratories")] SPL.Configuration.Api
        public decimal SV { get; set; }
        public decimal Alfa { get; set; }
        //*********Fin*************Tomados de SPL_INFO_LABORATORIO[HttpGet("getInformationLaboratories")] SPL.Configuration.Api



        //********Inicio**************viene del obtener configuracion
        public decimal Diferencia { get; set; } 
        public decimal FactorCoreccion { get; set; }
        //********Fin**************viene del obtener configuracion

    

        public decimal Guaranteed { get; set; }
        public decimal AvgSoundPressureLevel  { get; set; }
        public decimal SoundPowerLevel { get; set; }



        public NRATestsDetailsOctDto NRATestsDetailsOcts { get; set; }
        public NRATestsDetailsRuiDto NRATestsDetailsRuis { get; set; }

    }
}
