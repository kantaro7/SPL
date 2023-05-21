namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Queries.Reports;

    public class GetCorrectionFactoHandler : IRequestHandler<GetCorrectionFactorQuery, ApiResponse<CorrectionFactorReports>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetCorrectionFactoHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<CorrectionFactorReports>> Handle(GetCorrectionFactorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                CorrectionFactorReports result = await this._infrastructure.GetCorrectionFactor(request.Clave, request.Language);
                
                return new ApiResponse<CorrectionFactorReports>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CorrectionFactorReports>()
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
