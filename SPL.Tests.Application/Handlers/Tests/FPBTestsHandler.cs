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
    using SPL.Tests.Application.Commands.Tests;

    public class FPBTestsHandler : IRequestHandler<FPBTestsCommand, ApiResponse<ResultFPBTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public FPBTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultFPBTests>> Handle(FPBTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data.Count == 0)
                {
                    return new ApiResponse<ResultFPBTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                double toRadians(double val) => Math.PI / 180 * val;

                double ConvertToDegree(double rad) => 180 / Math.PI * rad;

                List<ErrorColumns> listErrors = new();

                ResultFPBTests result = new();
             

                List<FPBTests> ResultFPBTests = new();
                FPCTests ObjFPCTests = new();

                List<FPBTestsDetails> ResultFPCTestsDetails = new();

                ResultFPBTests = request.Data.ToList();

                for (int i = 0; i < request.Data.Count; i++)
                {
                    
                    //ResultFPBTests[i].TempFP = (request.Data[i].UpperOilTemperature + request.Data[i].LowerOilTemperature) / 2;
                    ResultFPBTests[i].TempTanD = request.Data[i].TanDelta.ToUpper().Equals("CON") ? request.Data[i].Temperature : Convert.ToDecimal(20.0);

                    ResultFPCTestsDetails = new List<FPBTestsDetails>();
                    
                    for (int r = 0; r < request.Data[i].FPBTestsDetails.Count; r++)
                    {

                        if (request.Data[i].FPBTestsDetails[r].CorrectionFactorSpecifications20Grados == null)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El factor de corrección por marca y tipo es requerido",
                                Structure = null
                            };
                        }
                        if (request.Data[i].FPBTestsDetails[r].CorrectionFactorSpecificationsTemperature == null)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El factor de corrección por temperatura es requerido",
                                Structure = null
                            };
                        }
                        if (string.IsNullOrEmpty(request.Data[i].FPBTestsDetails[r].T))
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar el valor de la columna con título de T",
                                Structure = null
                            };
                        }

                        if (request.Data[i].FPBTestsDetails[r].T.Length > 2)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La columna T no puede excederse de 2 caracteres",
                                Structure = null
                            };
                        }

                        if (request.Data[i].FPBTestsDetails[r].Current == 0)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la corriente en mA por boquilla",
                                Structure = null
                            };
                        }

                        int[] validationCurrent = CommonMethods.cantDigitsPoint(Convert.ToDouble(request.Data[i].FPBTestsDetails[r].Current));

                        if (validationCurrent[0] > 6 || validationCurrent[1] > 3)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La corriente en mA debe ser mayor a cero considerando 6 enteros con 3 decimales",
                                Structure = null
                            };
                        }

                        if (request.Data[i].FPBTestsDetails[r].Power == 0)
                        {
                            return new ApiResponse<ResultFPBTests>()  
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la potencia en W por boquilla",
                                Structure = null
                            };
                        }

                        int[] validationPower = CommonMethods.cantDigitsPoint(Convert.ToDouble(request.Data[i].FPBTestsDetails[r].Power));

                        if (validationPower[0] > 3 || validationPower[1] > 3)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La potencia en W debe ser mayor a cero considerando 3 enteros con 3 decimales",
                                Structure = null
                            };
                        }

                        if (request.Data[i].FPBTestsDetails[r].Capacitance == 0)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la capacitancia en pF por boquilla",
                                Structure = null
                            };
                        }

                        int[] validationCapacitance = CommonMethods.cantDigitsPoint(Convert.ToDouble(request.Data[i].FPBTestsDetails[r].Capacitance));

                        if (validationCapacitance[0] > 6 || validationCapacitance[1] > 0)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La capacitancia en pF debe ser mayor a cero considerando 6 enteros sin decimales",
                                Structure = null
                            };
                        }

                        if (request.Data[i].TanDelta.ToUpper().Equals("CON"))
                        {
                            ResultFPBTests[i].FPBTestsDetails[r].PercentageA = request.Data[i].FPBTestsDetails[r].Power * 10 / request.Data[i].FPBTestsDetails[r].Current * request.Data[i].FPBTestsDetails[r].CorrectionFactorSpecificationsTemperature.FactorCorr;
                        }
                        else
                        {
                            ResultFPBTests[i].FPBTestsDetails[r].PercentageA = request.Data[i].FPBTestsDetails[r].Power * 10 / request.Data[i].FPBTestsDetails[r].Current;
                        }

                        ResultFPBTests[i].FPBTestsDetails[r].PercentageA = Math.Round(ResultFPBTests[i].FPBTestsDetails[r].PercentageA, 3);

                        //ResultFPBTests[i].FPBTestsDetails[r].PercentageA = Math.Round(request.Data[i].TanDelta.ToUpper().Equals("CON") ? ((Math.Round(request.Data[i].FPBTestsDetails[r].Power * 10,3) / request.Data[i].FPBTestsDetails[r].Current) * request.Data[i].FPBTestsDetails[r].CorrectionFactorSpecificationsTemperature.FactorCorr) : (Math.Round(request.Data[i].FPBTestsDetails[r].Power * 10, 3) / request.Data[i].FPBTestsDetails[r].Current),3);

                        if (request.Data[i].TanDelta.ToUpper().Equals("CON")){
                            if(ResultFPBTests[i].FPBTestsDetails[r].PercentageA is >= (-1) and <= 1)
                            {
                                ResultFPBTests[i].FPBTestsDetails[r].PercentageB = Convert.ToDecimal(Math.Tan(toRadians(90 - ConvertToDegree(Math.Acos(Convert.ToDouble(ResultFPBTests[i].FPBTestsDetails[r].PercentageA))))));
                            }
                            else
                            {
                                ResultFPBTests[i].FPBTestsDetails[r].PercentageB = decimal.MinusOne;
                            }
                        }
                        else
                        {
                            ResultFPBTests[i].FPBTestsDetails[r].PercentageB = ResultFPBTests[i].FPBTestsDetails[r].PercentageA * request.Data[i].FPBTestsDetails[r].CorrectionFactorSpecificationsTemperature.FactorCorr;
                        }

                        ResultFPBTests[i].FPBTestsDetails[r].PercentageB = Math.Round(ResultFPBTests[i].FPBTestsDetails[r].PercentageB, 3);

                        //ResultFPBTests[i].FPBTestsDetails[r].PercentageB = Math.Round(request.Data[i].TanDelta.ToUpper().Equals("CON") ? (ResultFPBTests[i].FPBTestsDetails[r].PercentageA is >= (-1) and <= 1
                        //    ? Convert.ToDecimal(Math.Tan(toRadians(90 - ConvertToDegree(Math.Acos(Convert.ToDouble(ResultFPBTests[i].FPBTestsDetails[r].PercentageA))))))
                        //    : decimal.MinusOne) : ResultFPBTests[i].FPBTestsDetails[r].PercentageA * request.Data[i].FPBTestsDetails[r].CorrectionFactorSpecifications20Grados.FactorCorr,3);

                        if (ResultFPBTests[i].FPBTestsDetails[r].PercentageB is decimal.MinusOne)
                        {
                            listErrors.Add(new ErrorColumns(i, r, "La columna %FP debe poseer valores validos para la funcion ACOS (entre 1 y -1)."));
                        }

                        if (ResultFPBTests[i].FPBTestsDetails[r].NozzlesByDesign.FactorPotencia == 0)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Por favor revise los valores, el factor de potencia es cero por lo tanto da error de división entre cero",
                                Structure = null
                            };
                        }

                        decimal valueValidation = request.Data[i].TanDelta.ToUpper().Equals("SIN") ? Math.Abs(Math.Round(ResultFPBTests[i].FPBTestsDetails[r].PercentageA / ResultFPBTests[i].FPBTestsDetails[r].NozzlesByDesign.FactorPotencia * 100, 0) - 100) : Math.Abs(Math.Round(ResultFPBTests[i].FPBTestsDetails[r].PercentageB / ResultFPBTests[i].FPBTestsDetails[r].NozzlesByDesign.FactorPotencia * 100, 0) - 100);

                        decimal verificarValorAceptacionFP = valueValidation;

                        if (ResultFPBTests[i].FPBTestsDetails[r].NozzlesByDesign.Capacitancia == 0)
                        {
                            return new ApiResponse<ResultFPBTests>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Por favor revise los valores, la capicitancia de la boquilla es cero por lo tanto da error de división entre cero",
                                Structure = null
                            };
                        }

                        decimal verificarValorAceptacionCap = Math.Abs(Math.Round(ResultFPBTests[i].FPBTestsDetails[r].Capacitance / ResultFPBTests[i].FPBTestsDetails[r].NozzlesByDesign.Capacitancia * 100, 0) - 100);

                        if (verificarValorAceptacionFP > request.Data[i].AcceptanceValueFP)
                        {
                            //listErrors.Add(new ErrorColumns(i, r, "El valor de aceptación para FP ha sido superado "+ verificarValorAceptacionFP));
                        }

                        if (verificarValorAceptacionCap > request.Data[i].AcceptanceValueCap)
                        {
                            listErrors.Add(new ErrorColumns(i, r, "El valor de aceptación para capacitancia ha sido superado "+ verificarValorAceptacionCap));
                        }

                        ResultFPBTests[i].CalValorAceptCAP.Add(verificarValorAceptacionCap);
                        ResultFPBTests[i].CalValorAceptFP.Add(verificarValorAceptacionFP);

                    }
                }


                var clavePrueba = request.Data.Where(x => x.KeyTest == "AYD").FirstOrDefault();

                if(clavePrueba != null)//tiene antes y despues
                {
                    bool[] capacitancia = new bool[] {  };
                    bool[] PF20 = new bool[] {  };

                    var dataAntes = request.Data[0].FPBTestsDetails;
                    var dataDesp = request.Data[1].FPBTestsDetails;

                    decimal tolCap = request.Data.FirstOrDefault().ToleranciaCap;
                    decimal tolFP = request.Data.FirstOrDefault().AcceptanceValueFP;
                    for (int i =0; i < dataAntes.Count; i++)
                    {
                        var excedenteFP20 = Math.Round(((Math.Max(dataAntes[i].PercentageA, dataDesp[i].PercentageA) / Math.Min(dataAntes[i].NozzlesByDesign.FactorPotencia, dataDesp[i].NozzlesByDesign.FactorPotencia)) - 1) * 100, 0);
                        var excedenteCapacitancia = Math.Round(((Math.Max(dataAntes[i].Capacitance, dataDesp[i].Capacitance) / Math.Min(dataAntes[i].Capacitance, dataDesp[i].Capacitance)) - 1) * 100, 0);

                        if (excedenteFP20 > tolFP)
                        {
                            listErrors.Add(new ErrorColumns(77, 77, "El excedente %FP supera al valor definido: " + tolFP));
                            break;
                        }

                        if (excedenteCapacitancia > tolCap)
                        {
                            //listErrors.Add(new ErrorColumns(77, 77, "El excedente capacitancia supera al valor definido: " + tolCap));
                            break;
                        }
                    }
                }

                result.FPBTests = ResultFPBTests;
                result.Results = listErrors.ToList();

                return new ApiResponse<ResultFPBTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                 };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultFPBTests>()
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
