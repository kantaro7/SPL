using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Configuration.Application.Queries.Configuration
{
    public class GetNozzleBrandsQuery : IRequest<ApiResponse<List<NozzleMarks>>>
    {
        public GetNozzleBrandsQuery(long pIdMark)
        {
            this.IdMark = pIdMark;
        

        }
        #region Constructor
        public long IdMark { get; }
     

        #endregion
    }
}
