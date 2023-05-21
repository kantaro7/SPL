namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;

    public class SettingsToDisplayCEMReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }

        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
        public List<ColumnTitleCEMReportsDto> SecondaryPositions { get; set; }

        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }

        public string MessageInformation { get; set; }
        public int CodeInformation { get; set; }
    }
}
