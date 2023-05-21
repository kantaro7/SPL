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

    public class SaveReportRANHandler : IRequestHandler<SaveReportRANCommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportRANHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveReportRANCommand request, CancellationToken cancellationToken)
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

                int positionPadre;
                foreach (var item in request.Data.Rans)
                {
                    positionPadre = request.Data.Rans.IndexOf(item);

                    foreach (var item2 in item.RANTestsDetailsRAs)
                    {
                        int position = item.RANTestsDetailsRAs.IndexOf(item2);

                        if (!string.IsNullOrEmpty(item2.Duration.Trim()) && string.IsNullOrEmpty(item2.Description.Trim()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Descripción es requerido debido a que Duración contiene valor, en el registro nro. "+ (position + 1) + " ubicado enla prueba nro. "+ (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                           
                        }

                        if (item2.Description.Trim().Length > 25)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Descripción no puede excederse de 25 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (!string.IsNullOrEmpty(item2.Description.Trim()) && string.IsNullOrEmpty(item2.UMMeasurement.Trim()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Unidad de Medición es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (!int.TryParse(item2.Measurement.ToString(), out _))
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Medición no debe poseer decimales, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (Math.Round(item2.Measurement) > 99999999)
                        {
                            return new ApiResponse<long>()
                            {
                            
                                 Code = (int)ResponsesID.fallido,
                                Description = "El valor Medición no puede ser mayor a 8 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (!string.IsNullOrEmpty(item2.Description.Trim()) && item2.Measurement <= 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Medición es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if ((item2.UMMeasurement.Trim().Length > 15))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Unidad de Medición no debe superar los 15 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (!string.IsNullOrEmpty(item2.Description.Trim()) && item2.VCD <= 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo VCD es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (!int.TryParse(item2.VCD.ToString(), out _))
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor VCD no debe poseer decimales, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (Math.Round(item2.VCD) > 99999999)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor VCD no puede ser mayor a 8 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                   

                        if (!string.IsNullOrEmpty(item2.Description.Trim()) && item2.Limit <= 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Limite es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (!int.TryParse(item2.Limit.ToString(), out _))
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Limite no debe poseer decimales, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (Math.Round(item2.Limit) > 99999999)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Limite no puede ser mayor a 8 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                       

                        if (!string.IsNullOrEmpty(item2.Description.Trim()) && string.IsNullOrEmpty(item2.Duration.Trim()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Duración es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (item2.Duration.Length > 10)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Duración no puede ser mayor a 10 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (!string.IsNullOrEmpty(item2.Description.Trim()) && item2.Time <= 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Tiempo es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (!int.TryParse(item2.Time.ToString(), out _))
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Tiempo no debe poseer decimales, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (Math.Round(item2.Time) > 99)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Tiempo no puede ser mayor a 2 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (!string.IsNullOrEmpty(item2.Description.Trim()) && string.IsNullOrEmpty(item2.UMTime.Trim()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Unidad de medida de tiempo es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (item2.UMTime.Length > 10)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Unidad de medida de tiempo no puede ser mayor a 10 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                    }

                    foreach (var item3 in item.RANTestsDetailsTAs)
                    {

                        int position = item.RANTestsDetailsTAs.IndexOf(item3);

                        if (!string.IsNullOrEmpty(item3.Duration.Trim()) && string.IsNullOrEmpty(item3.Description.Trim()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Descripción es requerido debido a que Duración contiene valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (item3.Description.Trim().Length > 25)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Descripción no puede excederse de 25 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (!string.IsNullOrEmpty(item3.Description.Trim()) && item3.VCD <= 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo VCD es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (!int.TryParse(item3.VCD.ToString(), out _))
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor VCD no debe poseer decimales, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (Math.Round(item3.VCD) > 99999999)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor VCD no puede ser mayor a 8 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (!string.IsNullOrEmpty(item3.Description.Trim()) && string.IsNullOrEmpty(item3.Duration.Trim()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Duración es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (item3.Duration.Length > 10)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Duración no puede ser mayor a 10 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (!string.IsNullOrEmpty(item3.Description.Trim()) && item3.Time <= 0)
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Tiempo es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (!int.TryParse(item3.Time.ToString(), out _))
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Tiempo no debe poseer decimales, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (Math.Round(item3.Time) > 99)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Tiempo no puede ser mayor a 2 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                        if (!string.IsNullOrEmpty(item3.Description.Trim()) && string.IsNullOrEmpty(item3.UMTime.Trim()))
                        {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "El campo Unidad de medida de tiempo es requerido debido a que Descripción contiene un valor, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };

                        }

                        if (item3.UMTime.Length > 10)
                        {
                            return new ApiResponse<long>()
                            {

                                Code = (int)ResponsesID.fallido,
                                Description = "El valor Unidad de medida de tiempo no puede ser mayor a 10 caracteres, en el registro nro. " + (position + 1) + " ubicado enla prueba nro. " + (positionPadre + 1) + " en la sección nro 10",
                                Structure = -1
                            };
                        }

                     

                    }

                }

           

                long result = await this._infrastructure.SaveInfoRANReport(request.Data);

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

