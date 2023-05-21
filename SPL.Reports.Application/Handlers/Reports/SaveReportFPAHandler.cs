namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using SPL.Artifact.Application.Commands.Reports;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Common;

    public class SaveReportFPAHandler : IRequestHandler<SaveReportFPACommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportFPAHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveReportFPACommand request, CancellationToken cancellationToken)
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


            


                //if (string.IsNullOrEmpty(request.Data.PosAT))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Favor de proporcionar la posición en AT",
                //        Structure = -1
                //    };
                //}



                //if (request.Data.PosAT.Length > 5)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "La posición en AT no puede excederse de 5 caracteres",
                //        Structure = -1
                //    };
                //}  
                
                //if (Regex.IsMatch(request.Data.PosAT, @"^[a-zA-Z0-9]*$"))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "La posición en AT solo puede contener letras y/o números",
                //        Structure = -1
                //    };
                //}







                //if (string.IsNullOrEmpty(request.Data.PosBT))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Favor de proporcionar la posición en BT",
                //        Structure = -1
                //    };
                //}

                //if (request.Data.PosBT.Length > 5)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "La posición en BT no puede excederse de 5 caracteres",
                //        Structure = -1
                //    };
                //}
                //if (Regex.IsMatch(request.Data.PosBT, @"^[a-zA-Z0-9]*$"))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "La posición en BT solo puede contener letras y/o números",
                //        Structure = -1
                //    };
                //}


                //if (string.IsNullOrEmpty(request.Data.PosTER))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Favor de proporcionar la posición en Terciario",
                //        Structure = -1
                //    };
                //}

                //if (request.Data.PosTER.Length > 5)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "La posición en TER no puede excederse de 5 caracteres",
                //        Structure = -1
                //    };
                //}

                //if (Regex.IsMatch(request.Data.PosTER, @"^[a-zA-Z0-9]*$"))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "La posición en TER solo puede contener letras y/o números",
                //        Structure = -1
                //    };
                //}


                long result = await this._infrastructure.SaveInfoFPAReport(request.Data);

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

