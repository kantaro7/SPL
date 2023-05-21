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

    public class GetColumsConfigFPCHandler : IRequestHandler<GetColumsConfigFPCQuery, ApiResponse<List<ColumnTitleFPCReports>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetColumsConfigFPCHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<ColumnTitleFPCReports>>> Handle(GetColumsConfigFPCQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TypeUnit))
                {
                    return new ApiResponse<List<ColumnTitleFPCReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Tipo de unidad es requerido",
                        Structure = null
                    };
                }
                if (string.IsNullOrEmpty(request.Lenguage))
                {
                    return new ApiResponse<List<ColumnTitleFPCReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Lenguaje es requerido",
                        Structure = null
                    };
                }
                List<ColumnTitleFPCReports> result = await this._infrastructure.GetColumnsConfigurableFPC(request.TypeUnit, request.Lenguage);

                return new ApiResponse<List<ColumnTitleFPCReports>>()
                {
                    Code = result.Count == 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count == 0 ? "No se encontraron resultados" : "Datos obtenidos exitosamente",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ColumnTitleFPCReports>>()
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
