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
    using SPL.Reports.Application.Queries.Reports;

    public class GetInfoReportsCGDHandler : IRequestHandler<GetInfoReportCGDQuery, ApiResponse<CGDTestsGeneral>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetInfoReportsCGDHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<CGDTestsGeneral>> Handle(GetInfoReportCGDQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.KeyTests))
                {
                    return new ApiResponse<CGDTestsGeneral>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Prueba es requerido",
                        Structure = null
                    };
                }

                

                CGDTestsGeneral result = await this._infrastructure.GetInfoCGDReport(request.NroSerie,request.KeyTests,request.Result);

                return new ApiResponse<CGDTestsGeneral>()
                {
                    Code =  (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CGDTestsGeneral>()
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
