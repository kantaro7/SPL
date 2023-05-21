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
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Reports.Application.Common;

    public class SaveReportPCIHandler : IRequestHandler<SaveReportPCICommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportPCIHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods

        public async Task<ApiResponse<long>> Handle(SaveReportPCICommand request, CancellationToken cancellationToken)
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

                if (request.Data.TypeReport.Trim().Length > 7)
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

                if (request.Data.KeyTest.Trim().Length > 7)
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

                if (string.IsNullOrEmpty(request.Data.WindingMaterial))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Favor de proporcionar material devanado",
                        Structure = -1
                    };
                }

                if (request.Data.Date > DateTime.Now)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Fecha de prueba debe ser menor o igual a la fecha actual.",
                        Structure = -1
                    };
                }

                foreach (PCITestRating item in request.Data.Ratings)
                {
                    int position = request.Data.Ratings.IndexOf(item);

                    foreach (PCISecondaryPositionTest item2 in item.SecondaryPositions)
                    {
                        int[] ValidI2r_sec = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.I2RSecondary)));

                        if (ValidI2r_sec[0] > 17 || ValidI2r_sec[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor I2r_sec " + item2.I2RSecondary + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidLossesCorrected = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.LossesCorrected)));

                        if (ValidLossesCorrected[0] > 17 || ValidLossesCorrected[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor Perd_Corregidas " + item2.LossesCorrected + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidPorcentajeR = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.PercentageR)));

                        if (ValidPorcentajeR[0] > 17 || ValidPorcentajeR[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor PorcentajeR " + item2.PercentageR + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidPorcentajeX = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.PercentageX)));

                        if (ValidPorcentajeX[0] > 17 || ValidPorcentajeX[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor PorcentajeX " + item2.PercentageX + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidPorcentajeZ = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.PercentageZ)));

                        if (ValidPorcentajeZ[0] > 17 || ValidPorcentajeZ[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor PorcentajeZ " + item2.PercentageZ + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidI2r_tot = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.I2RTotal)));

                        if (ValidI2r_tot[0] > 17 || ValidI2r_tot[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor I2r_tot " + item2.I2RTotal + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidI2rTotCorr = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.I2RCorrectedTotal)));

                        if (ValidI2rTotCorr[0] > 17 || ValidI2rTotCorr[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor I2rTotCorr " + item2.I2RCorrectedTotal + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidWcuCorr = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.WcuCor)));

                        if (ValidWcuCorr[0] > 17 || ValidWcuCorr[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor Wcu_cor " + item2.WcuCor + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidWind = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.Wind)));

                        if (ValidWind[0] > 17 || ValidWind[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor Wind " + item2.Wind + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }

                        int[] ValidWindCorr = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(item2.WindCorr)));

                        if (ValidWindCorr[0] > 17 || ValidWindCorr[1] > 7)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se puede realizar el registro porque el valor Wind " + item2.WindCorr + " supera el limite en BD, que es numeric(17,7). Por favor revise los datos y vuelva a calcular",
                                Structure = -1
                            };
                        }
                    }
                }

                long result = await this._infrastructure.SaveInfoPCIReport(request.Data);

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