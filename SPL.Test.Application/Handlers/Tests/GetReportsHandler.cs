using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Reports;
using SPL.Reports.Application.Queries.Reports;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Tests.Application.Handlers.Tests
{
    public class GetReportsHandler : IRequestHandler<GetReportsQuery, ApiResponse<List<SPL.Domain.SPL.Reports.Reports>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetReportsHandler(IReportsInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }


        #region Methods
        public async Task<ApiResponse<List<SPL.Domain.SPL.Reports.Reports>>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
        {

            try
            {

                List<SPL.Domain.SPL.Reports.Reports> result =  await _infrastructure.GetReports();

                return new ApiResponse<List<SPL.Domain.SPL.Reports.Reports>>()
                {
                    Code = result.Count <=0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<List<SPL.Domain.SPL.Reports.Reports>>()
                {
                    Code = (int)(ResponsesID.fallido),
                    Description = ex.Message,
                    Structure = null
                };

            }
        }

        
        #endregion
    }
}
