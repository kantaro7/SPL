namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class SettingsToDisplayPCIReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public string CapacidadP { get; set; }
        public string TitPosPrim { get; set; }
        public decimal? Frecuency { get; set; }
        public decimal TitPerdCorr { get; set; }
        public decimal TitPerdTot { get; set; }
        public string TitPosSec { get; set; }
        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }
        public BaseTemplateDTO BaseTemplate { get; set; }
        public InformationArtifactDTO InfotmationArtifact { get; set; }
    }
}
