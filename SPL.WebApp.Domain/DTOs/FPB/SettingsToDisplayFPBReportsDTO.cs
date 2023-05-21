namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class SettingsToDisplayFPBReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
        public List<ColumnTitleFPBReportsDTO> TitleOfColumns { get; set; }
        public long NextTestNumber { get; set; }
        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }
        public BaseTemplateDTO BaseTemplate { get; set; }
        public InformationArtifactDTO InfotmationArtifact { get; set; }

        public NozzlesByDesignDTO NozzlesByDesignDtos { get; set; }
    }
}
