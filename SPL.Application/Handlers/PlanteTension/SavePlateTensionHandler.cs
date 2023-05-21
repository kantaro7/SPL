using MediatR;

using SPL.Artifact.Application.Commands.Artifactdesign;
using SPL.Artifact.Application.Commands.PlateTension;
using SPL.Artifact.Application.Common;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Artifact.PlateTension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.PlateTension
{
    public class SaveBaseTemplateHandler : IRequestHandler<SavePlateTensionCommand, ApiResponse<long>>
    {

        private readonly IPlateTensionInfrastructure _infrastructure;

        public SaveBaseTemplateHandler(IPlateTensionInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<long>> Handle(SavePlateTensionCommand request, CancellationToken cancellationToken)
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

                int count = 0;

                foreach (var item in request.Data)
                {
                    count++;
                    if (string.IsNullOrEmpty(item.Unidad))
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Uno de los elementos no contiene el valor Unidad",
                            Structure = -1
                        };
                    }
                 
                    bool valLongitud = Validations.validacion55Caracteres(item.Unidad.Trim());
                    bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(item.Unidad.Trim());

                    if (valLongitud)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El valor Unidad no puede excederse de 55 caracteres, en el registro nro " + count,
                            Structure = -1
                        };
                    }
                    if (valFormat)
                    {

                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El valor Unidad solo debe contener letras, números y guion medio, en el registro nro. " + count,
                            Structure = -1
                        };
                    }

                    if (item.TipoTension.Trim().Length > 3)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El valor Tipo tension solo puede contener 3 caracteres, en el registro nro. " + count,
                            Structure = -1
                        };
                    }
                    
                    if (item.Orden.ToString().Length > 5)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El valor Orden solo puede contener 5 dígitos, en el registro nro. " + count,
                            Structure = -1
                        };
                    }

                   
                }

                var result = await _infrastructure.savePlateTension(request.Data,request.StatusDelete);

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

