namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Tests.Application.Commands.Tests;

    public class StabilizationDataTestsHandler : IRequestHandler<StabilizationDataTestsCommand, ApiResponse<ResultStabilizationDataTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public StabilizationDataTestsHandler(ITestsInfrastructure infrastructure) => _infrastructure = infrastructure;

        #region Methods
        public Task<ApiResponse<ResultStabilizationDataTests>> Handle(StabilizationDataTestsCommand request, CancellationToken cancellationToken)
        {
            ResultStabilizationDataTests result = new();

            try
            {
                if (request.Data == null)
                {
                    return Task.FromResult(new ApiResponse<ResultStabilizationDataTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    });
                }

                for (int r = 0; r < request.Data.StabilizationDataDetails.Count; r++)
                {
                    int countSum = 0;
                    decimal sum = 0;

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco1 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco1);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco2 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco2);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco3 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco3);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco4 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco4);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco5 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco5);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco6 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco6);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco7 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco7);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco8 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco8);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco9 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco9);
                    }
                    if (request.Data.StabilizationDataDetails[r].CabSupRadBco10 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabSupRadBco10);
                    }

                    request.Data.StabilizationDataDetails[r].PromRadSup = Math.Round(sum / countSum, 2);

                    countSum = 0;
                    sum = 0;

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco1 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco1);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco2 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco2);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco3 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco3);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco4 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco4);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco5 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco5);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco6 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco6);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco7 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco7);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco8 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco8);
                    }

                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco9 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco9);
                    }
                    if (request.Data.StabilizationDataDetails[r].CabInfRadBco10 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].CabInfRadBco10);
                    }

                    request.Data.StabilizationDataDetails[r].PromRadInf = Math.Round(sum / countSum, 2);

                    countSum = 0;
                    sum = 0;

                    if (request.Data.StabilizationDataDetails[r].Ambiente1 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].Ambiente1);
                    }

                    if (request.Data.StabilizationDataDetails[r].Ambiente2 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].Ambiente2);
                    }

                    if (request.Data.StabilizationDataDetails[r].Ambiente3 != null)
                    {
                        countSum++;
                        sum += Convert.ToDecimal(request.Data.StabilizationDataDetails[r].Ambiente3);
                    }

                    request.Data.StabilizationDataDetails[r].AmbienteProm = Math.Round(sum / countSum, 2);

                    request.Data.StabilizationDataDetails[r].Aor = Math.Round(request.Data.StabilizationDataDetails[r].TempTapa - (((+request.Data.StabilizationDataDetails[r].PromRadSup - request.Data.StabilizationDataDetails[r].PromRadInf) / 2) + request.Data.StabilizationDataDetails[r].AmbienteProm), 2);

                    request.Data.StabilizationDataDetails[r].Tor = Math.Round(request.Data.StabilizationDataDetails[r].TempTapa - request.Data.StabilizationDataDetails[r].AmbienteProm, 2);

                    request.Data.StabilizationDataDetails[r].Bor = Math.Round(request.Data.StabilizationDataDetails[r].PromRadInf - request.Data.StabilizationDataDetails[r].AmbienteProm, 2);

                    request.Data.StabilizationDataDetails[r].AorCorr = Math.Round((request.Data.StabilizationDataDetails[r].Aor * (request.Data.FactAlt - 1) * request.Data.FactEnf) + request.Data.StabilizationDataDetails[r].Aor, 2);

                    request.Data.StabilizationDataDetails[r].TorCorr = Math.Round((request.Data.StabilizationDataDetails[r].Tor * (request.Data.FactAlt - 1)
                    * request.Data.FactEnf) + request.Data.StabilizationDataDetails[r].Tor, 2);

                    decimal AO = request.Data.StabilizationDataDetails[r].TempTapa - Math.Round((request.Data.StabilizationDataDetails[r].PromRadSup - request.Data.StabilizationDataDetails[r].PromRadInf) / 2, 2);

                    request.Data.StabilizationDataDetails[r].Ao = AO;
                }

                //result.MessageErrors = listErrors;
                result.Results = request.Data;

                return Task.FromResult(new ApiResponse<ResultStabilizationDataTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                });

            }
            catch (Exception ex)
            {
                return Task.FromResult(new ApiResponse<ResultStabilizationDataTests>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                });
            }
        }

        #endregion

    }
}

