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

    public class SaveReportCEMHandler : IRequestHandler<SaveReportCEMCommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportCEMHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveReportCEMCommand request, CancellationToken cancellationToken)
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


                //if (request.Data.Data.PorcZ == 0)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Favor de proporcionar el %Z",
                //        Structure = -1
                //    };
                //}

                //int[] ValidCapBaseMin = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(request.Data.Data.PorcZ)));

                //if (ValidCapBaseMin[0] > 2 || ValidCapBaseMin[1] > 3)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "%Z debe ser mayor a cero considerando 2 enteros con 3 decimales",
                //        Structure = -1
                //    };
                //}







                //if (request.Data.Data.EmptyLosses == 0)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Favor de proporcionar las pérdidas en vacío",
                //        Structure = -1
                //    };
                //}

                //int[] ValidEmptyLosses = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(request.Data.Data.EmptyLosses)));

                //if (ValidEmptyLosses[0] > 6 || ValidEmptyLosses[1] > 3)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Pérdidas en vacío debe ser mayor a cero considerando 6 enteros con 3 decimales",
                //        Structure = -1
                //    };
                //}




                //if (request.Data.Data.Lostload == 0)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Favor de proporcionar las pérdidas con carga",
                //        Structure = -1
                //    };
                //}

                //int[] ValidLostload = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(request.Data.Data.Lostload)));

                //if (ValidLostload[0] > 6 || ValidLostload[1] > 3)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Pérdidas con carga debe ser mayor a cero considerando 6 enteros con 3 decimales",
                //        Structure = -1
                //    };
                //}






                //if (request.Data.Data.LostCooldown == 0)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Favor de proporcionar las pérdidas por equipo de enfriamiento",
                //        Structure = -1
                //    };
                //}

                //int[] ValidLostCooldown = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(request.Data.Data.LostCooldown)));

                //if (ValidLostCooldown[0] > 6 || ValidLostCooldown[1] > 3)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Pérdidas por equipo de enfriamiento debe ser mayor a cero considerando 6 enteros con 3 decimales",
                //        Structure = -1
                //    };
                //}





                //if (request.Data.Data.TotalLosses == 0)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Favor de proporcionar las pérdidas totales",
                //        Structure = -1
                //    };
                //}

                //int[] ValidTotalLosses = CommonMethods.cantDigitsPoint(Math.Abs(Convert.ToDouble(request.Data.Data.TotalLosses)));

                //if (ValidTotalLosses[0] > 6 || ValidTotalLosses[1] > 3)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Pérdidas totales debe ser mayor a cero considerando 6 enteros con 3 decimales",
                //        Structure = -1
                //    };
                //}

                if (request.Data.StatusAllPosSec)
                {
                    request.Data.PosSecundary = "Todas";
                }

                long result = await this._infrastructure.SaveInfoCEMReport(request.Data);

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

