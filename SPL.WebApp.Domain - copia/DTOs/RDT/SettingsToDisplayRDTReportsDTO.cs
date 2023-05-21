namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class SettingsToDisplayRDTReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
        public List<ColumnTitleRDTReportsDTO> TitleOfColumns { get; set; }
        public long NextTestNumber { get; set; }
        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }
        public BaseTemplateDTO BaseTemplate { get; set; }
        public InformationArtifactDTO InfotmationArtifact { get; set; }
        public List<string> ValuePositions { get; set; }
    }
}
