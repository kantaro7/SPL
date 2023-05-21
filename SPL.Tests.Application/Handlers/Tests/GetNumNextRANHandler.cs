namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Tests.Application.Queries.Tests;

    public class GetNumNextRANHandler : IRequestHandler<GetNumNextRANQuery, ApiResponse<long>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public GetNumNextRANHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(GetNumNextRANQuery request, CancellationToken cancellationToken)
        {

            try
            {
                if (string.IsNullOrEmpty(request.NroSerie))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie es requerido",
                        Structure = 0
                    };
                }

                if (string.IsNullOrEmpty(request.KeyTests))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "La prueba es requerido",
                        Structure = 0
                    };
                }

                if (string.IsNullOrEmpty(request.Lenguage))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El idioma es requerido",
                        Structure = 0
                    };
                }

                if (request.NumberMeasurements == 0)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "La cantidad de mediciones es requerido",
                        Structure = 0
                    };
                }

                long result = await this._infrastructure.GetNumTestNextRAN(request.NroSerie, request.KeyTests, request.Lenguage, request.NumberMeasurements);

                return new ApiResponse<long>()
                {
                    Code = result == 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == 0 ? "No se encontraron resultados" : "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<long>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = 0
                };

            }
        }
        #endregion
    }
}
