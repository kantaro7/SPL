using SPL.Domain.SPL.Artifact.BaseTemplate;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class SettingsToDisplayRADReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public List<ColumnTitleRADReportsDto> TitleOfColumns { get; set; }
        public long NextTestNumber { get; set; }
        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
        public BaseTemplateDto BaseTemplate { get; set; }

    

        //public string[][] BeforeTestTable { get; set; }
        //public string[][] AfterTestTable { get; set; }

    }
}
