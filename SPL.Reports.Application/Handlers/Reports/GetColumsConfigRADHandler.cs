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

    public class GetColumsConfigRADHandler : IRequestHandler<GetColumsConfigRADQuery, ApiResponse<List<ColumnTitleRADReports>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetColumsConfigRADHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<ColumnTitleRADReports>>> Handle(GetColumsConfigRADQuery request, CancellationToken cancellationToken)
        {
            try
            {

                if (string.IsNullOrEmpty(request.TypeUnit))
                {
                    return new ApiResponse<List<ColumnTitleRADReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Tipo de unidad es requerido",
                        Structure = null
                    };
                }
                if (string.IsNullOrEmpty(request.Lenguage))
                {
                    return new ApiResponse<List<ColumnTitleRADReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Lenguaje es requerido",
                        Structure = null
                    };
                }

                if (string.IsNullOrEmpty(request.ThirdWinding))
                {
                    return new ApiResponse<List<ColumnTitleRADReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Tercer devanado es requerido",
                        Structure = null
                    };
                }
                List<ColumnTitleRADReports> result = await this._infrastructure.GetColumnsConfigurableRAD(request.TypeUnit, request.Lenguage, request.ThirdWinding);

                return new ApiResponse<List<ColumnTitleRADReports>>()
                {
                    Code = result.Count == 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count == 0 ? "No se encontraron resultados" : "Datos obtenidos exitosamente",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ColumnTitleRADReports>>()
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
