using MediatR;

using SPL.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Reports.Application.Queries.Reports
{
    public class GetGeneralTypesReportsQuery : IRequest<ApiResponse<List<SPL.Domain.SPL.Reports.InfoGeneralTypesReports>>>
    {
        public GetGeneralTypesReportsQuery(string pNroSerie,string pTypeReport)
        {
            NroSerie = pNroSerie;
            TypeReport = pTypeReport;

        }
        #region Constructor

        public string NroSerie { get; }
        public string TypeReport { get; }
        #endregion
    }
}