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

    public class GetColumsConfigCEMHandler : IRequestHandler<GetColumsConfigCEMQuery, ApiResponse<List<ColumnTitleCEMReports>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetColumsConfigCEMHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<ColumnTitleCEMReports>>> Handle(GetColumsConfigCEMQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Pos))
                {
                    return new ApiResponse<List<ColumnTitleCEMReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Posición es requerido",
                        Structure = null
                    };
                }
                if (string.IsNullOrEmpty(request.KeyLenguage))
                {
                    return new ApiResponse<List<ColumnTitleCEMReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Lenguaje es requerido",
                        Structure = null
                    };
                }
                List<ColumnTitleCEMReports> result = await this._infrastructure.GetColumnsConfigurableCEM(request.TypeTrafoId, request.KeyLenguage, request.Pos,request.PosSecundary, request.NoSerieNormal);

                return new ApiResponse<List<ColumnTitleCEMReports>>()
                {
                    Code = result.Count == 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count == 0 ? "No se encontraron resultados" : "Datos obtenidos exitosamente",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ColumnTitleCEMReports>>()
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
