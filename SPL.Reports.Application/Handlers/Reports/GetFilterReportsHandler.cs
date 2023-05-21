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

    public class GetFilterReportsHandler : IRequestHandler<GetFilterReportsQuery, ApiResponse<List<FilterReports>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetFilterReportsHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<FilterReports>>> Handle(GetFilterReportsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TypeReport))
                {
                    return new ApiResponse<List<FilterReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El tipo de reporte es requerido",
                        Structure = null
                    };
                }

                if (request.TypeReport.Trim().Length > 3)
                {
                    return new ApiResponse<List<FilterReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor tipo de reporte no puede ser mayor a 3 caracteres",
                        Structure = null
                    };
                }

                List<FilterReports> result = await this._infrastructure.GetFiltersReports(request.TypeReport);

                return new ApiResponse<List<FilterReports>>()
                {
                    Code =  (int)ResponsesID.exitoso,
                    Description = result.Count <= 0 ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<FilterReports>>()
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
