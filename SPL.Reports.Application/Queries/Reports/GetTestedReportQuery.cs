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
    public class GetTestedReportQuery : IRequest<ApiResponse<List<ConsolidatedReport>>>
    {
        public GetTestedReportQuery(string pNoSerie)
        {
         
            this.NoSerie = pNoSerie;

        }
        #region Constructor
     
        public string NoSerie { get; }
        #endregion

    }
}
