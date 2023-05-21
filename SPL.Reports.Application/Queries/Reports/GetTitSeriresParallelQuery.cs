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
    public class GetTitSeriresParallelQuery : IRequest<ApiResponse<TitSeriresParallelReports>>
    {
        public GetTitSeriresParallelQuery(string pClave, string pLenguage)
        {
          
            this.Clave = pClave;
            this.Language = pLenguage;
     

        }

        #region Constructor
        public string Clave { get; }
        public string Language { get; }

        #endregion

    }
}
