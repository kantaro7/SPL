using MediatR;

using SPL.Artifact.Application.Common;
using SPL.Artifact.Application.Queries.Artifactdesign;
using SPL.Artifact.Application.Queries.PlateTension;
using SPL.Domain;

using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.PlateTension
{
    public class GetTapBaanHandler : IRequestHandler<GetTapBaanQuery, ApiResponse<TapBaan>>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetTapBaanHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<TapBaan>> Handle(GetTapBaanQuery request, CancellationToken cancellationToken)
        {

            try
            {
                string[] aparato = request.NroSerie.Split('-');

                if (string.IsNullOrEmpty(aparato[0].Trim()))
                {

                    return new ApiResponse<TapBaan>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie es requerido",
                        Structure = null
                    };
                }

                bool valLongitud = Validations.validacion55Caracteres(aparato[0].Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(aparato[0].Trim());

                if (valLongitud)
                {
                    return new ApiResponse<TapBaan>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie no puede excederse de 55 caracteres",
                        Structure = null
                    };
                }
                if (valFormat)
                {

                    return new ApiResponse<TapBaan>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie solo debe contener letras, números y guion medio",
                        Structure = null
                    };
                }

              

                TapBaan result = await _infrastructure.GetTapBaan(aparato[0]);

                return new ApiResponse<TapBaan>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "El aparato no cuenta con información de las tapas" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<TapBaan>()
                {
                    Code = (int)(ResponsesID.fallido),
                    Description = ex.Message,
                    Structure = null
                };

            }
        }

        
        #endregion
    }
}
