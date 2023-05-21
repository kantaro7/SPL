using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports
{
    public class RANReportDetailsDto
    {

        public List<RANReportDetailsRADto> RANTestsDetailsRAs { get; set; }
        public List<RANReportDetailsTADto> RANTestsDetailsTAs { get; set; }
   

    }

}
