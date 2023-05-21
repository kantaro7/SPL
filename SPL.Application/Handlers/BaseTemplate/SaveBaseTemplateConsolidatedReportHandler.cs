using MediatR;

using SPL.Artifact.Application.Commands.Artifactdesign;
using SPL.Artifact.Application.Commands.BaseTemplate;
using SPL.Artifact.Application.Commands.PlateTension;
using SPL.Artifact.Application.Common;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Artifact.BaseTemplate;
using SPL.Domain.SPL.Artifact.PlateTension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.BaseTemplate
{
    public class SaveBaseTemplateConsolidatedReportHandler : IRequestHandler<SaveBaseTemplateConsolidatedReportCommand, ApiResponse<long>>
    {

        private readonly IBaseTemplateInfrastructure _infrastructure;

        public SaveBaseTemplateConsolidatedReportHandler(IBaseTemplateInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveBaseTemplateConsolidatedReportCommand request, CancellationToken cancellationToken)
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

                if (string.IsNullOrEmpty(request.Data.ClaveIdioma.Trim()))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave idioma es requerido",
                        Structure = -1
                    };
                }

                if (request.Data.ClaveIdioma.Trim().Length > 2)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave idioma no puede ser mayor a 2 caracteres",
                        Structure = -1
                    };
                }

                var result = await _infrastructure.saveBaseTemplateConsolidatedReport(request.Data);

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

