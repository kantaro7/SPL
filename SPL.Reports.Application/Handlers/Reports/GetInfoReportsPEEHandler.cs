namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Domain.SPL.Reports.PEE;
    using SPL.Reports.Application.Queries.Reports;

    public class GetInfoReportsPEEHandler : IRequestHandler<GetInfoReportPEEQuery, ApiResponse<PEETestsGeneral>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetInfoReportsPEEHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<PEETestsGeneral>> Handle(GetInfoReportPEEQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.KeyTest))
                {
                    return new ApiResponse<PEETestsGeneral>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Prueba es requerido",
                        Structure = null
                    };
                }



                PEETestsGeneral result = await this._infrastructure.GetInfoPEEReport(request.NroSerie, request.KeyTest, request.Result);

                return new ApiResponse<PEETestsGeneral>()
                {
                    Code =  (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PEETestsGeneral>()
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
