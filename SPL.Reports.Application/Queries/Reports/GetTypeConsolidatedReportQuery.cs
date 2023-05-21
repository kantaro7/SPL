using MediatR;
using SPL.Domain;
using SPL.Domain.SPL.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Application.Queries.Reports
{

    public class GetTypeConsolidatedReportQuery : IRequest<ApiResponse<List<TypeConsolidatedReport>>>
    {
        public GetTypeConsolidatedReportQuery(string pNoSerie, string pKeyLenguage)
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
