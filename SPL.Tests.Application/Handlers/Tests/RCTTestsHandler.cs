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
    using SPL.Domain.SPL.Tests.RCT;
    using SPL.Tests.Application.Commands.Tests;

    public class RCTTestsHandler : IRequestHandler<RCTTestsCommand, ApiResponse<ResultRCTTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public RCTTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultRCTTests>> Handle(RCTTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.Data.Count == 0)
                {
                    return new ApiResponse<ResultRCTTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                decimal valueAcept = request.Data[0].AcceptanceValue / 100;

                List<ErrorColumns> listErrors = new();

                ResultRCTTests result = new();
             

                List<RCTTests> ResultFPCTests = new();

        
                ResultFPCTests = request.Data.ToList();
                List<decimal> Resistencias = new() { };
                decimal dMaxColumna, dMinColumna;
                for (int i = 0; i < request.Data[0].RCTTestsDetails.Count; i++)
                {
                    dMaxColumna = dMinColumna = 0;
                    Resistencias = new() { };
                    // Fase A
                    Resistencias.Add(request.Data[0].RCTTestsDetails[i].Resistencia);
                    // Fase B
                    Resistencias.Add(request.Data[1].RCTTestsDetails[i].Resistencia);
                    // Fase C
                    Resistencias.Add(request.Data[2].RCTTestsDetails[i].Resistencia);

                    dMaxColumna = Resistencias.Max();
                    dMinColumna = Resistencias.Min();

                    if (dMaxColumna == 0 || dMinColumna == 0)
                    {
                        listErrors.Add(new ErrorColumns(i, i, " Al dividir el valor máximo entre el mínimo, en la columna  " + (i + 1) + " ,da error de división entre 0, por favor chequee los valores"));
                    }

                    if (((dMaxColumna / dMinColumna) - 1) > valueAcept)
                    {
                        listErrors.Add(new ErrorColumns(i, i, "El valor máximo de la columna nro "+ (i + 1) + " supera al valor de aceptación"));
                    }
                }

          
                result.Results = listErrors.ToList();

                return new ApiResponse<ResultRCTTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                 };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultRCTTests>()
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
