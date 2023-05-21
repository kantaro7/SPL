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

    public class SaveReportRADHandler : IRequestHandler<SaveReportRADCommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportRADHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveReportRADCommand request, CancellationToken cancellationToken)
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

                if (string.IsNullOrEmpty(request.Data.TypeUnit.Trim()))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Tipo de unidad es requerido",
                        Structure = -1
                    };
                }

                if (request.Data.TypeUnit.Trim().Length > 3)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Tipo de unidad no puede ser mayor a 3 caracteres",
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

                foreach (HeaderRADTests item in request.Data.DataTests.Headers)
                {
                    int registro = request.Data.DataTests.Headers.IndexOf(item);

                    if (item.TestDate > DateTime.Now)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El valor Fecha de prueba no puede ser mayor a la fecha actual, en el registro nro " + registro + 1,
                            Structure = -1
                        };
                    }
                }

                long result = await this._infrastructure.SaveInfoRADReport(request.Data);

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

