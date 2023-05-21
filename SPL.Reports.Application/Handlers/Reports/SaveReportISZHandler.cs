namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using SPL.Artifact.Application.Commands.Reports;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Common;

    public class SaveReportISZHandler : IRequestHandler<SaveReportISZCommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportISZHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveReportISZCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data == null)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Estructura incorrecta",
                        Structure = -1
                    };
                }

              

                if (string.IsNullOrEmpty(request.Data.TypeReport.Trim()))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Tipo de reporte es requerido",
                        Structure = -1
                    };
                }

                if (request.Data.TypeReport.Trim().Length > 3)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Tipo de reporte no puede ser mayor a 3 caracteres",
                        Structure = -1
                    };
                }

                if (string.IsNullOrEmpty(request.Data.KeyTest.Trim()))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave prueba es requerido",
                        Structure = -1
                    };
                }

                if (request.Data.KeyTest.Trim().Length > 3)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave prueba no puede ser mayor a 3 caracteres",
                        Structure = -1
                    };
                }

                if (string.IsNullOrEmpty(request.Data.LanguageKey.Trim()))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave idioma es requerido",
                        Structure = -1
                    };
                }

                if (request.Data.LanguageKey.Trim().Length > 2)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave idioma no puede ser mayor a 2 caracteres",
                        Structure = -1
                    };
                }

                if (Validations.validacion55Caracteres(request.Data.SerialNumber.Trim()))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Nro de serie no puede excederse de 55 caracteres",
                        Structure = -1
                    };
                }
                if (Validations.validacionCaracteresNoSeriePruebasConsutla(request.Data.SerialNumber.Trim()))
                {

                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Nro de serie solo debe contener letras, números y guion medio",
                        Structure = -1
                    };
                }

                if (!int.TryParse(request.Data.TestNumber.ToString(), out _))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Nro de prueba no debe poseer decimales",
                        Structure = -1
                    };
                }

                if (request.Data.TestNumber <= 0)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Nro de prueba es requerido",
                        Structure = -1
                    };
                }

                if (request.Data.TestNumber > 99)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Nro de prueba no puede ser mayor a 2 caracteres",
                        Structure = -1
                    };
                }


                if (request.Data.Data.CapBaseMin == 0)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Favor de proporcionar la capacidad base",
                        Structure = -1
                    };
                }
                int[] ValidCapBaseMin = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(request.Data.Data.CapBaseMin)));
                if (ValidCapBaseMin[0] > 4 || ValidCapBaseMin[1] > 3)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Capacidad base debe ser mayor a cero considerando 4 enteros con 3 decimales",
                        Structure = -1
                    };
                }


                if (request.Data.Data.Temperature == 0)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Favor de proporcionar la temperatura",
                        Structure = -1
                    };
                }
                int[] ValidTemperature = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(request.Data.Data.Temperature)));
                if (ValidTemperature[0] > 3 || ValidTemperature[1] > 2)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Temperatura debe ser mayor a cero considerando 3 enteros con 2 decimales",
                        Structure = -1
                    };
                }
                foreach (var item in request.Data.Data.SeccionesISZTestsDetails)
                {
                    foreach (var item2 in item.ISZTestsDetails)
                    {

                        if (!string.IsNullOrEmpty(item2.Position1) && !string.IsNullOrEmpty(item2.Position2))
                        {
                            if( item2.FactorCorr < 0 || item2.FactorCorr > 999)
                            {
                                return new ApiResponse<long>()
                                {
                                    Code = (int)ResponsesID.fallido,
                                    Description = "El factor de correccion debe ser mayor a 0 conciderando 3 enteros y 6 decimales ",
                                    Structure = -1
                                };
                            }

                            if (item2.VoltsVRMS == 0)
                            {
                                return new ApiResponse<long>()
                                {
                                    Code = (int)ResponsesID.fallido,
                                    Description = "Favor de proporcionar la tensión vrms para la posición",
                                    Structure = -1
                                };
                            }


                            if (item2.PowerKW == 0)
                            {
                                return new ApiResponse<long>()
                                {
                                    Code = (int)ResponsesID.fallido,
                                    Description = "Favor de proporcionar la potencia kw para la posición",
                                    Structure = -1
                                };
                            }



                            if (item2.CurrentsIRMS == 0)
                            {
                                return new ApiResponse<long>()
                                {
                                    Code = (int)ResponsesID.fallido,
                                    Description = "Favor de proporcionar la corriente irms para la posición",
                                    Structure = -1
                                };
                            }


                            int[] ValidVoltsVRMS = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.VoltsVRMS)));
                            if (ValidVoltsVRMS[0] > 6 || ValidVoltsVRMS[1] > 1)
                            {
                                return new ApiResponse<long>()
                                {
                                    Code = (int)ResponsesID.fallido,
                                    Description = "El valor de la tensión vrms debe ser numérico mayor a cero considerando 6 enteros con 1 decimales",
                                    Structure = -1
                                };
                            }




                            int[] ValidCurrentsIRMS = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.CurrentsIRMS)));
                            if (ValidCurrentsIRMS[0] > 6 || ValidCurrentsIRMS[1] > 3)
                            {
                                return new ApiResponse<long>()
                                {
                                    Code = (int)ResponsesID.fallido,
                                    Description = "El valor de la corriente irms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                                    Structure = -1
                                };
                            }



                            int[] ValidPowerKW = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.PowerKW)));
                            if (ValidPowerKW[0] > 3 || ValidPowerKW[1] > 3)
                            {
                                return new ApiResponse<long>()
                                {
                                    Code = (int)ResponsesID.fallido,
                                    Description = "El valor de la potencia kw debe ser numérico mayor a cero considerando 3 enteros con 3 decimales",
                                    Structure = -1
                                };
                            }
                        }





                    }
                }




                long result = await this._infrastructure.SaveInfoISZReport(request.Data);

                return new ApiResponse<long>()
                {
                    Code = result == Enums.EnumsGen.Error ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == Enums.EnumsGen.Error ? "Hubo un error al guardar los datos" : "Datos guardados exitosamente",
                    Structure = result
                };
            }
            catch (DbUpdateException ex)
            {

                return new ApiResponse<long>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = Enums.EnumsGen.Error
                };
            }

            catch (Exception ex)
            {
                return new ApiResponse<long>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = Enums.EnumsGen.Error
                };
            }
        }
        #endregion
    }
}

