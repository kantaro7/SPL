namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Tests.Application.Queries.Tests;

    public class GetNumNextDPRHandler : IRequestHandler<GetNumNextDPRQuery, ApiResponse<long>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public GetNumNextDPRHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(GetNumNextDPRQuery request, CancellationToken cancellationToken)
        {

            try
            {
                if (string.IsNullOrEmpty(request.NroSerie))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Número de serie es requerido",
                        Structure = 0
                    };
                }

                if (string.IsNullOrEmpty(request.KeyTests))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Prueba es requerido",
                        Structure = 0
                    };
                }

                if (string.IsNullOrEmpty(request.Lenguage))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Idioma es requerido",
                        Structure = 0
                    };
                }

                long result = await this._infrastructure.GetNumTestNextDPR(request.NroSerie, request.KeyTests, request.Lenguage, request.NoCycles, request.TotalTime, request.Interval, request.TimeLevel, request.OutputLevel, request.DescMayPc, request.DescMayMv, request.IncMaxPc, request.MeasurementType, request.TerminalsTest );

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
