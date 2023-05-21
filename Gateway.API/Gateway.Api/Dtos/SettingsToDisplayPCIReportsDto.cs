namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;

    public class SettingsToDisplayPCIReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public string CapacidadP { get; set; }
        public string TitPosPrim { get; set; }
        public decimal? Frecuency { get; set; }
        public decimal TitPerdCorr { get; set; }
        public decimal TitPerdTot { get; set; }
        public string TitPosSec { get; set; }
        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
    }
}
