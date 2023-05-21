namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.CGD;
    using SPL.Domain.SPL.Tests.FPA;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;

    using SPL.Domain.SPL.Tests.PCI;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Domain.SPL.Tests.RDD;
    using SPL.Tests.Application.Commands.Tests;

    public class FPATestsHandler : IRequestHandler<FPATestsCommand, ApiResponse<ResultFPATests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public FPATestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultFPATests>> Handle(FPATestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ResultFPATests result = new();
                List<decimal> datosDesv = new();
                List<decimal> datosTen = new();
                List<ErrorColumns> listErrors = new();

                if (request.Data == null)
                {
                    return new ApiResponse<ResultFPATests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }
   
                for (int s = 0; s < request.Data.FPATestsDetails.FPADielectricStrength.Count; s++)
                {
                    int pos = request.Data.FPATestsDetails.FPADielectricStrength.IndexOf(request.Data.FPATestsDetails.FPADielectricStrength[s]) + 1;

                    decimal prom = (request.Data.FPATestsDetails.FPADielectricStrength[s].OneSt + request.Data.FPATestsDetails.FPADielectricStrength[s].TwoNd + request.Data.FPATestsDetails.FPADielectricStrength[s].ThreeRd + request.Data.FPATestsDetails.FPADielectricStrength[s].FourTh + request.Data.FPATestsDetails.FPADielectricStrength[s].FiveTh) / 5;
                    request.Data.FPATestsDetails.FPADielectricStrength[s].Average = Math.Round(prom, 1);

                    if (request.Data.FPATestsDetails.FPADielectricStrength[s].Average >= request.Data.FPATestsDetails.FPADielectricStrength[s].MinLimit1)
                    {
                        request.Data.FPATestsDetails.FPADielectricStrength[s].MinLimit2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Aceptado" : "Accepted";
                    }
                    else
                    {
                        listErrors.Add(new ErrorColumns(0, 0, "El valor de Promedio es menor al valor LimiteMin en el renglon nro.  " + pos + " de la sección nro. 2"));
                        request.Data.FPATestsDetails.FPADielectricStrength[s].MinLimit2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Rechazado" : "Rejected";
                    }
                }

                for (int s = 0; s < request.Data.FPATestsDetails.FPAPowerFactor.Count; s++)
                {
                    int pos = request.Data.FPATestsDetails.FPAPowerFactor.IndexOf(request.Data.FPATestsDetails.FPAPowerFactor[s]) + 1;

                    decimal factorPotencia = request.Data.FPATestsDetails.FPAPowerFactor[s].Measurement * request.Data.FPATestsDetails.FPAPowerFactor[s].Scale * request.Data.FPATestsDetails.FPAPowerFactor[s].CorrectionFactor;
                    request.Data.FPATestsDetails.FPAPowerFactor[s].PowerFactor = Math.Round(factorPotencia,3);

                    if (request.Data.FPATestsDetails.FPAPowerFactor[s].PowerFactor<= request.Data.FPATestsDetails.FPAPowerFactor[s].MaxLimitFP1)
                    {
                        request.Data.FPATestsDetails.FPAPowerFactor[s].MaxLimitFP2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Aceptado" : "Accepted";
                    }
                    else 
                    {
                        listErrors.Add(new ErrorColumns(0, 0, "El valor de Factor Potencia es mayor al valor LimiteMax en el renglon nro.  "+ pos+ " de la sección nro. 1"));
                        request.Data.FPATestsDetails.FPAPowerFactor[s].MaxLimitFP2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Rechazado" : "Rejected"; 
                    }
                }

                decimal promContWater = (request.Data.FPATestsDetails.FPAWaterContent.OneSt + request.Data.FPATestsDetails.FPAWaterContent.TwoNd + request.Data.FPATestsDetails.FPAWaterContent.ThreeRd) / 3;
                request.Data.FPATestsDetails.FPAWaterContent.Average = Math.Round(promContWater, 1);

                if (request.Data.FPATestsDetails.FPAWaterContent.Average <= request.Data.FPATestsDetails.FPAWaterContent.MaxLimit1)
                {
                    request.Data.FPATestsDetails.FPAWaterContent.MaxLimit2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Aceptado" : "Accepted";
                }
                else
                {
                    listErrors.Add(new ErrorColumns(0, 0, "El valor de Promedio es mayor al valor LimiteMax en la sección nro. 3"));
                    request.Data.FPATestsDetails.FPAWaterContent.MaxLimit2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Rechazado" : "Rejected";
                }

                if (request.Data.ClavePrueba != "DDP")
                {


                    if (request.Data.FPATestsDetails.FPAGasContent.Measurement <= request.Data.FPATestsDetails.FPAGasContent.Limit1)
                    {
                        request.Data.FPATestsDetails.FPAGasContent.Limit2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Aceptado" : "Accepted";
                    }
                    else
                    {
                        listErrors.Add(new ErrorColumns(0, 0, "El valor de Meidición es mayor al valor LimiteMax en la sección nro. 4"));

                        request.Data.FPATestsDetails.FPAGasContent.Limit2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Rechazado" : "Rejected";
                    }
                }
                else
                {
                    request.Data.FPATestsDetails.FPAGasContent.Limit2 = request.Data.KeyLanguage.ToUpper().Equals("ES") ? "Aceptado" : "Accepted";
                    request.Data.FPATestsDetails.FPAGasContent.Limit1 = 0;
                }

                result.FPATests = request.Data;
                result.Results = listErrors.ToList();
                
                return new ApiResponse<ResultFPATests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultFPATests>()
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
