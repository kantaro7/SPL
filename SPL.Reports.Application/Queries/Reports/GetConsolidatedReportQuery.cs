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
    public class GetConsolidatedReportQuery : IRequest<ApiResponse<List<ConsolidatedReport>>>
    {
        public GetConsolidatedReportQuery(string pNoSerie, string pKeyLenguage)
        {
         
            this.NoSerie = pNoSerie;
    
            this.KeyLenguage = pKeyLenguage;

        }
        #region Constructor

     
        public string NoSerie { get; }
        public string KeyLenguage { get; }
        #endregion

    }
}
