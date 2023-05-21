namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Tests.Application.Queries.Tests;

    public class GetTestsHandler : IRequestHandler<GetTestsQuery, ApiResponse<List<Tests>>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public GetTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<Tests>>> Handle(GetTestsQuery request, CancellationToken cancellationToken)
        {

            try
            {
                if (string.IsNullOrEmpty(request.TypeReport))
                {
                    return new ApiResponse<List<Tests>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El tipo de reporte es requerido",
                        Structure = null
                    };
                }

                if (string.IsNullOrEmpty(request.KeyTests))
                {
                    return new ApiResponse<List<Tests>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El id de la prueba es requerido",
                        Structure = null
                    };
                }

                List<Tests> result = await this._infrastructure.GetTests(request.TypeReport, request.KeyTests);

                return new ApiResponse<List<Tests>>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<List<Tests>>()
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
