namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class SettingsToDisplayCEMReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }

        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }

        public List<ColumnTitleCEMReportsDTO> SecondaryPositions { get; set; }

        public BaseTemplateDTO BaseTemplate { get; set; }
        public InformationArtifactDTO InfotmationArtifact { get; set; }

        public string MessageInformation { get; set; }

        public int CodeInformation { get; set; }
    }
}
