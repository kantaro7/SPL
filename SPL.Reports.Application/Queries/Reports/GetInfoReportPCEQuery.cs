using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Reports;
using SPL.Domain.SPL.Reports.PCE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Reports.Application.Queries.Reports

{
    public class GetInfoReportPCEQuery : IRequest<ApiResponse<PCETestsGeneral>>
    {
        public GetInfoReportPCEQuery(string pNroSerie, string pKeyTest, bool pResult)
        {
         
            this.NroSerie = pNroSerie;
            this.KeyTest = pKeyTest;
            this.Result = pResult;

        }
        #region Constructor

     
        public string NroSerie { get; }
        public string KeyTest { get; }
        public bool Result { get; }
        #endregion

    }
}
