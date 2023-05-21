using SPL.Domain.SPL.Masters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports
{
    public class ShowFilterReports
    {
        public string TypeReport { get; set; }
        public int DataInputType { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public List<GeneralProperties> Values { get; set; }

        public ShowFilterReports(string pTypeReport, int pDataInputType, string pName, int pPosition, List<GeneralProperties> pValues) {
            this.TypeReport = pTypeReport;
            this.DataInputType = pDataInputType;
            this.Name = pName;
            this.Position = pPosition;
            this.Values = pValues;
            
        }
    }
}
