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
    public class GetConfigFileETDQuery : IRequest<ApiResponse<List<SPL.Domain.SPL.Reports.ETD.ETDConfigFileReport>>>
    {
        public GetConfigFileETDQuery()
        {
       
          

        }
        #region Constructor


        #endregion
    }
}