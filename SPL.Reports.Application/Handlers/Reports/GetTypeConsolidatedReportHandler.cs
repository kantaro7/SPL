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

    public class GetTypeConsolidatedReportHandler : IRequestHandler<GetTypeConsolidatedReportQuery, ApiResponse<List<TypeConsolidatedReport>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetTypeConsolidatedReportHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<TypeConsolidatedReport>>> Handle(GetTypeConsolidatedReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NoSerie))
                {
                    return new ApiResponse<List<TypeConsolidatedReport>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El numero de serie es requerido",
                        Structure = null
                    };
                }

                if (string.IsNullOrEmpty(request.KeyLenguage))
                {
                    return new ApiResponse<List<TypeConsolidatedReport>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El idioma es requerido",
                        Structure = null
                    };
                }

                List<TypeConsolidatedReport> result = await this._infrastructure.GetTypeConsolidatedReport(request.NoSerie, request.KeyLenguage);

                return new ApiResponse<List<TypeConsolidatedReport>>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TypeConsolidatedReport>>()
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
