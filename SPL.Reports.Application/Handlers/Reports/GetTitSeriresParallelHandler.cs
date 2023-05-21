namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Queries.Reports;

    public class GetTitSeriresParallelHandler : IRequestHandler<GetTitSeriresParallelQuery, ApiResponse<TitSeriresParallelReports>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetTitSeriresParallelHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<TitSeriresParallelReports>> Handle(GetTitSeriresParallelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                TitSeriresParallelReports result = await this._infrastructure.GetTitSeriesParallel(request.Clave, request.Language);

                return new ApiResponse<TitSeriresParallelReports>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TitSeriresParallelReports>()
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
