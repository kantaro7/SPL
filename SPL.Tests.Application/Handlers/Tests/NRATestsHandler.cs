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
    using SPL.Domain.SPL.Tests.NRA;
    using SPL.Tests.Application.Commands.Tests;

    public class NRATestsHandler : IRequestHandler<NRATestsCommand, ApiResponse<ResultNRATests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public NRATestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultNRATests>> Handle(NRATestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<ErrorColumns> listErrors = new();

                ResultNRATests result = new();

                NRATestsGeneral ResultNRATests = new();

                ResultNRATests = request.Data;



                decimal AreaS;
                decimal FactorK;
                decimal FactorCorIECIEEE;
                decimal NivelPresionProm = 0;
                decimal PotenciaRuido;


                decimal FactorK2;



                decimal AmbPromGen = 0;
                decimal AmbTramGen = 0;

                if (request.Data.Rule.ToUpper().Equals("002") && (request.Data.KeyTest == "RUI" || request.Data.KeyTest == "OCT"))
                {
                    AreaS = Convert.ToDecimal(1.25) * request.Data.Data.Height * request.Data.Data.Length;
                    request.Data.Data.Surface = AreaS;
                }
                else {
                    if (request.Data.CoolingType.ToUpper().Equals("ONAN"))
                    {
                        AreaS = Convert.ToDecimal(1.25) * request.Data.Data.Height * request.Data.Data.Length;
                        request.Data.Data.Surface = AreaS;
                    }
                    else
                    {
                        AreaS = (2 + request.Data.Data.Height) * request.Data.Data.Length;
                        request.Data.Data.Surface = AreaS;

                    }
                    
                }

                FactorK =Convert.ToDecimal(10 * Math.Log10(Convert.ToDouble(1 + (4 / ((request.Data.Data.SV * request.Data.Data.Alfa) / AreaS)))));
                request.Data.Data.KFactor = FactorK;
            
                FactorCorIECIEEE = FactorK;


                decimal ambProm = 0;
                decimal pbaOamTrans = 0;
                //if (!request.Data.InformationLoad)  //si no selecciono cargar información desde BD
                //{
                if (request.Data.KeyTest.ToUpper().Equals("OCT"))
                {
                    MatrixNRATests MatrixNRAAmbProm;
                    MatrixNRATests MatrixNRAAmbTrans;

                    List<MatrixNRATests> listAntDes = request.Data.Data.NRATestsDetailsOcts.MatrixNRAAntTests.Concat(request.Data.Data.NRATestsDetailsOcts.MatrixNRADesTests).ToList();


                    int countAntDes = listAntDes.Count();

                    MatrixNRAAmbProm = new MatrixNRATests()
                    {


                        Dba = Math.Round(listAntDes.Sum(x => x.Dba) / countAntDes, 0),
                        Decibels315 = Math.Round(listAntDes.Sum(x => x.Decibels315) / countAntDes, 1),
                        Decibels63 = Math.Round(listAntDes.Sum(x => x.Decibels63) / countAntDes, 1),
                        Decibels125 = Math.Round(listAntDes.Sum(x => x.Decibels125) / countAntDes, 1),
                        Decibels250 = Math.Round(listAntDes.Sum(x => x.Decibels250) / countAntDes, 1),

                        Decibels500 = Math.Round(listAntDes.Sum(x => x.Decibels500) / countAntDes, 1),

                        Decibels1000 = Math.Round(listAntDes.Sum(x => x.Decibels1000) / countAntDes, 1),

                        Decibels2000 = Math.Round(listAntDes.Sum(x => x.Decibels2000) / countAntDes, 1),

                        Decibels4000 = Math.Round(listAntDes.Sum(x => x.Decibels4000) / countAntDes, 1),

                        Decibels8000 = Math.Round(listAntDes.Sum(x => x.Decibels8000) / countAntDes, 1),
                        Decibels10000 = Math.Round(listAntDes.Sum(x => x.Decibels10000) / countAntDes, 1)
                    };
                    request.Data.Data.NRATestsDetailsOcts.MatrixNRAAmbProm = MatrixNRAAmbProm;
                    AmbPromGen = request.Data.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Dba;


                    List<MatrixNRATests> listCoolingType = request.Data.Data.NRATestsDetailsOcts.MatrixNRACoolingTypeTests;

                    int countCoolingType = listCoolingType.Count();

                    MatrixNRAAmbTrans = new MatrixNRATests()
                    {


                        Dba = Math.Round(listCoolingType.Sum(x => x.Dba) / countCoolingType, 0),
                        Decibels315 = Math.Round(listCoolingType.Sum(x => x.Decibels315) / countCoolingType, 1),
                        Decibels63 = Math.Round(listCoolingType.Sum(x => x.Decibels63) / countCoolingType, 1),
                        Decibels125 = Math.Round(listCoolingType.Sum(x => x.Decibels125) / countCoolingType, 1),
                        Decibels250 = Math.Round(listCoolingType.Sum(x => x.Decibels250) / countCoolingType, 1),

                        Decibels500 = Math.Round(listCoolingType.Sum(x => x.Decibels500) / countCoolingType, 1),

                        Decibels1000 = Math.Round(listCoolingType.Sum(x => x.Decibels1000) / countCoolingType, 1),

                        Decibels2000 = Math.Round(listCoolingType.Sum(x => x.Decibels2000) / countCoolingType, 1),

                        Decibels4000 = Math.Round(listCoolingType.Sum(x => x.Decibels4000) / countCoolingType, 1),

                        Decibels8000 = Math.Round(listCoolingType.Sum(x => x.Decibels8000) / countCoolingType, 1),
                        Decibels10000 = Math.Round(listCoolingType.Sum(x => x.Decibels10000) / countCoolingType, 1)
                    };
                    request.Data.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans = MatrixNRAAmbTrans;
                    AmbTramGen = request.Data.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Dba;


                    //var resta = AmbTramGen - AmbPromGen;

                    // var test11 = request.Data.Data.reportsDTO.MatrixOneDto.Where(x => x.Height == "1/3" && x.TypeInformation == "ANT").Take(cantidadColumnas).ToList();
                    // var test22 = reportsDTO.MatrixOneDto.Where(x => x.Height == "1/3" && x.TypeInformation == "DES").Take(cantidadColumnas).ToList();

                    var matrixAntDesConvertida = new List<MatrixTwo>();
                    foreach(var renglon in request.Data.Data.NRATestsDetailsOcts.MatrizAntesDespuesPura)
                    {

                        for(int i =0; i < renglon.Decibels.Count; i++)
                        {
                            renglon.Decibels[i] = Convert.ToDecimal(Math.Pow(10, Convert.ToDouble(renglon.Decibels[i] / 10m)));
                        }

                        matrixAntDesConvertida.Add(new MatrixTwo
                        {
                           Height = renglon.Height,
                           Decibels = renglon.Decibels,
                           SumRealDecibels = renglon.Decibels.Sum(),
                           Position = renglon.Position,
                           TypeInformation = renglon.TypeInformation
                        });
                    }

                    var matrixEnfriamientoConvertida = request.Data.Data.NRATestsDetailsOcts.MatrizDeEnfriamientoPura;
                    var totales = matrixEnfriamientoConvertida.Select(x => x.SumRealDecibels).Sum();
                    pbaOamTrans = Convert.ToDecimal( 10M *  Convert.ToDecimal(Math.Log10(Decimal.ToDouble(totales / matrixEnfriamientoConvertida.Count()))));

                }
                /*/*   es ruido*/
                else
                {


                    List<MatrixNRATests> listAntDes = request.Data.Data.NRATestsDetailsRuis.MatrixNRAAntTests.Concat(request.Data.Data.NRATestsDetailsRuis.MatrixNRADesTests).ToList();
                    List<MatrixNRATests> list1323 = request.Data.Data.NRATestsDetailsRuis.MatrixNRA13Tests.Concat(request.Data.Data.NRATestsDetailsRuis.MatrixNRA23Tests).ToList();


                    List<double> SUMAAntDesReal = new List<double>();
                    List<double> SUMAMedicionesReal = new List<double>();

                    foreach(var item in listAntDes)
                    {
                        SUMAAntDesReal.Add(Math.Pow(10, Convert.ToDouble(item.Dba / 10)));
                    }

                    foreach (var item in list1323)
                    {
                        SUMAMedicionesReal.Add(Math.Pow(10, Convert.ToDouble(item.Dba / 10)));
                    }

                    ambProm = Convert.ToDecimal( 10 * Math.Log10(SUMAAntDesReal.Sum() / SUMAAntDesReal.Count));
                    pbaOamTrans = Convert.ToDecimal( 10 * Math.Log10(SUMAMedicionesReal.Sum() / SUMAMedicionesReal.Count));

                    request.Data.Data.NRATestsDetailsRuis.AverageAmb = Math.Round(ambProm, 0);
                    request.Data.Data.NRATestsDetailsRuis.AmbTrans = Math.Round(pbaOamTrans, 0);

                }




                decimal diferencia = 0;
                decimal factorCoreccion = 0;

                decimal presionPromedioIECIEE = 0;
                decimal presionPromedioIEEE = 0;
                decimal potenciaRuidoIECIEE = 0;
                decimal potenciaRuidoIEEE = 0;

                if (request.Data.KeyTest.ToUpper().Equals("OCT"))
                {
                    diferencia = Convert.ToDecimal(Math.Round(request.Data.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Dba -
                        request.Data.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Dba, 0));


                    if (diferencia < 0 || diferencia >= 11)
                    {
                        factorCoreccion = 0;
                    }
                    else if (diferencia == 0 || diferencia == 1 || diferencia == 2 || diferencia == 3 || diferencia == 4 || diferencia == 5)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.6);
                    }
                    else if (diferencia == 6)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.3);
                    }
                    else if (diferencia == 7)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.0);
                    }
                    else if (diferencia == 8)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.8);
                    }
                    else if (diferencia == 9)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.6);
                    }
                    else if (diferencia == 10)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.4);
                    }


                    if ((request.Data.Rule.ToUpper().Equals("002")))
                    {
                        if (request.Data.CoolingType.ToUpper().Equals("ONAN"))
                        {
                            if (diferencia > 3)
                            {
                                presionPromedioIEEE = ((pbaOamTrans + factorCoreccion) - FactorCorIECIEEE) - 1;
                              
                            }
                            else
                            {
                                if (diferencia > 0 && diferencia < 3)
                                {
                                    presionPromedioIEEE = ((pbaOamTrans + factorCoreccion) - (0.0278M * Convert.ToDecimal((Math.Pow(Convert.ToDouble(diferencia), 2))) - 0.4986M * diferencia + 3.0102M)) - 1;
                                    
                                }
                                else
                                {
                                    presionPromedioIEEE = pbaOamTrans - 1;
                                    
                                }

                            }
                        }
                        else
                        {

                            if (diferencia > 3)
                            {
                                presionPromedioIEEE = (pbaOamTrans + factorCoreccion) - FactorCorIECIEEE;
                              
                            }
                            else
                            {
                                if (diferencia > 0 && diferencia < 3)
                                {
                                    presionPromedioIEEE = (pbaOamTrans + factorCoreccion) - (0.0278M * Convert.ToDecimal((Math.Pow(Convert.ToDouble(diferencia), 2))) - 0.4986M * diferencia + 3.0102M);
                                   
                                }
                                else
                                {
                                    presionPromedioIEEE = pbaOamTrans;
                                  
                                }

                            }


                        }

                       
                    }
                    else
                    {

                        if (diferencia > 3)
                        {
                            presionPromedioIEEE = pbaOamTrans - FactorCorIECIEEE;
                        }
                        else
                        {
                            if (diferencia > 0 && diferencia < 3)
                            {
                                presionPromedioIEEE = pbaOamTrans - (0.0278M * Convert.ToDecimal((Math.Pow(Convert.ToDouble(diferencia), 2))) - 0.4986M * diferencia + 3.0102M);
                            }
                            else
                            {
                                presionPromedioIEEE = pbaOamTrans;
                            }

                        }

                    }
                    // ya tengo NivelPResionPromedioIEEE




                    if (diferencia > 3)
                    {
                        presionPromedioIECIEE = pbaOamTrans - FactorCorIECIEEE;
                    }
                    else if (diferencia > 0 && diferencia < 3)
                    {
                        presionPromedioIECIEE = (pbaOamTrans) - (0.0278M * Convert.ToDecimal((Math.Pow(Convert.ToDouble(diferencia), 2))) - 0.4986M * diferencia + 3.0102M);
                    }
                    else
                    {
                        presionPromedioIECIEE = pbaOamTrans;
                    }

                    // ya tengo NivelPResionPromedioIECIEEE


                    FactorK2 = Convert.ToDecimal(10 * Math.Log10(Convert.ToDouble(request.Data.Data.Surface)));

                    potenciaRuidoIEEE = presionPromedioIEEE + FactorK2;
                    potenciaRuidoIECIEE = presionPromedioIECIEE + FactorK2;

                    if (request.Data.Rule.ToUpper().Equals("002"))
                    {
                        request.Data.Data.AvgSoundPressureLevel = presionPromedioIEEE;
                        request.Data.Data.SoundPowerLevel = potenciaRuidoIEEE;
                    }
                    else
                    {
                        request.Data.Data.AvgSoundPressureLevel = presionPromedioIECIEE;
                        request.Data.Data.SoundPowerLevel = potenciaRuidoIECIEE;
                    }



                }
                else
                {
                    diferencia = Convert.ToDecimal(Math.Round(pbaOamTrans - ambProm, 0));

                    if (diferencia < 0 || diferencia >= 11)
                    {
                        factorCoreccion = 0;
                    }
                    else if (diferencia == 0 || diferencia == 1 || diferencia == 2 || diferencia == 3 || diferencia == 4 || diferencia == 5)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.6);
                    }
                    else if (diferencia == 6)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.3);
                    }
                    else if (diferencia == 7)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.0);
                    }
                    else if (diferencia == 8)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.8);
                    }
                    else if (diferencia == 9)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.6);
                    }
                    else if (diferencia == 10)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.4);
                    }



                    if ((request.Data.Rule.ToUpper().Equals("002")))
                    {
                        if (request.Data.CoolingType.ToUpper().Equals("ONAN"))
                        {
                            if (diferencia > 3)
                            {
                                NivelPresionProm = ((pbaOamTrans + factorCoreccion) - FactorCorIECIEEE) - 1;
                                request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                            }
                            else
                            {
                                if (diferencia > 0 && diferencia < 3)
                                {
                                    NivelPresionProm = ((pbaOamTrans + factorCoreccion) - (0.0278M * Convert.ToDecimal((Math.Pow(Convert.ToDouble(diferencia), 2))) - 0.4986M * diferencia + 3.0102M)) - 1;
                                    request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                                }
                                else
                                {
                                    NivelPresionProm = pbaOamTrans - 1;
                                    request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                                }

                            }
                        }
                        else
                        {

                            if (diferencia > 3)
                            {
                                NivelPresionProm = (pbaOamTrans + factorCoreccion) - FactorCorIECIEEE;
                                request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                            }
                            else
                            {
                                if (diferencia > 0 && diferencia < 3)
                                {
                                    NivelPresionProm = (pbaOamTrans + factorCoreccion) - (0.0278M * Convert.ToDecimal((Math.Pow(Convert.ToDouble(diferencia), 2))) - 0.4986M * diferencia + 3.0102M);
                                    request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                                }
                                else
                                {
                                    NivelPresionProm = pbaOamTrans;
                                    request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                                }

                            }


                        }
                    }
                    else
                    {

                        if (diferencia > 3)
                        {
                            NivelPresionProm = pbaOamTrans - FactorCorIECIEEE;
                            request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                        }
                        else
                        {
                            if (diferencia > 0 && diferencia < 3)
                            {
                                NivelPresionProm = pbaOamTrans - (0.0278M * Convert.ToDecimal((Math.Pow(Convert.ToDouble(diferencia), 2))) - 0.4986M * diferencia + 3.0102M);
                                request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                            }
                            else
                            {
                                NivelPresionProm = pbaOamTrans;
                                request.Data.Data.AvgSoundPressureLevel = NivelPresionProm;
                            }

                        }

                    }
                    request.Data.Data.AvgSoundPressureLevel = Math.Round(request.Data.Data.AvgSoundPressureLevel, 0);

                    if (request.Data.KeyTest.ToUpper().Equals("RUI"))
                    {
                        foreach (var item in request.Data.Data.NRATestsDetailsRuis.MatrixNRA13Tests)
                        {
                            item.DbaCor = item.Dba + factorCoreccion;

                        }
                        foreach (var item in request.Data.Data.NRATestsDetailsRuis.MatrixNRA23Tests)
                        {
                            item.DbaCor = item.Dba + factorCoreccion;

                        }
                    }



                    FactorK2 = Convert.ToDecimal(10 * Math.Log10(Convert.ToDouble(request.Data.Data.Surface)));

                    PotenciaRuido = NivelPresionProm + FactorK2;
                    request.Data.Data.SoundPowerLevel = Math.Round(PotenciaRuido, 0);


                    if (request.Data.Data.AvgSoundPressureLevel >= request.Data.Data.Guaranteed)
                    {
                        listErrors.Add(new ErrorColumns(1, 1, "El valor de Nivel Presion Promedio  es mayor al de Garantías"));
                    }

                }





                result.NRATestsGeneral = request.Data;
                result.Results = listErrors.ToList();

                return new ApiResponse<ResultNRATests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<ResultNRATests>()
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
