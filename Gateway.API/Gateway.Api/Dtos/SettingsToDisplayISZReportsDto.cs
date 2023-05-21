namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;

    public class SettingsToDisplayISZReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }

        public decimal BaseCapacity { get; set; }
        public decimal XXDegreesCorrection { get; set; }
        public string XXWindingEnergized { get; set; }

      
        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }

        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
    }
}
