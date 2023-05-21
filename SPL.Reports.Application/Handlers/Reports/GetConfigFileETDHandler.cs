namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.ETD;
    using SPL.Reports.Application.Queries.Reports;

    public class GetConfigFileETDHandler : IRequestHandler<GetConfigFileETDQuery, ApiResponse<List<ETDConfigFileReport>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetConfigFileETDHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<ETDConfigFileReport>>> Handle(GetConfigFileETDQuery request, CancellationToken cancellationToken)
        {
            try
            {
               

                List<ETDConfigFileReport> result = await this._infrastructure.GetConfigFileEtd();

                return new ApiResponse<List<ETDConfigFileReport>>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ETDConfigFileReport>>()
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
