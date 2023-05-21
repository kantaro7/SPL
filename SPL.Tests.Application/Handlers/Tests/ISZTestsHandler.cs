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

    public class ISZTestsHandler : IRequestHandler<ISZTestsCommand, ApiResponse<ResultISZTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public ISZTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultISZTests>> Handle(ISZTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data ==null)
                {
                    return new ApiResponse<ResultISZTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }


                List<ErrorColumns> listErrors = new();

                ResultISZTests result = new();
             

                OutISZTests ResultISZTests = new();

                FPCTests ObjFPCTests = new();

            

                ResultISZTests = request.Data;


                int secciones = 0;
                foreach (var item in request.Data.SeccionesISZTestsDetails)//recorrido de las secciones
                {


                    for (int r = 0; r < item.ISZTestsDetails.Count; r++)
                    {

                        Decimal factoCorrección = 0;
                        if (request.Data.MaterialWinding.ToUpper().Equals("COBRE"))
                        {
                            factoCorrección = (Convert.ToDecimal(234.5) + request.Data.DegreesCor) / (Convert.ToDecimal(234.5) + request.Data.Temperature);
                        }
                        else if (request.Data.MaterialWinding.ToUpper().Equals("ALUMINIO"))
                        {
                            factoCorrección = (225 + request.Data.DegreesCor) / (225 + request.Data.Temperature);
                        }

                        Decimal factorCorrRedondeado = Math.Round(factoCorrección, 14);
                        Decimal PotenciaCorr = item.ISZTestsDetails[r].PowerKW * factorCorrRedondeado;
                        Decimal PotenciaCorrRedondeada = Math.Round(PotenciaCorr, 15);

                        item.ISZTestsDetails[r].PowerKWVoltage1 = Math.Round(PotenciaCorr, 3);
                        item.ISZTestsDetails[r].FactorCorr = Math.Round(factoCorrección,6);



                        decimal ZBase = 0;

                        if (request.Data.KeyTest.ToUpper().Equals("AYB"))
                        {
                            if (request.Data.WindingEnergized.ToUpper().Equals("AT"))
                            {
                                ZBase = item.ISZTestsDetails[r].Voltage1 * item.ISZTestsDetails[r].Voltage1 / request.Data.CapBaseMin;
                            }

                            if (request.Data.WindingEnergized.ToUpper().Equals("BT"))
                            {
                                ZBase = item.ISZTestsDetails[r].Voltage2 * item.ISZTestsDetails[r].Voltage2 / request.Data.CapBaseMin;
                            }
                        }

                        else
                        if (request.Data.KeyTest.ToUpper().Equals("AYT"))
                        {
                            if (request.Data.WindingEnergized.ToUpper().Equals("AT"))
                            {
                                ZBase = item.ISZTestsDetails[r].Voltage1 * item.ISZTestsDetails[r].Voltage1 / request.Data.CapBaseMin;
                            }

                            if (request.Data.WindingEnergized.ToUpper().Equals("TER"))
                            {
                                ZBase = item.ISZTestsDetails[r].Voltage2 * item.ISZTestsDetails[r].Voltage2 / request.Data.CapBaseMin;
                            }
                        }
                        else if(request.Data.KeyTest.ToUpper().Equals("BYT"))
                        {
                            if (request.Data.WindingEnergized.ToUpper().Equals("BT"))
                            {
                                ZBase = item.ISZTestsDetails[r].Voltage1 * item.ISZTestsDetails[r].Voltage1 / request.Data.CapBaseMin;
                            }

                            if (request.Data.WindingEnergized.ToUpper().Equals("TER"))
                            {
                                ZBase = item.ISZTestsDetails[r].Voltage2 * item.ISZTestsDetails[r].Voltage2 / request.Data.CapBaseMin;
                            }

                        }
                        else
                        {
                            var devanadosSplit = request.Data.WindingEnergized.Split(",");

                            if(secciones + 1 == 1)//secciones
                            {
                                if (devanadosSplit[secciones] == "AT")
                                {
                                    ZBase = item.ISZTestsDetails[r].Voltage1 * item.ISZTestsDetails[r].Voltage1 / request.Data.CapBaseMin;
                                }
                                else if (devanadosSplit[secciones] == "BT")
                                {
                                    ZBase = item.ISZTestsDetails[r].Voltage2 * item.ISZTestsDetails[r].Voltage2 / request.Data.CapBaseMin;
                                }
                            }
                            else if(secciones + 1 == 2)
                            {
                                if (devanadosSplit[secciones] == "AT")
                                {
                                    ZBase = item.ISZTestsDetails[r].Voltage1 * item.ISZTestsDetails[r].Voltage1 / request.Data.CapBaseMin;
                                }
                                else if (devanadosSplit[secciones].ToUpper() == "TER")
                                {
                                    ZBase = item.ISZTestsDetails[r].Voltage2 * item.ISZTestsDetails[r].Voltage2 / request.Data.CapBaseMin;
                                }
                            }
                            else if(secciones + 1 == 3)
                            {
                                if (devanadosSplit[secciones] == "BT")
                                {
                                    ZBase = item.ISZTestsDetails[r].Voltage1 * item.ISZTestsDetails[r].Voltage1 / request.Data.CapBaseMin;
                                }
                                else if (devanadosSplit[secciones].ToUpper() == "TER")
                                {
                                    ZBase = item.ISZTestsDetails[r].Voltage2 * item.ISZTestsDetails[r].Voltage2 / request.Data.CapBaseMin;
                                }
                            }

                        }

                        ZBase = Math.Round(ZBase, 7);
                        item.ISZTestsDetails[r].ZBase = ZBase;

                        Decimal ZOhms = item.ISZTestsDetails[r].VoltsVRMS / item.ISZTestsDetails[r].CurrentsIRMS;

                        ZOhms = Math.Round(ZOhms, 14);
                        item.ISZTestsDetails[r].ZOhms = Math.Round(ZOhms, 3); ;

                        Decimal Porc_Ro = (Math.Round(PotenciaCorrRedondeada, 3) / ((item.ISZTestsDetails[r].CurrentsIRMS * item.ISZTestsDetails[r].CurrentsIRMS) * 3M)) * 100000M / Math.Round(ZBase, 3);

                        //decimal Porc_Ro2 = ((item.ISZTestsDetails[r].PowerKWVoltage1 / ((item.ISZTestsDetails[r].CurrentsIRMS * item.ISZTestsDetails[r].CurrentsIRMS) )) * 3 * Convert.ToDecimal(100000) / ZBase);
                        item.ISZTestsDetails[r].PercentageRo = Math.Round(Porc_Ro, 3);


                        Decimal Porc_Zo = (ZOhms / ZBase) * 300M;
                        item.ISZTestsDetails[r].PercentageZo = Math.Round(Porc_Zo, 3);

                        Decimal Porc_jXo = Convert.ToDecimal(Math.Sqrt(Decimal.ToDouble(Porc_Zo * Porc_Zo - Porc_Ro * Porc_Ro)));
                        item.ISZTestsDetails[r].PercentagejXo = Math.Round(Porc_jXo, 3);

                        Complex ValorComplejo = new Complex(Decimal.ToDouble(Porc_Ro), Decimal.ToDouble(Porc_jXo));





                        item.ISZTestsDetails[r].PercentageComplex = ValorComplejo;

                    }
                    secciones++;
                }
                 



                string TypeUnit = request.Data.GeneralArtifact.GeneralArtifact.TipoUnidad;
                string Mvaf1_connection_id = "";
                string Mvaf2_connection_id = "";
                string Mvaf4_connection_id = "";

                string ArregloDev = "";

                if (request.Data.GeneralArtifact.Derivations.conexionequivalente.ToUpper().Equals("WYE"))
                
                    Mvaf1_connection_id = "Y";
                else
                if (request.Data.GeneralArtifact.Derivations.conexionequivalente.ToUpper().Equals("DELTA"))
                
                    Mvaf1_connection_id = "D";
                 else
                if (request.Data.GeneralArtifact.Derivations.conexionequivalente.ToUpper().Equals("AUTO"))
                  Mvaf1_connection_id = "AUTO";



                if (request.Data.GeneralArtifact.Derivations.conexionequivalente_2.ToUpper().Equals("WYE"))

                    Mvaf2_connection_id = "Y";
                else
              if (request.Data.GeneralArtifact.Derivations.conexionequivalente_2.ToUpper().Equals("DELTA"))

                    Mvaf2_connection_id = "D";
                else
              if (request.Data.GeneralArtifact.Derivations.conexionequivalente_2.ToUpper().Equals("AUTO"))
                    Mvaf2_connection_id = "AUTO";



                if (request.Data.GeneralArtifact.Derivations.conexionequivalente_4.ToUpper().Equals("WYE"))

                    Mvaf4_connection_id = "Y";
                else
              if (request.Data.GeneralArtifact.Derivations.conexionequivalente_4.ToUpper().Equals("DELTA"))

                    Mvaf4_connection_id = "D";
                else
              if (request.Data.GeneralArtifact.Derivations.conexionequivalente_4.ToUpper().Equals("AUTO"))
                    Mvaf4_connection_id = "AUTO";

                string guion2 = "-";
                string guion3 = "-";

                if (TypeUnit.ToUpper().Equals("2DE"))
                {
                    if (string.IsNullOrEmpty(Mvaf2_connection_id))
                    {
                        guion2 = "";
                    }
                    ArregloDev = Mvaf1_connection_id + guion2 + Mvaf2_connection_id;
                }
                else
                if (TypeUnit.ToUpper().Equals("3DE"))
                {

                    if (string.IsNullOrEmpty(Mvaf2_connection_id))
                    {
                        guion2 = "";
                    }
                    if (string.IsNullOrEmpty(Mvaf4_connection_id))
                    {
                        guion3 = "";
                    }

                    ArregloDev = Mvaf1_connection_id + guion2 + Mvaf2_connection_id + guion3 + Mvaf4_connection_id;
                }
                else
                if (TypeUnit.ToUpper().Equals("ACT"))
                {

                    if (string.IsNullOrEmpty(Mvaf2_connection_id))
                    {
                        guion2 = "";
                    }

                    ArregloDev = Mvaf1_connection_id + guion2 + Mvaf2_connection_id;

                    if (Mvaf1_connection_id.ToUpper().Equals("AUTO") && Mvaf2_connection_id.ToUpper().Equals("AUTO"))
                    {
                        ArregloDev = Mvaf1_connection_id;
                    }
                }

                string MedicionDev = "";

                if (request.Data.KeyTest.ToUpper().Equals("AYB"))
                {
                    MedicionDev = "H-X";
                }
                else
                if (request.Data.KeyTest.ToUpper().Equals("AYT"))
                {
                    MedicionDev = "H-Y";
                }
                else
                if (request.Data.KeyTest.ToUpper().Equals("BYT"))
                {
                    MedicionDev = "X-Y";
                }

                decimal ValorMinAcep = 0;
                decimal ValorMaxAcep = 0;

                decimal ValorMinAcepSec1 = 0;
                decimal ValorMinAcepSec2 = 0;
                decimal ValorMinAcepSec3 = 0;
                decimal ValorMaxAcepSec1 = 0;
                decimal ValorMaxAcepSec2 = 0;
                decimal ValorMaxAcepSec3 = 0;


                if (request.Data.KeyTest != "ABT")
                {

                    if (decimal.TryParse(request.Data.ImpedanceGar, out decimal impedanciaGar))
                    {
                        ValorMinAcep = impedanciaGar * (request.Data.PorcMinAcepImp / 100);
                        ValorMaxAcep = impedanciaGar * (request.Data.PorcMaxAcepImp / 100);
                    }
                }
                else
                {
                    var impedanciasSelecionadas = request.Data.ImpedanceGar.Split(",");

                    for (int i = 0; i < impedanciasSelecionadas.Length; i++)
                    {
                        switch (i+1)
                        {
                            case 1:
                                if (decimal.TryParse(impedanciasSelecionadas[i], out decimal impedanciaGar))
                                {
                                    ValorMinAcepSec1 = impedanciaGar * (request.Data.PorcMinAcepImp / 100);
                                    ValorMaxAcepSec1 = impedanciaGar * (request.Data.PorcMaxAcepImp / 100);
                                }
                                break;
                            case 2:
                                if (decimal.TryParse(impedanciasSelecionadas[i], out  impedanciaGar))
                                {
                                    ValorMinAcepSec2 = impedanciaGar * (request.Data.PorcMinAcepImp / 100);
                                    ValorMaxAcepSec2 = impedanciaGar * (request.Data.PorcMaxAcepImp / 100);
                                }
                                break;
                            case 3:
                                if (decimal.TryParse(impedanciasSelecionadas[i], out impedanciaGar) )
                                {
                                    ValorMinAcepSec3 = impedanciaGar * (request.Data.PorcMinAcepImp / 100);
                                    ValorMaxAcepSec3 = impedanciaGar * (request.Data.PorcMaxAcepImp / 100);
                                }
                                break;
                            default:
                                // code block
                                break;
                        }
                       
                    }

                }

                  ISZTestsDetails data = null;
                  ISZTestsDetails data1 = null;
                  ISZTestsDetails data2= null;
                  ISZTestsDetails data3 = null;

                if (request.Data.KeyTest.ToUpper().Equals("AYB"))
                {
                    if (request.Data.PosAT.ToUpper().Equals("TODAS"))
                        data = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.ValueNomPosAll));
                    else if (request.Data.PosBT.ToUpper().Equals("TODAS"))
                        data = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position2.Equals(request.Data.ValueNomPosAll));

                }
                else if (request.Data.KeyTest.ToUpper().Equals("AYT"))
                {
                    if (request.Data.PosAT.ToUpper().Equals("TODAS"))
                        data = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.ValueNomPosAll));
                    else if (request.Data.PosTER.ToUpper().Equals("TODAS"))
                        data = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position2.Equals(request.Data.ValueNomPosAll));

                }
                else if (request.Data.KeyTest.ToUpper().Equals("BYT"))
                {
                    if (request.Data.PosBT.ToUpper().Equals("TODAS"))
                        data = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.ValueNomPosAll));
                    else if (request.Data.PosTER.ToUpper().Equals("TODAS"))
                        data = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position2.Equals(request.Data.ValueNomPosAll));

                }
                else
                {
                    if (request.Data.PosicionMayotABT == "AT")//todos
                    {
                        data1 = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.NominalAT));
                        data2 = request.Data.SeccionesISZTestsDetails[1].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.NominalAT));
                    }
                    else if (request.Data.PosicionMayotABT == "BT")//todos
                    {
                        data1 = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position2.Equals(request.Data.NominalBT));
                        data3 = request.Data.SeccionesISZTestsDetails[2].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.NominalBT));
                    }
                    else if(request.Data.PosicionMayotABT.ToUpper() == "TER")//todos
                    {
                        data2 = request.Data.SeccionesISZTestsDetails[1].ISZTestsDetails.FirstOrDefault(x => x.Position2.Equals(request.Data.NominalTer));
                        data3 = request.Data.SeccionesISZTestsDetails[2].ISZTestsDetails.FirstOrDefault(x => x.Position2.Equals(request.Data.NominalTer));
                    }
                }

               

                if (request.Data.KeyTest != "ABT")
                {
                    if (data != null)
                    {
                        decimal ValorAbsoluto = Math.Round(Convert.ToDecimal(Complex.Abs(data.PercentageComplex)), 3);
                        //AJUSTAR PARA LAS TRES SECCIONEs
                        if (ValorMinAcep != 0 && ValorMaxAcep != 0)
                        {

                            if (ValorAbsoluto < ValorMinAcep || ValorAbsoluto > ValorMaxAcep)
                            {
                                listErrors.Add(new ErrorColumns(1, 1, "El resultado final no entá dentro de los parametros de -> Valor mínimo de aceptación y Valor máximo de aceptación. Valor mínimo:" + ValorMinAcep + " Valor máximo: " + ValorMaxAcep + " y Resultado final: " + ValorAbsoluto));
                            }
                        }
                    }
                    else
                    {
                        /*return new ApiResponse<ResultISZTests>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "No se encuentra el valor nominal para las posiciones (Todas)",
                            Structure = null
                        };*/

                    }
                }
                else
                {
                    data1 = request.Data.SeccionesISZTestsDetails[0].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.NominalAT));
                    data2 = request.Data.SeccionesISZTestsDetails[1].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.NominalAT));
                    data3 = request.Data.SeccionesISZTestsDetails[2].ISZTestsDetails.FirstOrDefault(x => x.Position1.Equals(request.Data.NominalBT));


                    if (data1 !=null & data2!=null && data3 != null)
                    {

                        CalculateFormulas(data1,data2,data3, request.Data);
                        decimal ValorAbsoluto1 = Math.Round(Convert.ToDecimal(Complex.Abs(data1.PercentageComplex)), 3);
                        decimal ValorAbsoluto2 = Math.Round(Convert.ToDecimal(Complex.Abs(data2.PercentageComplex)), 3);
                        decimal ValorAbsoluto3 = Math.Round(Convert.ToDecimal(Complex.Abs(data3.PercentageComplex)), 3);

                        


                        if (ValorMinAcepSec1 > 0 && ValorMaxAcepSec1 > 0)
                        {
                            //AJUSTAR PARA LAS TRES SECCIONEs
                            if ((ValorAbsoluto1 < ValorMinAcepSec1 || ValorAbsoluto1 > ValorMaxAcepSec1))
                            {
                                listErrors.Add(new ErrorColumns(1, 1, "El resultado final no entá dentro de los parametros de -> Valor mínimo de aceptación seccion 1 y Valor máximo de aceptación seccion 1. Valor mínimo seccion 1:" + ValorMinAcepSec1 + " Valor máximo seccion 1: " + ValorMaxAcepSec1 + " y Resultado final seccion 1: " + ValorAbsoluto1));
                            }
                        }

                        if (ValorMinAcepSec2 > 0 && ValorMaxAcepSec2 > 0)
                        {
                            //AJUSTAR PARA LAS TRES SECCIONEs
                            if ((ValorAbsoluto2 < ValorMinAcepSec2 || ValorAbsoluto2 > ValorMaxAcepSec2))
                            {
                                listErrors.Add(new ErrorColumns(1, 1, "El resultado final no entá dentro de los parametros de -> Valor mínimo de aceptación seccion 2 y Valor máximo de aceptación seccion 2. Valor mínimo seccion 2:" + ValorMinAcepSec2 + " Valor máximo seccion 2: " + ValorMaxAcepSec2 + " y Resultado final seccion 2: " + ValorAbsoluto2));
                            }
                        }

                        if (ValorMinAcepSec3 > 0 && ValorMaxAcepSec3 > 0)
                        {
                            //AJUSTAR PARA LAS TRES SECCIONEs
                            if ((ValorAbsoluto3 < ValorMinAcepSec3 || ValorAbsoluto3 > ValorMaxAcepSec3))
                            {
                                listErrors.Add(new ErrorColumns(1, 1, "El resultado final no entá dentro de los parametros de -> Valor mínimo de aceptación seccion 3 y Valor máximo de aceptación seccion 3. Valor mínimo seccion 3:" + ValorMinAcepSec3 + " Valor máximo seccion 3: " + ValorMaxAcepSec3 + " y Resultado final seccion 3: " + ValorAbsoluto3));
                            }
                        }

                    }
                    else
                    {
                        return new ApiResponse<ResultISZTests>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "No se ha encontrado valor nominal para alguna de las secciones",
                            Structure = null
                        };
                    }

                 
                }

                

                result.ISZTests = ResultISZTests;
             
                result.Results = listErrors.ToList();

                
                return new ApiResponse<ResultISZTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                 };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultISZTests>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }


        #endregion

        private void CalculateFormulas(ISZTestsDetails data1, ISZTestsDetails data2 , ISZTestsDetails data3, OutISZTests dataGeneral )
        {
            decimal R_Z1NS = 0;
            decimal X_Z1NS = 0;
            decimal Z_Z1NS = 0;
            decimal RAD_Z1NS = 0;
            decimal DEGRESS_Z1NS = 0;

            decimal R_Z1No = 0;
            decimal X_Z1No = 0;
            decimal Z_Z1No = 0;
            decimal RAD_Z1No = 0;
            decimal DEGRESS_Z1No = 0;

            decimal R_Z2No = 0;
            decimal X_Z2No = 0;
            decimal Z_Z2No = 0;
            decimal RAD_Z2No = 0;
            decimal DEGRESS_Z2No = 0;

            R_Z1NS = Math.Round(Convert.ToDecimal(data1.PercentageComplex.Real), 14);
            X_Z1NS = Math.Round(Convert.ToDecimal(data1.PercentageComplex.Imaginary), 14);
            Z_Z1NS = Math.Round(Convert.ToDecimal(Complex.Abs(data1.PercentageComplex)), 14);
            RAD_Z1NS = Math.Round(Convert.ToDecimal(Math.Acos(Convert.ToDouble(R_Z1NS / Z_Z1NS))), 14);
            DEGRESS_Z1NS = Math.Round(Convert.ToDecimal(Convert.ToDouble(RAD_Z1NS) * 180 / Math.PI), 14);


            R_Z1No = Math.Round(Convert.ToDecimal(data2.PercentageComplex.Real), 14);
            X_Z1No = Math.Round(Convert.ToDecimal(data2.PercentageComplex.Imaginary), 14);
            Z_Z1No = Math.Round(Convert.ToDecimal(Complex.Abs(data2.PercentageComplex)), 14);
            RAD_Z1No = Math.Round(Convert.ToDecimal(Math.Acos(Convert.ToDouble(R_Z1No / Z_Z1No))), 14);
            DEGRESS_Z1No = Math.Round(Convert.ToDecimal(Convert.ToDouble(RAD_Z1No) * 180 / Math.PI), 14);


            R_Z2No = Math.Round(Convert.ToDecimal(data3.PercentageComplex.Real),14);
            X_Z2No = Math.Round(Convert.ToDecimal(data3.PercentageComplex.Imaginary), 14);
            Z_Z2No = Math.Round(Convert.ToDecimal(Complex.Abs(data3.PercentageComplex)), 14);
            RAD_Z2No = Math.Round(Convert.ToDecimal(Math.Acos(Convert.ToDouble(R_Z2No / Z_Z2No))), 14);
            DEGRESS_Z2No = Math.Round(Convert.ToDecimal(Convert.ToDouble(RAD_Z2No) * 180 / Math.PI), 14);


            decimal R_Z1No_Z1Ns = R_Z1No - R_Z1NS;
            decimal X_Z1No_Z1Ns = X_Z1No - X_Z1NS;
            decimal Z_Z1No_Z1Ns = Convert.ToDecimal(Math.Sqrt(Decimal.ToDouble((R_Z1No_Z1Ns* R_Z1No_Z1Ns) + (X_Z1No_Z1Ns* X_Z1No_Z1Ns))));
            decimal RAD_Z1No_Z1Ns = Convert.ToDecimal(Math.Asin(Convert.ToDouble(X_Z1No_Z1Ns / Z_Z1No_Z1Ns)));
            decimal DEGRESS_Z1No_Z1Ns = Convert.ToDecimal(Convert.ToDouble(RAD_Z1No_Z1Ns) * 180 / Math.PI);

            decimal Z_Z2NoxZ1No_Z1Ns = Z_Z2No * Z_Z1No_Z1Ns;
            decimal DEGRESS_Z2NoxZ1No_Z1Ns = DEGRESS_Z2No + DEGRESS_Z1No_Z1Ns;
            decimal RAD_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Convert.ToDouble(DEGRESS_Z2NoxZ1No_Z1Ns) * Math.PI / 180);
            decimal X_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Sin(Convert.ToDouble(RAD_Z2NoxZ1No_Z1Ns)) * Convert.ToDouble(Z_Z2NoxZ1No_Z1Ns));
            decimal R_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Cos(Convert.ToDouble(RAD_Z2NoxZ1No_Z1Ns)) * Convert.ToDouble(Z_Z2NoxZ1No_Z1Ns));
            decimal XP_Z2NoxZ1No_Z1Ns = (R_Z2No * X_Z1No_Z1Ns) + (R_Z1No_Z1Ns * X_Z2No);
            decimal RP_Z2NoxZ1No_Z1Ns = (R_Z1No_Z1Ns * R_Z2No) + ((X_Z1No_Z1Ns * X_Z2No * -1));
            decimal ZP_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((RP_Z2NoxZ1No_Z1Ns * RP_Z2NoxZ1No_Z1Ns) + (XP_Z2NoxZ1No_Z1Ns * XP_Z2NoxZ1No_Z1Ns))));
            decimal RADP_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Asin(Convert.ToDouble(RP_Z2NoxZ1No_Z1Ns / ZP_Z2NoxZ1No_Z1Ns)));
            decimal DEGRESSP_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Convert.ToDouble(RADP_Z2NoxZ1No_Z1Ns) * 180 / Math.PI);


            decimal RAZI_Z_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(Z_Z2NoxZ1No_Z1Ns)));// este va en z3 de la tabla
            decimal RAZI_DEGRESS_Z2NoxZ1No_Z1Ns = DEGRESS_Z2NoxZ1No_Z1Ns / 2;
            decimal RAZI_RAD_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Convert.ToDouble(RAZI_DEGRESS_Z2NoxZ1No_Z1Ns) * Math.PI / 180);
            decimal RAZI_X_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Sin(Convert.ToDouble(RAZI_RAD_Z2NoxZ1No_Z1Ns)) * Convert.ToDouble(RAZI_Z_Z2NoxZ1No_Z1Ns));// este va en z3 de la tabla
            decimal RAZI_R_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Cos(Convert.ToDouble(RAZI_RAD_Z2NoxZ1No_Z1Ns)) * Convert.ToDouble(RAZI_Z_Z2NoxZ1No_Z1Ns));// este va en z3 de la tabla
            decimal RAZI_ZP_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((ZP_Z2NoxZ1No_Z1Ns))));
            decimal RAZI_DEGRESSP_Z2NoxZ1No_Z1Ns = DEGRESSP_Z2NoxZ1No_Z1Ns / 2;
            decimal RAZI_RADP_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Convert.ToDouble(RAZI_DEGRESSP_Z2NoxZ1No_Z1Ns) * Math.PI / 180);
            decimal RAZI_RP_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Cos(Convert.ToDouble(RAZI_RADP_Z2NoxZ1No_Z1Ns)) * Convert.ToDouble(RAZI_ZP_Z2NoxZ1No_Z1Ns));
            decimal RAZI_XP_Z2NoxZ1No_Z1Ns = Convert.ToDecimal(Math.Sin(Convert.ToDouble(RAZI_RADP_Z2NoxZ1No_Z1Ns * RAZI_ZP_Z2NoxZ1No_Z1Ns)));

            //en la z2 de la tabla
            decimal RZ2 = R_Z2No - RAZI_R_Z2NoxZ1No_Z1Ns;
            decimal XZ2 = X_Z2No - RAZI_X_Z2NoxZ1No_Z1Ns;
            decimal ZZ2 = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((RZ2 * RZ2) + (XZ2 * XZ2))));

            //en la z1 de la tabla
            decimal RZ1 = R_Z1No - RAZI_R_Z2NoxZ1No_Z1Ns;
            decimal XZ1 = X_Z1No - RAZI_X_Z2NoxZ1No_Z1Ns;
            decimal ZZ1 = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((RZ1 * RZ1) + (XZ1 * XZ1))));


            dataGeneral.SeccionesISZTestsDetails[0].Porc_R = Math.Round(RZ1, 3);
            dataGeneral.SeccionesISZTestsDetails[0].Porc_Z = Math.Round(ZZ1, 3);
            dataGeneral.SeccionesISZTestsDetails[0].Porc_X = Math.Round(XZ1, 3);

            dataGeneral.SeccionesISZTestsDetails[1].Porc_R = Math.Round(RZ2, 3);
            dataGeneral.SeccionesISZTestsDetails[1].Porc_Z = Math.Round(ZZ2, 3);
            dataGeneral.SeccionesISZTestsDetails[1].Porc_X = Math.Round(XZ2, 3);

            dataGeneral.SeccionesISZTestsDetails[2].Porc_R = Math.Round(RAZI_R_Z2NoxZ1No_Z1Ns, 3);
            dataGeneral.SeccionesISZTestsDetails[2].Porc_Z = Math.Round(RAZI_Z_Z2NoxZ1No_Z1Ns, 3);
            dataGeneral.SeccionesISZTestsDetails[2].Porc_X = Math.Round(RAZI_X_Z2NoxZ1No_Z1Ns, 3);



        }
    }
}
