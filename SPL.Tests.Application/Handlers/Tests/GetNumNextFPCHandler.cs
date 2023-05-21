﻿namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Tests.Application.Queries.Tests;

    public class GetNumNextFPCHandler : IRequestHandler<GetNumNextFPCQuery, ApiResponse<long>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public GetNumNextFPCHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(GetNumNextFPCQuery request, CancellationToken cancellationToken)
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

                if (string.IsNullOrEmpty(request.TypeUnit))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Tipo unidad es requerido",
                        Structure = 0
                    };
                }

                long result = await this._infrastructure.GetNumTestNextFPC(request.NroSerie, request.KeyTests, request.TypeUnit, request.Specification, request.Frequency,request.Lenguage);

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
