namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Tests.Application.Commands.Tests;

    public class RANTestsHandler : IRequestHandler<RANTestsCommand, ApiResponse<ResultRANTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public RANTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultRANTests>> Handle(RANTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ErrorColumns errors;
                List<ErrorColumns> listErrors = new();

                ResultRANTests result = new();

                if (request.Data.Count == 0 )
                {
                    return new ApiResponse<ResultRANTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                for (int i = 0; i < request.Data.Count; i++)
                {

                    for (int r = 0; r < request.Data[i].RANTestsDetailsRAs.Count; r++)
                    {
                        if (request.Data[i].RANTestsDetailsRAs[r].Measurement < request.Data[i].RANTestsDetailsRAs[r].Limit)
                        {
                            errors = new ErrorColumns(i, r, "Error en valores de la prueba nro. " + (i+1) + " El valor está ubicado en la sección " + 1 + ", en la fila nro. : " + (r + 1) + " El campo Medición es menor que el Limite");
                            listErrors.Add(errors);
                        }
                     

                    }

                    for (int t = 0; t < request.Data[i].RANTestsDetailsTAs.Count; t++)
                    {
                        if (!request.Data[i].RANTestsDetailsTAs[t].Valid)
                        {
                            errors = new ErrorColumns(i, t, "Error en valores de la prueba nro. " + (i + 1) + " El valor está ubicado en la sección " + 2 + ", en la fila nro. : " + (t + 1) + " El campo Valido tiene seleccionado No");
                            listErrors.Add(errors);
                        }

                    }
                }
                result.MessageErrors = listErrors;
                    return new ApiResponse<ResultRANTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultRANTests>()
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
