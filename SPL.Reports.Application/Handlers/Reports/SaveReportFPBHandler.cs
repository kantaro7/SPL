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
    using SPL.Domain.SPL.Tests;
    using SPL.Reports.Application.Common;

    public class SaveReportFPBHandler : IRequestHandler<SaveReportFPBCommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportFPBHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveReportFPBCommand request, CancellationToken cancellationToken)
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

                if (string.IsNullOrEmpty(request.Data.TanDelta))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Favor de proporcionar TanDelta",
                        Structure = -1
                    };
                }

             

                foreach (Domain.SPL.Reports.FPB.FPBTests item in request.Data.Data)
                {
                    int position = request.Data.Data.IndexOf(item);
                   
                    if (item.Date > DateTime.Now)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Fecha de prueba debe ser menor o igual a la fecha actua, en la sección nro. " + (position + 1),
                            Structure = -1
                        };
                    }

                    if (item.Tension == 0)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Favor de proporcionar tensión de la prueba, en la sección nro. " + (position + 1),
                            Structure = -1
                        };
                    }

                    int[] validationTension = CommonMethods.cantDigitsPoint(Convert.ToDouble(item.Tension));

                    if (validationTension[0] > 6 || validationTension[1] > 3)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "La tensión de prueba en kV debe ser mayor a cero considerando 6 enteros con 3 decimales, en la sección nro. " + (position + 1),
                            Structure = -1
                        };
                    }

                    if (item.Temperature == 0)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Favor de proporcionar la temperatura en °C, en la sección nro. " + (position + 1),
                            Structure = -1
                        };
                    }

                    int[] validationTemperatura = CommonMethods.cantDigitsPoint(Convert.ToDouble(item.Tension));

                    if (validationTemperatura[0] > 3 || validationTemperatura[1] > 1)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "La temperatura en °C debe ser mayor a cero considerando 3 enteros con 1 decimal, en la sección nro. " + (position + 1),
                            Structure = -1
                        };
                    }

                    foreach (Domain.SPL.Reports.FPB.FPBTestsDetails itemDetail in item.FPBTestsDetails)
                    {
                        if (itemDetail.CorrectionFactorSpecifications20Grados == null)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El factor de corrección para 20 grados es requerido",
                                Structure = -1
                            };
                        }

                        if (itemDetail.CorrectionFactorSpecificationsTemperature == null)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El factor de corrección por temperatura es requerido",
                                Structure = -1
                            };
                        }
                        if (string.IsNullOrEmpty(itemDetail.T))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar el valor de la columna con título de T",
                                Structure = -1
                            };
                        }

                        if (itemDetail.T.Length > 2 )
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La columna T no puede excederse de 2 caracteres",
                                Structure = -1
                            };
                        }

                        if (itemDetail.Current == 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la corriente en mA por boquilla",
                                Structure = -1
                            };
                        }

                        int[] validationCurrent = CommonMethods.cantDigitsPoint(Convert.ToDouble(itemDetail.Current));

                        if (validationCurrent[0] > 6 || validationCurrent[1] > 3)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La corriente en mA debe ser mayor a cero considerando 6 enteros con 3 decimales",
                                Structure = -1
                            };
                        }  
                        
                        
                        
                        
                        if (itemDetail.Power == 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la potencia en W por boquilla",
                                Structure = -1
                            };
                        }

                        int[] validationPower = CommonMethods.cantDigitsPoint(Convert.ToDouble(itemDetail.Power));

                        if (validationPower[0] > 3 || validationPower[1] > 3)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La potencia en W debe ser mayor a cero considerando 3 enteros con 3 decimales",
                                Structure = -1
                            };
                        }

                        if (itemDetail.Capacitance == 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la capacitancia en pF por boquilla",
                                Structure = -1
                            };
                        }

                        int[] validationCapacitance = CommonMethods.cantDigitsPoint(Convert.ToDouble(itemDetail.Capacitance));

                        if (validationCapacitance[0] > 6 || validationCapacitance[1] > 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La capacitancia en pF debe ser mayor a cero considerando 6 enteros sin decimales",
                                Structure = -1
                            };
                        }
                    }
                }

                long result = await this._infrastructure.SaveInfoFPBReport(request.Data);

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

