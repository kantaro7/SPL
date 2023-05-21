namespace SPL.Reports.Api.DTOs.Reports.ISZ
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Artifact.ArtifactDesign;
    using global::SPL.Domain.SPL.Configuration;

    public class OutISZTestsDto
    {

        public string NominalAT { get; set; }
        public string NominalBT { get; set; }
        public string NominalTer { get; set; }
        public string PosicionMayotABT { get; set; }

        public string KeyTest { get; set; }
        public string Lenguage { get; set; }
        public decimal DegreesCor { get; set; }
        public string PosAT { get; set; }
        public string PosBT { get; set; }
        public string PosTER { get; set; }
        public string WindingEnergized { get; set; }
        public int QtyNeutral { get; set; }
        public string ImpedanceGar { get; set; }
        public string MaterialWinding { get; set; }

        public DateTime Date { get; set; }

        public decimal CapBaseMin { get; set; }
        public string UmCapBaseMin { get; set; }

        public decimal Temperature { get; set; }
        public string UmTemperature { get; set; }
        public string ValueNomPosAll { get; set; }
        public decimal PorcMinAcepImp { get; set; }
        public decimal PorcMaxAcepImp { get; set; }

        public List<SeccionesISZTestDetailsDto> SeccionesISZTestsDetails { get; set; }
    }

    public class SeccionesISZTestDetailsDto
    {
        public decimal? Porc_Z { get; set; }
        public decimal? Porc_X { get; set; }
        public decimal? Porc_R { get; set; }
        public List<ISZTestsDetailsDto> ISZTestsDetails { get; set; }

        public SeccionesISZTestDetailsDto()
        {
            this.ISZTestsDetails = new();
        }
    }
}
