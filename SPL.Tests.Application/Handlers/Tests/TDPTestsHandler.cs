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
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.TDP;
    using SPL.Tests.Application.Commands.Tests;

    public class TDPTestsHandler : IRequestHandler<TDPTestsCommand, ApiResponse<ResultTDPTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public TDPTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultTDPTests>> Handle(TDPTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data == null)
                {
                    return new ApiResponse<ResultTDPTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }
                ResultTDPTests result = new ResultTDPTests();


                List<ErrorColumns> listErrors = new();
             
                List<bool> listHorizontal = new List<bool>();
                List<TDPValid20> listGen = new List<TDPValid20>();


                #region IncreaseTheLast20Minutes

                
                    for (int d = 4; d < request.Data.TDPTest.TDPTestsDetails.Count(); d++) /*Detalles*/
                    {
                        listHorizontal = new List<bool>();
                        for (int t = 0; t < request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals.Count(); t++) /*terminales*/
                        {
                            if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC is not null and not 0)
                            {
                                if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC > 0 && request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC > request.Data.TDPTest.TDPTestsDetails[d - 1].TDPTerminals[t].pC)
                                {
                                    listHorizontal.Add(true);
                                }
                                else
                                {
                                    listHorizontal.Add(false);
                                }
                            }

                        //if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].µV is not null and not 0)
                        //{
                        //    if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].µV > 0 && request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].µV > request.Data.TDPTest.TDPTestsDetails[d - 1].TDPTerminals[t].µV)
                        //    {
                        //        listHorizontal.Add(true);
                        //    }
                        //    else
                        //    {
                        //        listHorizontal.Add(false);
                        //    }
                        //}
                    }

                        listGen.Add(new TDPValid20() { TerminalValid = listHorizontal });
                    }

                

                decimal RenglonesValid = Math.Round(Convert.ToDecimal(request.Data.TotalTime / request.Data.Interval) - (20 / request.Data.Interval), 0);



                int sum = 0;
                for (int g = Convert.ToInt32(RenglonesValid) - 2; g < listGen.Count(); g++)
                {
                    listGen[g].ResultTerminalValid = 0;
                    int valids = 0;
                    for (int d = 0 ; d < listGen[g].TerminalValid.Count(); d++)
                    {
                        if (listGen[g].TerminalValid[d])
                        {
                            valids++;
                        }
                    }
                    if(valids == listGen[g].TerminalValid.Count())
                    {
                        listGen[g].ResultTerminalValid = 1;
                    }
                    sum = sum + listGen[g].ResultTerminalValid;
                }

                int validPc = sum >= Math.Round(20M / request.Data.Interval) + 1 ? 1 : 0;



                if (validPc == 1)
                {
                    listErrors.Add(new ErrorColumns(1, 1, "El resultado de Validar incremento a los 20 minutos fue rechazado"));
                }

                #endregion


                #region ValidateDownloads
                List<bool> listHorizontalDownloads = new List<bool>();
                
                for (int d = 3; d < request.Data.TDPTest.TDPTestsDetails.Count(); d++)
                {
                    listHorizontal = new List<bool>();
                    for (int t = 0; t < request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals.Count(); t++)
                    {
                        //if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC != null)
                        //{
                        //    listHorizontalDownloads.Add(request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC > request.Data.DescMayPc);
                        //}
                        //if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].µV != null)
                        //{
                        //    listHorizontalDownloads.Add(request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].µV > request.Data.DescMayMv);
                        //}






                        if (request.Data.MeasurementType.ToUpper().Equals("picolumns".ToUpper()))
                        {

                            if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC != null)
                            {
                                listHorizontalDownloads.Add(request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC > request.Data.DescMayPc);
                            }
                        }
                        else if (request.Data.MeasurementType.ToUpper().Equals("microvolts".ToUpper()))
                        {
                            if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC != null)
                            {
                                listHorizontalDownloads.Add(request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC > request.Data.DescMayMv);
                            }
                        }
                        else
                        {
                            if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC != null)
                            {
                                listHorizontalDownloads.Add(request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].pC > request.Data.DescMayPc);
                            }
                            if (request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].µV != null)
                            {
                                listHorizontalDownloads.Add(request.Data.TDPTest.TDPTestsDetails[d].TDPTerminals[t].µV > request.Data.DescMayMv);
                            }

                        }
                    }
                }

                int validTension = listHorizontalDownloads.Count(x => x) > request.Data.Tolerance ? 1 : 0;

                if (validTension == 1)
                {
                    listErrors.Add(new ErrorColumns(1, 1, "El resultado de Validar descargas mayores de pc y mv fue rechazado"));
                }
                #endregion




                if (request.Data.MeasurementType.ToUpper().Equals("picolumns".ToUpper()))
                {
                    #region ValidateIncreaseInPc

                    List<decimal> pcs1 = new();
                    List<decimal> pcs2 = new();
                    List<decimal> pcs3 = new();
                    int validPcMM;
                    if (request.Data.TDPTest.TDPTestsDetails[1].TDPTerminals.First().pC is null or 0)
                    {
                        validPcMM = 0;
                    }

                    for (int i = 3; i < request.Data.TDPTest.TDPTestsDetails.Count; i++)
                    {
                        for (int j = 0; j < request.Data.TDPTest.TDPTestsDetails[i].TDPTerminals.Count; j++)
                        {
                            switch (j)
                            {
                                case 0:
                                    pcs1.Add(request.Data.TDPTest.TDPTestsDetails[i].TDPTerminals[j].pC ?? 0);
                                    break;
                                case 1:
                                    pcs2.Add(request.Data.TDPTest.TDPTestsDetails[i].TDPTerminals[j].pC ?? 0);
                                    break;
                                case 2:
                                    pcs3.Add(request.Data.TDPTest.TDPTestsDetails[i].TDPTerminals[j].pC ?? 0);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    List<decimal> diffs = new();

                    if (pcs1.Count() > 0)
                    {
                        diffs.Add(pcs1.Max() - pcs1.Min());
                    }

                    if (pcs2.Count() > 0)
                    {
                        diffs.Add(pcs2.Max() - pcs2.Min());
                    }

                    if (pcs3.Count() > 0)
                    {
                        diffs.Add(pcs3.Max() - pcs3.Min());
                    }

                    validPcMM = diffs.Exists(x => x > request.Data.IncMaxPc) ? 1 : 0;
                    if (validPcMM == 1)
                    {
                        listErrors.Add(new ErrorColumns(1, 1, "El resultado de Validar Incremento máximo de pC fue rechazado"));
                    }
                    #endregion
                }


                result.Results = listErrors.ToList();

                return new ApiResponse<ResultTDPTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                 };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultTDPTests>()
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
