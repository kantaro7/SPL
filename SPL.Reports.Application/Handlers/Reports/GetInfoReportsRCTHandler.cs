namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Domain.SPL.Reports.RCT;
    using SPL.Reports.Application.Queries.Reports;

    public class GetInfoReportsRCTHandler : IRequestHandler<GetInfoReportRCTQuery, ApiResponse<RCTTestsGeneral>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetInfoReportsRCTHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<RCTTestsGeneral>> Handle(GetInfoReportRCTQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.KeyTests))
                {
                    return new ApiResponse<RCTTestsGeneral>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Prueba es requerido",
                        Structure = null
                    };
                }

                RCTTestsGeneral result = await this._infrastructure.GetInfoRCTReport(request.NroSerie,request.KeyTests,request.Result);

                return new ApiResponse<RCTTestsGeneral>()
                {
                    Code =  (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RCTTestsGeneral>()
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
