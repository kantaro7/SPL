using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs
{
    public class SettingsToDisplayARFReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public string VoltageLevel { get; set; }
        public string Team { get; set; }

        //***Los datos que se deben mostrar en mas de una seccion no van aqui. Se hace mas facil hacerlos a nivel de fornt
        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }
        public BaseTemplateDTO BaseTemplate { get; set; }
        public InformationArtifactDTO InfotmationArtifact { get; set; }
        
        public string CeldaAceite1 { get; set; }
        public string CeldaAceite2 { get; set; }
        public string CeldaBoquilla1 { get; set; }
        public string CeldaBoquilla2 { get; set; }
        public string CeldaNucleo1 { get; set; }
        public string CeldaNucleo2 { get; set; }
        public string CeldaTer1 { get; set; }
        public string CeldaTer2 { get; set; }
        public string TitTer1 { get; set; }
        public string TitTer2 { get; set; }
        public string CeldaUmTemp1 { get; set; }
        public string CeldaUmTemp2 { get; set; }
    }
}
