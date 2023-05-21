namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Reports.Application.Queries.Reports;

    public class GetInfoReportsFPCHandler : IRequestHandler<GetInfoReportFPCQuery, ApiResponse<FPCTestsGeneral>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetInfoReportsFPCHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<FPCTestsGeneral>> Handle(GetInfoReportFPCQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.KeyTest))
                {
                    return new ApiResponse<FPCTestsGeneral>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Prueba es requerido",
                        Structure = null
                    };
                }

                

                FPCTestsGeneral result = await this._infrastructure.GetInfoFPCReport(request.NroSerie, request.KeyTest, request.Lenguage, request.UnitType, request.Frecuency, request.Result);

                return new ApiResponse<FPCTestsGeneral>()
                {
                    Code =  (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<FPCTestsGeneral>()
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
