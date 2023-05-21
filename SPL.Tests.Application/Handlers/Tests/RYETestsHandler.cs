 namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.ISZ;
    using SPL.Tests.Application.Commands.Tests;

    public class RYETestsHandler : IRequestHandler<RYETestsCommand, ApiResponse<ResultRYETests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public RYETestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultRYETests>> Handle(RYETestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data ==null)
                {
                    return new ApiResponse<ResultRYETests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }


                List<ErrorColumns> listErrors = new();

                ResultRYETests result = new();
             

                OutRYETests ResultISZTests = new();

          

            

                ResultISZTests = request.Data;

                decimal PorcR = request.Data.Lostload / request.Data.Capacity * 100;

                request.Data.PorcR = Math.Round(PorcR, 4);

                decimal PorcX = Convert.ToDecimal(Math.Sqrt(Math.Pow(Convert.ToDouble(request.Data.PorcZ), 2) - Math.Pow(Convert.ToDouble(request.Data.PorcR), 2)));

                request.Data.PorcX = Math.Round(PorcX, 4);

                decimal XEntreR = PorcX / PorcR;
                request.Data.XIntoR = Math.Round(XEntreR, 4);



                decimal Y1 = Convert.ToDecimal(Math.Sqrt(1 - Math.Pow(Convert.ToDouble(request.Data.FactPot1), 2)));

                decimal Y2 = Convert.ToDecimal(Math.Sqrt(1 - Math.Pow(Convert.ToDouble(request.Data.FactPot2), 2)));

                decimal Y3 = Convert.ToDecimal(Math.Sqrt(1 - Math.Pow(Convert.ToDouble(request.Data.FactPot3), 2)));

                decimal Y4 = Convert.ToDecimal(Math.Sqrt(1 - Math.Pow(Convert.ToDouble(request.Data.FactPot4), 2)));

                decimal Y5 = Convert.ToDecimal(Math.Sqrt(1 - Math.Pow(Convert.ToDouble(request.Data.FactPot5), 2)));

                decimal Y6 = Convert.ToDecimal(Math.Sqrt(1 - Math.Pow(Convert.ToDouble(request.Data.FactPot6), 2)));

                decimal Y7 = Convert.ToDecimal(Math.Sqrt(1 - Math.Pow(Convert.ToDouble(request.Data.FactPot7), 2)));



                decimal PorcReg1 = Convert.ToDecimal((Math.Sqrt(Math.Pow((Convert.ToDouble(PorcR / 100 + request.Data.FactPot1)), 2) + Math.Pow(Convert.ToDouble((PorcX / 100 + Y1)), 2)) - 1) * 100);
                request.Data.PorcReg1 = Math.Round(PorcReg1,4);
                decimal PorcReg2 = Convert.ToDecimal((Math.Sqrt(Math.Pow((Convert.ToDouble(PorcR / 100 + request.Data.FactPot2)), 2) + Math.Pow(Convert.ToDouble((PorcX / 100 + Y2)), 2)) - 1) * 100);
                request.Data.PorcReg2 = Math.Round(PorcReg2, 4);

                decimal PorcReg3 = Convert.ToDecimal((Math.Sqrt(Math.Pow((Convert.ToDouble(PorcR / 100 + request.Data.FactPot3)), 2) + Math.Pow(Convert.ToDouble((PorcX / 100 + Y3)), 2)) - 1) * 100);
                request.Data.PorcReg3 = Math.Round(PorcReg3, 4);

                decimal PorcReg4 = Convert.ToDecimal((Math.Sqrt(Math.Pow((Convert.ToDouble(PorcR / 100 + request.Data.FactPot4)), 2) + Math.Pow(Convert.ToDouble((PorcX / 100 + Y4)), 2)) - 1) * 100);
                request.Data.PorcReg4 = Math.Round(PorcReg4, 4);


              decimal PorcReg5 = Convert.ToDecimal((Math.Sqrt(Math.Pow((Convert.ToDouble(PorcR / 100 + request.Data.FactPot5)), 2) + Math.Pow(Convert.ToDouble((PorcX / 100 + Y5)), 2)) - 1) * 100);
                request.Data.PorcReg5 = Math.Round(PorcReg5, 4);

                decimal PorcReg6 = Convert.ToDecimal((Math.Sqrt(Math.Pow((Convert.ToDouble(PorcR / 100 + request.Data.FactPot6)), 2) + Math.Pow(Convert.ToDouble((PorcX / 100 + Y6)), 2)) - 1) * 100);
                request.Data.PorcReg6 = Math.Round(PorcReg6, 4);

                decimal PorcReg7 = Convert.ToDecimal((Math.Sqrt(Math.Pow((Convert.ToDouble(PorcR / 100 + request.Data.FactPot7)), 2) + Math.Pow(Convert.ToDouble((PorcX / 100 + Y7)), 2)) - 1) * 100);
                request.Data.PorcReg7 = Math.Round(PorcReg7, 4);


                decimal W = 100 * (request.Data.TotalLosses - request.Data.EmptyLosses) / request.Data.Capacity;
                decimal G = 100 * request.Data.EmptyLosses / request.Data.Capacity;
                request.Data.ValueW = Math.Round(W, 6);
                request.Data.ValueG = Math.Round(G, 6);

                for (int r = 0; r < request.Data.RYETestsDetails.Count; r++)
                    {
               

                 
                        request.Data.RYETestsDetails[r].Efficiency1 =Math.Round( (1 - (Convert.ToDecimal((Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G) / ((Convert.ToDecimal(Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G + request.Data.RYETestsDetails[r].PercentageMVA * request.Data.FactPot1)) * 100,4);
                    
                        request.Data.RYETestsDetails[r].Efficiency2 = Math.Round((1 - (Convert.ToDecimal((Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G) / ((Convert.ToDecimal(Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G + request.Data.RYETestsDetails[r].PercentageMVA * request.Data.FactPot2)) * 100,4);
                 
                        request.Data.RYETestsDetails[r].Efficiency3 = Math.Round((1 - (Convert.ToDecimal((Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G) / ((Convert.ToDecimal(Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G + request.Data.RYETestsDetails[r].PercentageMVA * request.Data.FactPot3)) * 100,4);
                    
                        request.Data.RYETestsDetails[r].Efficiency4 = Math.Round((1 - (Convert.ToDecimal((Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G) / ((Convert.ToDecimal(Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G + request.Data.RYETestsDetails[r].PercentageMVA * request.Data.FactPot4)) * 100,4);
                   
                        request.Data.RYETestsDetails[r].Efficiency5 = Math.Round((1 - (Convert.ToDecimal((Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G) / ((Convert.ToDecimal(Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G + request.Data.RYETestsDetails[r].PercentageMVA * request.Data.FactPot5)) * 100,4);
                   
                        request.Data.RYETestsDetails[r].Efficiency6 = Math.Round((1 - (Convert.ToDecimal((Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G) / ((Convert.ToDecimal(Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G + request.Data.RYETestsDetails[r].PercentageMVA * request.Data.FactPot6)) * 100,4);
                    
                        request.Data.RYETestsDetails[r].Efficiency7 = Math.Round((1 - (Convert.ToDecimal((Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G) / ((Convert.ToDecimal(Math.Pow(Convert.ToDouble(request.Data.RYETestsDetails[r].PercentageMVA / 100), 2))) * W + G + request.Data.RYETestsDetails[r].PercentageMVA * request.Data.FactPot7)) * 100,4);
                    


                }


                result.RYETests = ResultISZTests;
             
                result.Results = listErrors.ToList();

                return new ApiResponse<ResultRYETests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                 };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultRYETests>()
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
