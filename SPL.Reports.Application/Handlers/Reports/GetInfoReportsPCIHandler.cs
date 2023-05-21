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
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Reports.Application.Queries.Reports;

    public class GetInfoReportsPCIHandler : IRequestHandler<GetInfoReportPCIQuery, ApiResponse<PCITestGeneral>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetInfoReportsPCIHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<PCITestGeneral>> Handle(GetInfoReportPCIQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.KeyTest))
                {
                    return new ApiResponse<PCITestGeneral>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Prueba es requerido",
                        Structure = null
                    };
                }

                

                PCITestGeneral result = await this._infrastructure.GetInfoPCIReport(request.NroSerie, request.KeyTest, request.Result);

                return new ApiResponse<PCITestGeneral>()
                {
                    Code =  (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PCITestGeneral>()
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
