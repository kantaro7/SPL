namespace SPL.Domain.SPL.Reports.NRA
{
    using System;
    using System.Collections.Generic;

    public class NRATests
    {
        public string HV { get; set; }
        public string LV { get; set; }
        public string TV { get; set; }
        public string Losses { get; set; }
        public decimal Height { get; set; }
        public string UmHeight { get; set; }

        public decimal Length { get; set; }
        public string UmLength { get; set; }
        public decimal Surface { get; set; }
        public string UmSurface { get; set; }
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
        public decimal AvgSoundPressureLevel { get; set; }
        public decimal SoundPowerLevel { get; set; }



        public NRATestsDetailsOct NRATestsDetailsOcts { get; set; }
        public NRATestsDetailsRui NRATestsDetailsRuis { get; set; }


    }
}
