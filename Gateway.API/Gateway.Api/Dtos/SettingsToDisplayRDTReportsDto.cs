using Gateway.Api.Dtos.ArtifactDesing;

using SPL.Domain.SPL.Artifact.BaseTemplate;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class SettingsToDisplayRDTReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public List<ColumnTitleRDTReportsDto> TitleOfColumns { get; set; }
        public long NextTestNumber { get; set; }
        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
        public List<string> ValuePositions { get; set; }
    }
}
