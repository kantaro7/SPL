namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;

    public class SettingsToDisplayPCEReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }

        public decimal CapMinima { get; set; }
        public string Pos_AT { get; set; }
        public string Pos_BT { get; set; }
        public string Pos_TER { get; set; }
        public decimal VoltajeBase { get; set; }
        public decimal? Frecuencia { get; set; }
        public decimal? Gar_Perdidas { get; set; }
        public decimal? Tol_Gar_Perdidas { get; set; }
        public decimal? Gar_Cexcitacion { get; set; }

      
        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }

        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
    }
}
