namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;

    public class SettingsToDisplayFPAReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public string OilType { get; set; } 
        public decimal? Measurement { get; set; }

        public decimal PorcMaximumLimit { get; set; }

        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
    }
}
