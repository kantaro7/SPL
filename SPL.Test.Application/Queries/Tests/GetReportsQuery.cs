using MediatR;

using SPL.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Tests.Application.Queries.Tests
{
    public class GetReportsQuery : IRequest<ApiResponse<List<SPL.Domain.SPL.Reports.Reports>>>
    {
        public GetReportsQuery()
        {    

        }
  

    }
}
