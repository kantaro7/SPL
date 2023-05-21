namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Queries.Reports;

    public class GetColumsConfigRDTHandler : IRequestHandler<GetColumsConfigRDTQuery, ApiResponse<List<ColumnTitleRDTReports>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetColumsConfigRDTHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<ColumnTitleRDTReports>>> Handle(GetColumsConfigRDTQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.KeyTests))
                {
                    return new ApiResponse<List<ColumnTitleRDTReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El id de la prueba es requerido",
                        Structure = null
                    };
                }

                List<ColumnTitleRDTReports> result = await this._infrastructure.GetColumnsConfigurableRDT(request.KeyTests, request.DAngular, request.Rule, request.Lenguage);

                return new ApiResponse<List<ColumnTitleRDTReports>>()
                {
                    Code = result.Count() == 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count() == 0 ? "No se encontraron resultados" : "Datos obtenidos exitosamente",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ColumnTitleRDTReports>>()
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
