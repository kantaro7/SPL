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
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Tests.Application.Commands.Tests;

    public class FPCTestsHandler : IRequestHandler<FPCTestsCommand, ApiResponse<ResultFPCTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public FPCTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultFPCTests>> Handle(FPCTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                double toRadians(double val) => Math.PI / 180 * val;

                double ConvertToDegree(double rad) => 180 / Math.PI * rad;

                List<ErrorColumns> listErrors = new();

                ResultFPCTests result = new();
             

                List<FPCTests> ResultFPCTests = new();
                FPCTests ObjFPCTests = new();

                List<FPCTestsDetails> ResultFPCTestsDetails = new();

                ResultFPCTests = request.Data.ToList();

                for (int i = 0; i < request.Data.Count; i++)
                {
                    ResultFPCTests[i].TempFP = (request.Data[i].UpperOilTemperature + request.Data[i].LowerOilTemperature) / 2;
                    ResultFPCTests[i].TempTanD = request.Data[i].Specification.Equals("Doble") ? Convert.ToDecimal(20.0) : ResultFPCTests[i].TempFP;

                    ResultFPCTestsDetails = new List<FPCTestsDetails>();
                    
                    for (int r = 0; r < request.Data[i].FPCTestsDetails.Count; r++)
                    {
                        

                        ResultFPCTests[i].FPCTestsDetails[r].PercentageA = Math.Round(request.Data[i].FPCTestsDetails[r].Power * 10 / request.Data[i].FPCTestsDetails[r].Current, 3);
                        decimal calculoTAN = ResultFPCTests[i].FPCTestsDetails[r].PercentageA is >= (-1) and <= 1
                            ? Convert.ToDecimal(Math.Tan(toRadians(90 - ConvertToDegree(Math.Acos(Convert.ToDouble(ResultFPCTests[i].FPCTestsDetails[r].PercentageA))))))
                            : decimal.MinusOne;
                        if (calculoTAN is decimal.MinusOne)
                        {
                            listErrors.Add(new ErrorColumns(i, r, "La columna %FP debe poseer valores validos para la funcion ACOS (entre 1 y -1)."));
                        }

                        decimal calculoFPCOR = ResultFPCTests[i].FPCTestsDetails[r].PercentageA * request.Data[i].CorrectionFactorSpecifications.FactorCorr;

                        ResultFPCTests[i].FPCTestsDetails[r].PercentageB = Math.Round(calculoTAN,3);
                        ResultFPCTests[i].FPCTestsDetails[r].PercentageC = Math.Round(calculoFPCOR, 3);

                        ResultFPCTests[i].FPCTestsDetails[r].Capacitance = request.Data[i].FPCTestsDetails[r].Current / (2 * Convert.ToDecimal(Math.PI) * request.Data[i].Frequency * request.Data[i].Tension) * 1000000;
                        ResultFPCTests[i].FPCTestsDetails[r].Capacitance = Math.Round(ResultFPCTests[i].FPCTestsDetails[r].Capacitance, 0);
                        if (ResultFPCTests[i].FPCTestsDetails[r].PercentageA > request.Data[i].AcceptanceValueFP)
                        {
                            listErrors.Add(new ErrorColumns(i, r, "La columna %FP posee valores que son mayor al valor de aceptacion"));
                        }
                    }

                    List<decimal> list = new();
                    bool aplicaValidCap = false;
                    List < FPCTestsValidationsCap> listCap = new List<FPCTestsValidationsCap>();
                    if (request.Data[i].FPCTestsDetails.Count > 1 )
                    {
                        list.Add(request.Data[i].FPCTestsDetails[0].Capacitance - request.Data[i].FPCTestsDetails[1].Capacitance);
                        listCap.Add(new FPCTestsValidationsCap() { Operation = "1-2", Value = request.Data[i].FPCTestsDetails[0].Capacitance.ToString() +" - " + request.Data[i].FPCTestsDetails[1].Capacitance.ToString(), Result = list[0].ToString()});

                        list.Add(request.Data[i].FPCTestsDetails[2].Capacitance - request.Data[i].FPCTestsDetails[3].Capacitance);
                        listCap.Add(new FPCTestsValidationsCap() { Operation = "3-4", Value = request.Data[i].FPCTestsDetails[2].Capacitance.ToString() + " - " + request.Data[i].FPCTestsDetails[3].Capacitance.ToString(), Result = list[1].ToString() });



                        list.Add(request.Data[i].FPCTestsDetails[4].Capacitance - request.Data[i].FPCTestsDetails[5].Capacitance);
                        listCap.Add(new FPCTestsValidationsCap() { Operation = "5-6", Value = request.Data[i].FPCTestsDetails[4].Capacitance.ToString() + " - " + request.Data[i].FPCTestsDetails[5].Capacitance.ToString(), Result = list[2].ToString() });


                        list.Add(request.Data[i].FPCTestsDetails[1].Capacitance + request.Data[i].FPCTestsDetails[3].Capacitance + request.Data[i].FPCTestsDetails[5].Capacitance);
                        listCap.Add(new FPCTestsValidationsCap() { Operation = "2+4+6", Value = request.Data[i].FPCTestsDetails[1].Capacitance.ToString() + " - " + request.Data[i].FPCTestsDetails[3].Capacitance.ToString() + " - " + request.Data[i].FPCTestsDetails[5].Capacitance.ToString(), Result = list[3].ToString() });





                        if (request.Data[i].UnitType.ToUpper().Equals("2DE") || request.Data[i].UnitType.ToUpper().Equals("ACT"))
                        {
                            aplicaValidCap = true;
                            list.Add((Math.Max(list[0], request.Data[i].FPCTestsDetails[4].Capacitance) / Math.Min(list[0], request.Data[i].FPCTestsDetails[4].Capacitance)) - 1);
                            listCap.Add(new FPCTestsValidationsCap() { Operation = "1-2 vs 5", Value = "Valor del cálculo renglón 1: "+list[0].ToString() + "Valor de la capacitancia 5: " + request.Data[i].FPCTestsDetails[4].Capacitance.ToString(), Result = list[4].ToString() });





                            list.Add((Math.Max(list[1], request.Data[i].FPCTestsDetails[5].Capacitance) / Math.Min(list[1], request.Data[i].FPCTestsDetails[5].Capacitance)) - 1);
                            listCap.Add(new FPCTestsValidationsCap() { Operation = "3-4 vs 6", Value = "Valor del cálculo renglón 2: " + list[1].ToString() + "Valor de la capacitancia 6: " + request.Data[i].FPCTestsDetails[5].Capacitance.ToString(), Result = list[5].ToString() });



                            list.Add((Math.Max(request.Data[i].FPCTestsDetails[4].Capacitance, request.Data[i].FPCTestsDetails[5].Capacitance) / Math.Min(request.Data[i].FPCTestsDetails[4].Capacitance, request.Data[i].FPCTestsDetails[5].Capacitance)) - 1);
                            listCap.Add(new FPCTestsValidationsCap() { Operation = "5 vs 6", Value = "Valor de la capacitancia 5:  "  +request.Data[i].FPCTestsDetails[4].Capacitance.ToString() + "Valor de la capacitancia 6:  " + request.Data[i].FPCTestsDetails[5].Capacitance.ToString(), Result = list[6].ToString() });

                            //if (request.Data[i].FPCTestsDetails.Count > 6)
                            //{
                            //    list.Add((Math.Max(list[0], request.Data[i].FPCTestsDetails[7].Capacitance) / Math.Min(list[0], request.Data[i].FPCTestsDetails[7].Capacitance)) - 1);

                            //    list.Add((Math.Max(list[1], request.Data[i].FPCTestsDetails[8].Capacitance) / Math.Min(list[1], request.Data[i].FPCTestsDetails[8].Capacitance)) - 1);

                            //    list.Add((Math.Max(list[2], request.Data[i].FPCTestsDetails[9].Capacitance) / Math.Min(list[2], request.Data[i].FPCTestsDetails[9].Capacitance)) - 1);

                            //    list.Add((Math.Max(list[3], request.Data[i].FPCTestsDetails[6].Capacitance) / Math.Min(list[3], request.Data[i].FPCTestsDetails[6].Capacitance)) - 1);
                            //}
                        }
                        else if (request.Data[i].UnitType.ToUpper().Equals("3DE")){
                            aplicaValidCap = true;
                            list.Add((Math.Max(list[0], request.Data[i].FPCTestsDetails[7].Capacitance) / Math.Min(list[0], request.Data[i].FPCTestsDetails[7].Capacitance)) - 1);
                            listCap.Add(new FPCTestsValidationsCap() { Operation = "1-2 vs 8", Value = "Valor del cálculo renglón 1: " + list[0].ToString() + "Valor de la capacitancia 8:  " + request.Data[i].FPCTestsDetails[7].Capacitance.ToString(), Result = list[4].ToString() });



                            list.Add((Math.Max(list[1], request.Data[i].FPCTestsDetails[8].Capacitance) / Math.Min(list[1], request.Data[i].FPCTestsDetails[8].Capacitance)) - 1);
                            listCap.Add(new FPCTestsValidationsCap() { Operation = "3-4 vs 9", Value = "Valor del cálculo renglón 2: " +list[1].ToString() + "Valor de la capacitancia 9:  " + request.Data[i].FPCTestsDetails[8].Capacitance.ToString(), Result = list[5].ToString() });


                            list.Add((Math.Max(list[2], request.Data[i].FPCTestsDetails[9].Capacitance) / Math.Min(list[2], request.Data[i].FPCTestsDetails[9].Capacitance)) - 1);
                            listCap.Add(new FPCTestsValidationsCap() { Operation = "5-6 vs 10", Value = "Valor del cálculo renglón 3: " + list[2].ToString()  +"Valor de la capacitancia 10:  " + request.Data[i].FPCTestsDetails[9].Capacitance.ToString(), Result = list[6].ToString() });


                            list.Add((Math.Max(list[3], request.Data[i].FPCTestsDetails[6].Capacitance) / Math.Min(list[3], request.Data[i].FPCTestsDetails[6].Capacitance)) - 1);
                            listCap.Add(new FPCTestsValidationsCap() { Operation = "2+4+6 vs 7", Value = "Valor del cálculo renglón 4: "+list[3].ToString() + "Valor de la capacitancia 7:  " + request.Data[i].FPCTestsDetails[6].Capacitance.ToString(), Result = list[7].ToString() });

                        }


                        if (aplicaValidCap)
                        {
                            for (int m = 4; m < list.Count; m++)
                            {
                                if (list[m] > request.Data[i].AcceptanceValueCap)
                                {
                                    listErrors.Add(new ErrorColumns((i + 1), m, "La columna capacitancia posee valores que son mayor al valor de aceptación"));
                                    break;
                                }
                            }
                        }

                    

                        ResultFPCTests[i].Capacitance = listCap;
                    }
                }

                result.FPCTests = ResultFPCTests;
             
                result.Results = listErrors.ToList();

                return new ApiResponse<ResultFPCTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                 };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultFPCTests>()
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
