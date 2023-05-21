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

    public class GetConfigurationReportsHandler : IRequestHandler<GetConfigurationReportsQuery, ApiResponse<List<ConfigurationReports>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetConfigurationReportsHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<ConfigurationReports>>> Handle(GetConfigurationReportsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TypeReport))
                {
                    return new ApiResponse<List<ConfigurationReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El tipo de reporte es requerido",
                        Structure = null
                    };
                }

                if (request.TypeReport.Trim().Length > 3)
                {
                    return new ApiResponse<List<ConfigurationReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor tipo de reporte no puede ser mayor a 3 caracteres",
                        Structure = null
                    };
                }

                List<ConfigurationReports> result = await this._infrastructure.GetConfigurationReport(request.TypeReport, request.KeyTest, request.NumberColumns);

                return new ApiResponse<List<ConfigurationReports>>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ConfigurationReports>>()
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
