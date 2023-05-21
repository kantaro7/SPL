﻿namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;

    public class SettingsToDisplayRDDReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public string TitConexion { get; set; }
        public string Phase { get; set; }

        //***Los datos que se deben mostrar en mas de una seccion no van aqui. Se hace mas facil hacerlos a nivel de fornt
        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
    }
}
