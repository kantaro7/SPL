namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class SettingsToDisplayISZReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }

        public decimal BaseCapacity { get; set; }
        public decimal XXDegreesCorrection { get; set; }
        public string XXWindingEnergized { get; set; }
        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }
        public BaseTemplateDTO BaseTemplate { get; set; }
        public InformationArtifactDTO InfotmationArtifact { get; set; }
    }
}
