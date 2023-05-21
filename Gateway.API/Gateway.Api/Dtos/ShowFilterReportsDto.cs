using SPL.Domain.SPL.Masters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class ShowFilterReportsDto
    {
        public string TypeReport { get; set; }
        public int DataInputType { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public List<GeneralPropertiesDto> Values { get; set; }

        public ShowFilterReportsDto(string pTypeReport, int pDataInputType, string pName, int pPosition, List<GeneralPropertiesDto> pValues) {
            this.TypeReport = pTypeReport;
            this.DataInputType = pDataInputType;
            this.Name = pName;
            this.Position = pPosition;
            this.Values = pValues;
            
        }
    }
}
