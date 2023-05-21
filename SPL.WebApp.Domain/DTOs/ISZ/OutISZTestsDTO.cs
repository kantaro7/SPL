namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class OutISZTestsDTO
    {
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
        public InformationArtifactDTO GeneralArtifact { get; set; }
        public List<ValidationTestsIszDTO> ValidationTestsIsz { get; set; }
        public decimal CapBaseMin { get; set; }
        public decimal PorcMinAcepImp { get; set; }
        public decimal PorcMaxAcepImp { get; set; }
        public string UmCapBaseMin { get; set; }
        public decimal Temperature { get; set; }
        public string UmTemperature { get; set; }
        public string ValueNomPosAll { get; set; }
        public string PosicionMayotABT { get; set; }

        public string NominalAT { get; set; }
        public string NominalBT { get; set; }
        public string NominalTer { get; set; }
        public List<SeccionesISZTestDetailsDTO> SeccionesISZTestsDetails { get; set; }
    }

    public class SeccionesISZTestDetailsDTO
    {
        public decimal? Porc_Z { get; set; }
        public decimal? Porc_X { get; set; }
        public decimal? Porc_R { get; set; }

        public List<ISZTestsDetailsDTO> ISZTestsDetails { get; set; }

        public SeccionesISZTestDetailsDTO()
        {
            this.ISZTestsDetails = new();
        }
    }
}
