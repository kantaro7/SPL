using MediatR;
using SPL.Artifact.Application.Commands.Artifactdesign;
using SPL.Artifact.Application.Common;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.Artifactdesign
{
    public class SaveResistDesignHandler : IRequestHandler<SaveResistDesignCommand, ApiResponse<long>>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public SaveResistDesignHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveResistDesignCommand request, CancellationToken cancellationToken)
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

       
            foreach (var item in request.Data)
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

                if (item.Resistencia==0)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Resistencia es un campo requerido",
                        Structure = -1
                    };
                }
            }

            var result = await _infrastructure.SaveResistDesign(request.Data);

            return new ApiResponse<long>()
            {
                Code = result <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                Description = result <= 0 ? "Hubo un error guardando los datos" : "Guardado con éxito",
                Structure = result
            };
        }

        #endregion
    }
}

