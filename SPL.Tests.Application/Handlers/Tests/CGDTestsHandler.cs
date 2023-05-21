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
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;

    using SPL.Domain.SPL.Tests.PCI;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Tests.Application.Commands.Tests;

    public class CGDTestsHandler : IRequestHandler<CGDTestsCommand, ApiResponse<ResultCGDTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public CGDTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultCGDTests>> Handle(CGDTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ResultCGDTests result = new();
                List<CGDTests> ObjTests = new();
                List<CGDTestsDetails> ResultCGDTestsDetails = new();
                List<decimal> datosDesv = new();
                List<decimal> datosTen = new();
                List<ErrorColumns> listErrors = new();

                bool statusLimites = false;
                ObjTests = request.Data;

                if (request.Data == null)
                {
                    return new ApiResponse<ResultCGDTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                string keyTest = request.Data.FirstOrDefault().KeyTest;

                for (int s = 0; s < request.Data.Count; s++)
                {
                    int pos = request.Data.IndexOf(request.Data[s]) + 1;
                    for (int details = 0; details < request.Data[s].CGDTestsDetails.Count; details++)
                    {
                        int posj = request.Data[s].CGDTestsDetails.IndexOf(request.Data[s].CGDTestsDetails[details]) + 1;
                        string keyUpper = request.Data[s].CGDTestsDetails[details].Key.ToUpper();

                        if ((keyUpper.Equals(Enums.CGDKeys.Hydrogen) ||
                            keyUpper.Equals(Enums.CGDKeys.Oxygen) ||
                            keyUpper.Equals(Enums.CGDKeys.Nitrogen) ||
                            keyUpper.Equals(Enums.CGDKeys.Methane) ||
                            keyUpper.Equals(Enums.CGDKeys.CarbonMonoxide) ||
                            keyUpper.Equals(Enums.CGDKeys.CarbonDioxide) ||
                            keyUpper.Equals(Enums.CGDKeys.Ethylene) ||
                            keyUpper.Equals(Enums.CGDKeys.Ethane) ||
                            keyUpper.Equals(Enums.CGDKeys.Acetylene)) && !keyTest.Equals("ADP"))
                        {
                            request.Data[s].CGDTestsDetails[details].Value3 = Math.Round(request.Data[s].CGDTestsDetails[details].Value2 - request.Data[s].CGDTestsDetails[details].Value1, 1);
                        }
                        else if (keyUpper.Equals(Enums.CGDKeys.TotalGases)) {

                            request.Data[s].CGDTestsDetails[details].Value1 = Math.Round(request.Data[s].CGDTestsDetails.Where(item => item.Key.Equals(Enums.CGDKeys.Hydrogen) ||
                            item.Key.Equals(Enums.CGDKeys.Oxygen) ||
                            item.Key.Equals(Enums.CGDKeys.Nitrogen) ||
                            item.Key.Equals(Enums.CGDKeys.Methane) ||
                            item.Key.Equals(Enums.CGDKeys.CarbonMonoxide) ||
                            item.Key.Equals(Enums.CGDKeys.CarbonDioxide) ||
                            item.Key.Equals(Enums.CGDKeys.Ethylene) ||
                            item.Key.Equals(Enums.CGDKeys.Ethane) ||
                            item.Key.Equals(Enums.CGDKeys.Acetylene)).Sum(item => item.Value1), 0);

                            if (!keyTest.Equals("ADP"))
                            {
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(request.Data[s].CGDTestsDetails.Where(item =>
                                item.Key.Equals(Enums.CGDKeys.Hydrogen) ||
                                item.Key.Equals(Enums.CGDKeys.Oxygen) ||
                                item.Key.Equals(Enums.CGDKeys.Nitrogen) ||
                                item.Key.Equals(Enums.CGDKeys.Methane) ||
                                item.Key.Equals(Enums.CGDKeys.CarbonMonoxide) ||
                                item.Key.Equals(Enums.CGDKeys.CarbonDioxide) ||
                                item.Key.Equals(Enums.CGDKeys.Ethylene) ||
                                item.Key.Equals(Enums.CGDKeys.Ethane) ||
                                item.Key.Equals(Enums.CGDKeys.Acetylene)).Sum(item => item.Value2), 0);
                            }
                        }
                        else if (keyUpper.Equals(Enums.CGDKeys.CombustibleGases))
                        {
                            request.Data[s].CGDTestsDetails[details].Value1 = Math.Round(request.Data[s].CGDTestsDetails.Where(item =>
                                item.Key.Equals(Enums.CGDKeys.Hydrogen) ||
                                item.Key.Equals(Enums.CGDKeys.Methane) ||
                                item.Key.Equals(Enums.CGDKeys.CarbonMonoxide) ||
                                item.Key.Equals(Enums.CGDKeys.Ethylene) ||
                                item.Key.Equals(Enums.CGDKeys.Ethane) ||
                                item.Key.Equals(Enums.CGDKeys.Acetylene)).Sum(item => item.Value1), 2);

                            if (!keyTest.Equals("ADP"))
                            {
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(request.Data[s].CGDTestsDetails.Where(item =>
                                item.Key.Equals(Enums.CGDKeys.Hydrogen) ||
                                item.Key.Equals(Enums.CGDKeys.Methane) ||
                                item.Key.Equals(Enums.CGDKeys.CarbonMonoxide) ||
                                item.Key.Equals(Enums.CGDKeys.Ethylene) ||
                                item.Key.Equals(Enums.CGDKeys.Ethane) ||
                                item.Key.Equals(Enums.CGDKeys.Acetylene)).Sum(item => item.Value2), 2);
                            }
                        }
                        else if (keyUpper.Equals(Enums.CGDKeys.PorcCombustibleGas))
                        {

                            decimal CombustibleGases1 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CombustibleGases)).Value1;
                            decimal TotalGases1 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.TotalGases.ToUpper())).Value1;
                            request.Data[s].CGDTestsDetails[details].Value1 = Math.Round(CombustibleGases1 / TotalGases1 * 100, 2);

                            if (!keyTest.Equals("ADP"))
                            {
                                decimal CombustibleGases2 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CombustibleGases.ToUpper())).Value2;
                                decimal TotalGases2 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.TotalGases.ToUpper())).Value2;
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(CombustibleGases2 / TotalGases2 * 100, 2);
                            }
                        }
                        else if (keyUpper.Equals(Enums.CGDKeys.PorcGasContent))
                        {
                            decimal TotalGases1 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.TotalGases.ToUpper())).Value1;
                            request.Data[s].CGDTestsDetails[details].Value1 = Math.Round(TotalGases1 / 10000, 2);
                            if (!keyTest.Equals("ADP"))
                            {
                                decimal TotalGases2 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.TotalGases.ToUpper())).Value2;
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(TotalGases2 / 10000, 2);
                            }
                        } //**************LIMITS FOR GAS INCREMENT DURING TEMPERATURE RISE TEST
                        else if (keyUpper.Equals(Enums.CGDKeys.AcetyleneTest))
                        {
                            statusLimites = true;
                            request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Acetylene)).Value2 - request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Acetylene)).Value1, 2);
                            request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                        }
                        else if (keyUpper.Equals(Enums.CGDKeys.HydrogenTest))
                        {
                            if (keyTest.Equals("DDT"))
                            {
                                statusLimites = true;
                                decimal Division = request.Data[s].Hour;
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round((request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Hydrogen)).Value2 - request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Hydrogen)).Value1) / Division, 2);
                                request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                            }
                            else
                            {
                                statusLimites = true;
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Hydrogen)).Value2 - request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Hydrogen)).Value1, 1);
                                request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                            }
                        }

                        else if (keyUpper.Equals(Enums.CGDKeys.MethaneEthyleneEthaneTest))
                        {
                            if (keyTest.Equals("DDT"))
                            {
                                statusLimites = true;
                                decimal Division = request.Data[s].Hour;

                                decimal Rest1 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Methane)).Value2 + request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Ethylene)).Value2 + request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Ethane)).Value2;

                                decimal Rest2 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Methane)).Value1 + request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Ethylene)).Value1 + request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Ethane)).Value1;

                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round((Rest1 - Rest2) / Division, 2);

                                request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                            }
                            else
                            {
                                statusLimites = true;
                                decimal Rest1 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Methane)).Value2 + request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Ethylene)).Value2 + request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Ethane)).Value2;

                                decimal Rest2 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Methane)).Value1 + request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Ethylene)).Value1 + request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.Ethane)).Value1;

                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(Rest1 - Rest2, 2);

                                request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                            }
                        }
                        else if (keyUpper.Equals(Enums.CGDKeys.CarbonMonoxideTest))
                        {
                            if (keyTest.Equals("DDT"))
                            {
                                statusLimites = true;
                                decimal Division = request.Data[s].Hour;
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round((request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CarbonMonoxide)).Value2 - request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CarbonMonoxide)).Value1) / Division, 2);
                                request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                            }
                            else
                            {
                                statusLimites = true;
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CarbonMonoxide)).Value2 - request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CarbonMonoxide)).Value1, 2);
                                request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                            }
                        }
                        else if (keyUpper.Equals(Enums.CGDKeys.CarbonDioxideTest))
                        {
                            if (keyTest.Equals("DDT"))
                            {
                                statusLimites = true;
                                decimal Division = request.Data[s].Hour;
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round((request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CarbonDioxide)).Value2 - request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CarbonDioxide)).Value1) / Division, 2);
                                request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                            }
                            else
                            {
                                statusLimites = true;
                                request.Data[s].CGDTestsDetails[details].Value2 = Math.Round(request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CarbonDioxide)).Value2 - request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.CarbonDioxide)).Value1, 2);
                                request.Data[s].CGDTestsDetails[details].Value4 = request.Data[s].CGDTestsDetails[details].Value2 <= request.Data[s].CGDTestsDetails[details].Value3 ? 1 : 0;
                            }
                        }
                    }

                    if (keyTest.Equals("ADP"))
                    {
                        bool result1 = request.Data[s].CGDTestsDetails.Where(item => item.Key.Equals(Enums.CGDKeys.Hydrogen) ||
                            item.Key.Equals(Enums.CGDKeys.Oxygen) ||
                            item.Key.Equals(Enums.CGDKeys.Nitrogen) ||
                            item.Key.Equals(Enums.CGDKeys.Methane) ||
                            item.Key.Equals(Enums.CGDKeys.CarbonMonoxide) ||
                            item.Key.Equals(Enums.CGDKeys.CarbonDioxide) ||
                            item.Key.Equals(Enums.CGDKeys.Ethylene) ||
                            item.Key.Equals(Enums.CGDKeys.Ethane) ||
                            item.Key.Equals(Enums.CGDKeys.Acetylene)).Select(x => x.Value1 <= x.Value2 ? 0 : 1).Sum() == 0;

                        if (!result1)
                        {
                            listErrors.Add(new ErrorColumns(pos, 0, "Un Resultado PPM sobrepasa el valor de aceptación"));
                        }

                        bool result2 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.PorcGasContent)).Value1 <= request.Data[s].ValAcceptCg;
                        if (!result2)
                        {
                            listErrors.Add(new ErrorColumns(pos, 0, "El resultado de la validación % Contenido de Gas es mayor a % Contenido de Gas Aceptable : " + request.Data[s].ValAcceptCg + " en la sección nro. " + pos));
                        }
                    }
                    else if (keyTest.Equals("DSC")) 
                    {
                        decimal PorcContenidoGasAcep = Convert.ToDecimal(request.Data[s].ValAcceptCg);
                        if (request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.PorcGasContent)).Value1 > PorcContenidoGasAcep)
                        {
                            listErrors.Add(new ErrorColumns(pos, 0, "El resultado de la validación % Contenido de Gas es mayor a % Contenido de Gas Aceptable : " + PorcContenidoGasAcep + " en la sección nro. " + pos));
                        }
                    }
                    else if(keyTest.Equals("DDE"))
                    {
                        decimal PorcContenidoGasAcep = Convert.ToDecimal(request.Data[s].ValAcceptCg);
                        if(request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.PorcGasContent)).Value1 > PorcContenidoGasAcep ||
                            request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.PorcGasContent)).Value2 > PorcContenidoGasAcep)
                        {
                            listErrors.Add(new ErrorColumns(pos, 0, "El resultado de la validación % Contenido de Gas es mayor a % Contenido de Gas Aceptable : " + PorcContenidoGasAcep + " en la sección nro. " + pos));
                        }
                    }else if (keyTest.Equals("DDD"))
                    {
                        decimal PorcContenidoGasAcep = Convert.ToDecimal(request.Data[s].ValAcceptCg);
                        decimal Reuslt1 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.PorcGasContent)).Value1;
                        if (Reuslt1 > PorcContenidoGasAcep)
                        {
                            listErrors.Add(new ErrorColumns(pos, 0, "El resultado de la validación % Contenido de Gas: " + Reuslt1 + " es mayor a % Contenido de Gas Aceptable : " + PorcContenidoGasAcep + " en la sección nro. " + pos));
                        }
                        decimal Reuslt2 = request.Data[s].CGDTestsDetails.Where(item => item.Key.Equals(Enums.CGDKeys.AcetyleneTest) ||
                            item.Key.Equals(Enums.CGDKeys.HydrogenTest) ||
                            item.Key.Equals(Enums.CGDKeys.MethaneEthyleneEthaneTest) ||
                            item.Key.Equals(Enums.CGDKeys.CarbonMonoxideTest) ||
                            item.Key.Equals(Enums.CGDKeys.CarbonDioxideTest)).Sum(item => item.Value4);

                        if (Reuslt2 < 2)
                        {
                            listErrors.Add(new ErrorColumns(pos, 0, "El resultado de la sumatoria de los limites máximos da mayor a cero: " + Reuslt2 + " en la sección nro. " + pos));
                        }
                    }
                    else
                    {
                        decimal PorcContenidoGasAcep = Convert.ToDecimal(request.Data[s].ValAcceptCg);
                        decimal Reuslt1 = request.Data[s].CGDTestsDetails.FirstOrDefault(item => item.Key.Equals(Enums.CGDKeys.PorcGasContent)).Value1;
                        if (Reuslt1 > PorcContenidoGasAcep)
                        {
                            listErrors.Add(new ErrorColumns(pos, 0, "El resultado de la validación % Contenido de Gas: " + Reuslt1 + " es mayor a % Contenido de Gas Aceptable : " + PorcContenidoGasAcep + " en la sección nro. " + pos));
                        }
                        decimal Reuslt2 = request.Data[s].CGDTestsDetails.Where(item => item.Key.Equals(Enums.CGDKeys.AcetyleneTest) ||
                            item.Key.Equals(Enums.CGDKeys.HydrogenTest) ||
                            item.Key.Equals(Enums.CGDKeys.MethaneEthyleneEthaneTest) ||
                            item.Key.Equals(Enums.CGDKeys.CarbonMonoxideTest) ||
                            item.Key.Equals(Enums.CGDKeys.CarbonDioxideTest)).Sum(item => item.Value4);

                        if (Reuslt2 < 5)
                        {
                            listErrors.Add(new ErrorColumns(pos, 0, "El resultado de la sumatoria de los limites máximos da mayor a cero: " + Reuslt2 + " en la sección nro. " + pos));
                        }
                    }
                }
                result.CGDTests = request.Data;
                result.Results = listErrors.ToList();
                return new ApiResponse<ResultCGDTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultCGDTests>()
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
