using MediatR;

using SPL.Artifact.Application.Commands.Artifactdesign;
using SPL.Artifact.Application.Commands.BaseTemplate;
using SPL.Artifact.Application.Commands.Nozzles;
using SPL.Artifact.Application.Commands.PlateTension;
using SPL.Artifact.Application.Common;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Artifact.BaseTemplate;
using SPL.Domain.SPL.Artifact.Nozzles;
using SPL.Domain.SPL.Artifact.PlateTension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.Nozzles
{
    public class SaveRecordNozzleInformationHandler : IRequestHandler<SaveRecordNozzleInformationCommand, ApiResponse<long>>
    {

        private readonly INozzlesInfrastructure _infrastructure;

        public SaveRecordNozzleInformationHandler(INozzlesInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveRecordNozzleInformationCommand request, CancellationToken cancellationToken)
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

                if (request.Data.TotalQuantity != request.Data.NozzleInformation.Where(x => !x.Prueba).Count())
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "La cantidad de boquillas no concuerda con las que tiene el aparato",
                        Structure = -1
                    };
                   
                }
                foreach (var item in request.Data.NozzleInformation)
                {
                    if (string.IsNullOrEmpty(item.Posicion))
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Posición es un campo requerido",
                            Structure = -1
                        };
                    }

                   

                    if (string.IsNullOrEmpty(item.NoSerie.Trim()))
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El número de serie es requerido",
                            Structure = -1
                        };
                    }

                    bool valLongitud = Validations.validacion55Caracteres(item.NoSerie.Trim());
                    bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(item.NoSerie.Trim());

                    if (valLongitud)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El número de serie no puede excederse de 50 caracteres",
                            Structure = -1
                        };
                    }
                    if (valFormat)
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El número de serie solo debe contener letras, números y guion medio",
                            Structure = -1
                        };
                    }

                    if (string.IsNullOrEmpty(item.NoSerie.Trim()))
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El número de serie es requerido",
                            Structure = -1
                        };
                    }

                    bool valLongitudBoquilla = Validations.validacion50Caracteres(item.NoSerieBoq.Trim());
                    bool valFormatBoquilla = Validations.validacionCaracteresNoSeriePruebasConsutla(item.NoSerieBoq.Trim());

                    if (valLongitudBoquilla)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El número de serie de la boquilla no puede excederse de 50 caracteres",
                            Structure = -1
                        };
                    }
                    if (valFormatBoquilla)
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El número de serie de la boquilla solo debe contener letras, números y guion medio",
                            Structure = -1
                        };
                    }

                    if (request.Data.NozzleInformation.Where(x=>x.NoSerieBoq.Equals(item.NoSerieBoq)).Count()>1)
                    {
                            return new ApiResponse<long>()
                            {
                                Code = (int)ResponsesID.fallido,
                                Description = "No se permite números de serie de la boquilla repetidos",
                                Structure = -1
                            };
                    }

                    if (item.IdMarca <= 0)
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Marca es requerido",
                            Structure = -1
                        };
                    }

                    if (item.IdTipo <= 0)
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Tipo es requerido",
                            Structure = -1
                        };
                    }

                    if (item.FactorPotencia <= 0)
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Factor de Potencia es requerido",
                            Structure = -1
                        };
                    } 
                    
                    if (item.FactorPotencia <= 0)
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Factor de Potencia debe ser numérico mayor a cero considerando 3 enteros con 3 decimales",
                            Structure = -1
                        };
                    }
                }

                var result = await _infrastructure.saveRecordNozzleInformation(request.Data);

                return new ApiResponse<long>()
                {
                    Code = result <=0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result <= 0 ? "Hubo un error guardando los datos" : "Guardado con éxito",
                    Structure = result
                };

            }
            catch (Exception ex)
            {

                return new ApiResponse<long>()
                {
                    Code = (int)(ResponsesID.fallido),
                    Description = ex.Message,
                    Structure = -1
                };

            }
         }
     }
       
    

        #endregion
    
}

