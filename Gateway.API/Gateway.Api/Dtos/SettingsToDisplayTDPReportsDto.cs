namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;

    public class SettingsToDisplayTDPReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public string VoltageLevel { get; set; }
        public decimal? Frequency { get; set; }
        public string TitTerminal1 { get; set; }
        public string TitTerminal2 { get; set; }
        public string TitTerminal3 { get; set; }
        public string TitTerminal4 { get; set; }
        public string TitTerminal5 { get; set; }
        public string TitTerminal6 { get; set; }
        public string UMed1 { get; set; }
        public string UMed2 { get; set; }
        public string UMed3 { get; set; }
        public string UMed4 { get; set; }
        public string UMed5 { get; set; }
        public string UMed6 { get; set; }
        public List<string> Times { get; set; }
        public List<string> Voltages { get; set; }

        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
       

        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
    }
}
