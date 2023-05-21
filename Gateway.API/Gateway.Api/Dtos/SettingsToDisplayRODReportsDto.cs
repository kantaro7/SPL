namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;

    public class SettingsToDisplayRODReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public List<ColumnTitleRODReportsDto> TitleOfColumns { get; set; }
        public long NextTestNumber { get; set; }
        public string TestVoltage { get; set; }
        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
    }
}
