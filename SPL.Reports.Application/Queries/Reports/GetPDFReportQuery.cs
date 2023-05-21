using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Reports;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Reports.Application.Queries.Reports
{
    public class GetPDFReportQuery : IRequest<ApiResponse<ReportPDF>>
    {
        public GetPDFReportQuery(long pCode, string pTypeReport)
        {

            Code = pCode;
            TypeReport = pTypeReport;

        }
        #region Constructor

 
        public long Code { get; }
        public string TypeReport { get; }
        #endregion
    }
}