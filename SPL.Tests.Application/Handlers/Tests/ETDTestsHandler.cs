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
    using SPL.Domain.SPL.Tests.ETD;
    using SPL.Domain.SPL.Tests.RCT;
    using SPL.Tests.Application.Commands.Tests;

    public class ETDTestsHandler : IRequestHandler<ETDTestsCommand, ApiResponse<ResultETDTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public ETDTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultETDTests>> Handle(ETDTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data == null)
                {
                    return new ApiResponse<ResultETDTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                ResultETDTests result = new() {
                    ETDTestsGeneral = request.Data,
                    Results = new()
                };
                // Funcion local obtiene titulo de la seccion
                static string getSectionName(int section) => section switch
                    {
                        1 => "Alta Tension",
                        2 => "Baja Tension",
                        3 => "Terciario",
                        _ => "NA",
                    };
                
                // Validando TOR
                bool torVal = true;
                foreach (ETDTestsDetails item in result.ETDTestsGeneral.ETDTests[0].ETDTestsDetails)
                {
                    decimal vali = (decimal)((item.Tor * result.ETDTestsGeneral.FactorKw) + item.Tor);
                    if(vali > result.ETDTestsGeneral.TorLim)
                    {
                        torVal = false;
                    }
                }

                if (!torVal)
                {
                    result.Results.Add(new ErrorColumns(0, 0, "Alguno de los valores de TOR es mayor al valor límite establecido por diseño para TOR"));
                    result.ETDTestsGeneral = result.ETDTestsGeneral;
                    result.ETDTestsGeneral.ETDTests.ForEach(x => x.Resultado = false);
                    result.Results = result.Results;
                    return new ApiResponse<ResultETDTests>()
                    {
                        Code = (int)ResponsesID.exitoso,
                        Description = "Resultado exitoso",
                        Structure = result
                    };
                }
                else
                {
                    int sections = result.ETDTestsGeneral.ETDTests.Count;
                    result.ETDTestsGeneral.ETDTests.ForEach(x => x.Resultado = true);
                    for (int section = 1; section < sections; section++)
                    {
                        // Validando ElevPromDev
                        if(result.ETDTestsGeneral.ETDTests[section].ElevPromDev > result.ETDTestsGeneral.AwrLim[section - 1])
                        {
                            result.Results.Add(new ErrorColumns(0, 0, $"El valor de la Elevación Promedio del Devanado en la seccion de {getSectionName(section)} es mayor al valor límite establecido por diseño (AWR: {result.ETDTestsGeneral.AwrLim[section - 1]})"));
                            result.ETDTestsGeneral.ETDTests[section].Resultado = false;
                        }

                        // Validando ElevPtoMasCal
                        if (result.ETDTestsGeneral.ETDTests[section].ElevPtoMasCal > result.ETDTestsGeneral.HsrLim[section - 1])
                        {
                            result.Results.Add(new ErrorColumns(0, 0, $"El valor de la Elevación del Punto más Caliente en la seccion de {getSectionName(section)} es mayor al valor límite establecido por diseño (HSR: {result.ETDTestsGeneral.HsrLim[section - 1]})"));
                            result.ETDTestsGeneral.ETDTests[section].Resultado = false;
                        }

                        // Validando GradienteDev
                        if (result.ETDTestsGeneral.ETDTests[section].GradienteDev > result.ETDTestsGeneral.GradienteLim[section - 1])
                        {
                            result.Results.Add(new ErrorColumns(0, 0, $"El valor del Gradiente del Devanado en la seccion de {getSectionName(section)} es mayor al valor límite establecido por diseño (Gradiente: {result.ETDTestsGeneral.GradienteLim[section - 1]})"));
                        }
                    }

                    return new ApiResponse<ResultETDTests>()
                    {
                        Code = (int)ResponsesID.exitoso,
                        Description = "Resultado exitoso",
                        Structure = result
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultETDTests>()
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
