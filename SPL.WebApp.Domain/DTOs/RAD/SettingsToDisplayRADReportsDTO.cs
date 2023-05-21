namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class SettingsToDisplayRADReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
        public List<ColumnTitleRADReportsDTO> TitleOfColumns { get; set; }
        public long NextTestNumber { get; set; }
        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }
        public BaseTemplateDTO BaseTemplate { get; set; }
    }
}
