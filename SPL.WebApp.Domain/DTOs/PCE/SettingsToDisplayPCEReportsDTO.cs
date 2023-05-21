namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class SettingsToDisplayPCEReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
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
        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }
        public BaseTemplateDTO BaseTemplate { get; set; }
        public InformationArtifactDTO InfotmationArtifact { get; set; }
    }
}
