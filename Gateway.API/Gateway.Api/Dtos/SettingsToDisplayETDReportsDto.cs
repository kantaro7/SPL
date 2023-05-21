namespace Gateway.Api.Dtos
{
    using System;
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;

    public class SettingsToDisplayETDReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public List<ETDConfigFileReportDto> ConfigurationReports { get; set; }
        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
        public decimal IdReg { get; set; }
        public decimal IdCorte { get; set; }
        public decimal? TorLim { get; set; }
        public List<decimal?> AwrLim { get; set; }
        public List<decimal?> HsrLim { get; set; }
        public List<decimal?> GradienteLim { get; set; }
        public decimal NivelTension { get; set; }
        public decimal Perdidas { get; set; }
        public decimal CapacidadPruebaAT { get; set; }
        public decimal CapacidadPruebaBT { get; set; }
        public decimal CapacidadPruebaTer { get; set; }
        public string TitPerdCorr { get; set; }
        public decimal DatoPerdCorr { get; set; }
        public string Enfriamiento { get; set; }
        public decimal Elevacion { get; set; }
        public decimal Altitud1 { get; set; }
        public string Altitud2 { get; set; }
        public string PosAT { get; set; }
        public string PosBT { get; set; }
        public List<string> Tiempo { get; set; }
        public DateTime UltimaHora { get; set; }
        public List<decimal> RadSuperior { get; set; }
        public List<decimal> RadInferior { get; set; }
        public List<decimal> AmbProm { get; set; }
        public List<decimal> TempTapa { get; set; }
        public List<decimal> TOR { get; set; }
        public List<decimal> AOR { get; set; }
        public List<decimal> BOR { get; set; }
        public decimal FactorAltitud { get; set; }
        public decimal FactorCorrecciónkW { get; set; }
        public List<decimal> ElevAceiteSup { get; set; }
        public List<decimal> ElevAceiteProm { get; set; }
        public List<decimal> ElevAceiteInf { get; set; }
        public List<decimal> ResistCorte { get; set; }
        public List<decimal> TempResistCorte { get; set; }
        public List<decimal> FactorK { get; set; }
        public List<decimal> ResistTCero { get; set; }
        public List<decimal> TempDev { get; set; }
        public List<decimal> GradienteDev { get; set; }
        public List<decimal> AORVKwA { get; set; }
        public List<decimal> TORVKwA { get; set; }
        public List<decimal> BORVKwA { get; set; }
        public List<decimal> ElevPromDev { get; set; }
        public List<decimal> ElevPtoMasCal { get; set; }
        public List<decimal> TempPromAceite { get; set; }
        public List<string> Terminal { get; set; }
        public List<string> UMResist { get; set; }
        public List<List<decimal>> TiempoResist { get; set; }
        public List<List<decimal>> Resistencia { get; set; }
        public List<List<decimal>> GraficaT { get; set; }
        public List<List<decimal>> GraficaR { get; set; }
        public bool TitGrafica { get; set; }
        public decimal PorcentajeRepresentanPerdidas { get; set; }
    }
}
