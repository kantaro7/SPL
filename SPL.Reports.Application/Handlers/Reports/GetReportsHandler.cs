namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Queries.Reports;

    public class GetReportsHandler : IRequestHandler<GetReportsQuery, ApiResponse<List<Reports>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetReportsHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<Reports>>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TypeReport))
                {
                    return new ApiResponse<List<Reports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El tipo de reporte es requerido",
                        Structure = null
                    };
                }

                List<Reports> result = await this._infrastructure.GetReports(request.TypeReport);

                return new ApiResponse<List<Reports>>()
                {
                    Code = result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Reports>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }
        #endregion
    }
}
