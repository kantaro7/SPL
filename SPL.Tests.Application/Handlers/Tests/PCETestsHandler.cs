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
    using SPL.Domain.SPL.Tests.PCE;
    using SPL.Tests.Application.Commands.Tests;

    public class PCETestsHandler : IRequestHandler<PCETestsCommand, ApiResponse<ResultPCETests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public PCETestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultPCETests>> Handle(PCETestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<ErrorColumns> listErrors = new();

                ResultPCETests result = new();

                List<PCETests> ResultPCETests = new();
                PCETests ObjRODTests = new();

                List<PCETestsDetails> ResultPCETestsDetails = new();

                ResultPCETests = request.Data.ToList();

                for (int i = 0; i < request.Data.Count; i++)
                {

                    ResultPCETestsDetails = new List<PCETestsDetails>();

                    for (int r = 0; r < request.Data[i].PCETestsDetails.Count; r++)
                    {
                        decimal PerdidasOnda = 100 / (50 + (50 * (request.Data[i].PCETestsDetails[r].TensionKVRMS / request.Data[i].PCETestsDetails[r].TensionKVAVG) * (request.Data[i].PCETestsDetails[r].TensionKVRMS / request.Data[i].PCETestsDetails[r].TensionKVAVG))) * request.Data[i].PCETestsDetails[r].PerdidasKW;
                        request.Data[i].PCETestsDetails[r].PerdidasOndaKW = PerdidasOnda;

                        decimal PerdidasCorr = PerdidasOnda * (1 + (Convert.ToDecimal(0.00065) * (request.Data[i].Temperatura - 20)));
                        request.Data[i].PCETestsDetails[r].Corregidas20KW = PerdidasCorr;

                        decimal Porc_Iexc = request.Data[i].PCETestsDetails[r].CorrienteIRMS / (request.Data[i].Capacidad / (request.Data[i].VoltajeBase * 1000) * 1000 / Convert.ToDecimal(Math.Sqrt(3))) * 100;
                        request.Data[i].PCETestsDetails[r].PorcentajeIexc = Porc_Iexc;

                       
                    }
                }

               

                for (int i = 0; i < request.Data.Count; i++)
                {
                    decimal CorrienteExcitación = request.Data[i].PCETestsDetails.FirstOrDefault(x => x.PorcentajeVN == 100).PorcentajeIexc;
                    if (CorrienteExcitación > request.Data[i].GarCExcitacion)
                    {
                        listErrors.Add(new ErrorColumns(1, 1, $"El valor de corriente de excitación ({CorrienteExcitación}) es mayor al de garantía de exictación, en la sección nro: "+(i+1)));
                    }

                    decimal valueMaxGarPer = request.Data[i].GarPerdidas + ((request.Data[i].PorGarPerdidasTolerancy/100) * request.Data[i].GarPerdidas);
                    decimal perdidasCorr = request.Data[i].PCETestsDetails.FirstOrDefault(x => x.PorcentajeVN == 100).Corregidas20KW;

                    if (perdidasCorr > request.Data[i].GarPerdidas && perdidasCorr > valueMaxGarPer)
                    {
                        listErrors.Add(new ErrorColumns(1, 1, $"El valor de pérdidas corregidas ({perdidasCorr}) es mayor al de garantía de pérdidas, en la sección nro: " + (i + 1)));
                    }
                }

                string keytest = request.Data.FirstOrDefault().KeyTest;

                if(keytest == "AYD")
                {
                    decimal correctedLosesSec1 = request.Data[0].PCETestsDetails.Where(x=>x.PorcentajeVN == 100).FirstOrDefault().Corregidas20KW ;
                    decimal correctedLosesSec2 = request.Data[1].PCETestsDetails.Where(x => x.PorcentajeVN == 100).FirstOrDefault().Corregidas20KW;

                    decimal execSec1 = request.Data[0].PCETestsDetails.Where(x => x.PorcentajeVN == 100).FirstOrDefault().PorcentajeIexc;
                    decimal execSec2 = request.Data[1].PCETestsDetails.Where(x => x.PorcentajeVN == 100).FirstOrDefault().PorcentajeIexc;

                    if(correctedLosesSec1!= 0 && correctedLosesSec2!= 0 && execSec1!= 0 && execSec2!=0)
                    {
                        var excedentePerdidas = Math.Round(((Math.Max(correctedLosesSec1, correctedLosesSec2) / Math.Min(correctedLosesSec1, correctedLosesSec2)) - 1) * 100,1);
                        var excedenteLexc = Math.Round(((Math.Max(execSec1, execSec2) / Math.Min(execSec1, execSec2)) - 1) * 100,1);

                        if(excedentePerdidas > request.Data[0].ToleranciaPer)
                        {
                            listErrors.Add(new ErrorColumns(77, 77, $"El excedente de pérdidas ({excedentePerdidas}) supera al valor definido: " + request.Data[0].ToleranciaPer));
                        }

                        if (excedenteLexc > request.Data[0].ToleranciaExec)
                        {
                            listErrors.Add(new ErrorColumns(77, 77, $"El excedente lexc ({excedenteLexc}) supera al valor definido: " + request.Data[0].ToleranciaExec));
                        }

                    }


                }


                ResultPCETests = request.Data.ToList();
                result.PCETests = ResultPCETests;
                result.Results = listErrors.ToList();

                return new ApiResponse<ResultPCETests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<ResultPCETests>()
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
