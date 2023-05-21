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

    public class SaveReportRCTHandler : IRequestHandler<SaveReportRCTCommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportRCTHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveReportRCTCommand request, CancellationToken cancellationToken)
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

                if (request.Data.LoadDate > DateTime.Now)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Fecha de prueba debe ser menor o igual a la fecha actual",
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

                if (string.IsNullOrEmpty(request.Data.MeasurementType))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Favor de proporcionar el tipo de medición",
                        Structure = -1
                    };
                }
                if (request.Data.MeasurementType.Length > 15)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Tipo de medición no puede excederse de 15 caracteres",
                        Structure = -1
                    };
                }

                switch (request.Data.KeyTest)
                {
                    case "ATI":
                        if (!request.Data.MeasurementType.ToUpper().Equals("Individual".ToUpper()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Tipo de medición no valida",
                                Structure = -1
                            };
                        }
                        break;
                    case "ABS":
                        if (request.Data.MeasurementType.ToUpper().Equals("Simultáneos".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultaneous".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultáneo".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultaneo".ToUpper()))
                        { }
                        else
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Tipo de medición no valida",
                                Structure = -1
                            };
                        }

                        break;
                    case "ABI":
                        if (!request.Data.MeasurementType.ToUpper().Equals("Individual".ToUpper()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Tipo de medición no valida",
                                Structure = -1
                            };
                        }
                        break;
                    case "TSI":
                        if (request.Data.MeasurementType.ToUpper().Equals("Individual".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultáneos".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultaneous".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultáneo".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultaneo".ToUpper()))
                        { }
                        else
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Tipo de medición no valida",
                                Structure = -1
                            };
                        }

                        break;
                    case "TOS":
                        if (request.Data.MeasurementType.ToUpper().Equals("Simultáneos".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultaneous".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultáneo".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultaneo".ToUpper()))
                        { }
                        else
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Tipo de medición no valida",
                                Structure = -1
                            };
                        }

                        break;

                    case "TOI":
                        if (!request.Data.MeasurementType.ToUpper().Equals("Individual".ToUpper()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Tipo de medición no valida",
                                Structure = -1
                            };
                        }
                        break;

                    case "TIS":
                        if (request.Data.MeasurementType.ToUpper().Equals("Individual".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultáneos".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultaneous".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultáneo".ToUpper()) || request.Data.MeasurementType.ToUpper().Equals("Simultaneo".ToUpper()))
                        { }
                        else
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Tipo de medición no valida",
                                Structure = -1
                            };
                        }

                        break;

                    default:
                        break;
                }

                foreach (Domain.SPL.Reports.RCT.RCTTests item in request.Data.RCTTests)
                {

                    foreach (Domain.SPL.Reports.RCT.RCTTestsDetails item2 in item.RCTTestsDetails)
                    {

                        if (string.IsNullOrEmpty(item2.Phase))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la fase",
                                Structure = -1
                            };
                        }

                        if (item2.Phase.Length > 5)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La fase no puede excederse de 5 caracteres",
                                Structure = -1
                            };
                        }

                        if (string.IsNullOrEmpty(item2.Terminal))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la terminal",
                                Structure = -1
                            };
                        }

                        if (item2.Terminal.Length > 20)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La terminal no puede excederse de 20 caracteres",
                                Structure = -1
                            };
                        }

                        if (item2.Resistencia == 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la resistencia",
                                Structure = -1
                            };
                        }
                        int[] validationA = CommonMethods.cantDigitsPoint(Convert.ToDouble(item2.Resistencia));

                        if (validationA[0] > 5 || validationA[1] > 6)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Resistencia debe ser mayor a cero considerando 5 enteros con 4 decimales",
                                Structure = -1
                            };
                        }

                        if (string.IsNullOrEmpty(item2.Unit))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la unidad",
                                Structure = -1
                            };
                        }

                        if (item2.Unit.Length > 15)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "La unidad no puede excederse de 15 caracteres",
                                Structure = -1
                            };
                        }

                        if (item2.Temperature == 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Favor de proporcionar la temperatura",
                                Structure = -1
                            };
                        }
                        int[] validationB = CommonMethods.cantDigitsPoint(Convert.ToDouble(item2.Temperature));

                        if (validationB[0] > 3 || validationB[1] > 1)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "Temperatura debe ser mayor a cero considerando 3 enteros con 1 decimal",
                                Structure = -1
                            };
                        }
                    }
                }

                long result = await this._infrastructure.SaveInfoRCTReport(request.Data);

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

