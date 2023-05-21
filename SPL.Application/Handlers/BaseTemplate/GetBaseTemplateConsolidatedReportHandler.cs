using MediatR;
using SPL.Artifact.Application.Queries.BaseTemplate;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.BaseTemplate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.BaseTemplate
{
    public class GetBaseTemplateConsolidatedReportHandler : IRequestHandler<GetBaseTemplateConsolidatedReportQuery, ApiResponse<BaseTemplateConsolidatedReport>>
    {

        private readonly IBaseTemplateInfrastructure _infrastructure;

        public GetBaseTemplateConsolidatedReportHandler(IBaseTemplateInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplateConsolidatedReport>> Handle(GetBaseTemplateConsolidatedReportQuery request, CancellationToken cancellationToken)
        {

            try
            {
                if (string.IsNullOrEmpty(request.KeyLanguage.Trim()))
                {
                    return new ApiResponse<BaseTemplateConsolidatedReport>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave idioma es requerido",
                        Structure = null
                    };
                }

                if (request.KeyLanguage.Trim().Length > 2)
                {
                    return new ApiResponse<BaseTemplateConsolidatedReport>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave idioma no puede ser mayor a 2 caracteres",
                        Structure = null
                    };
                }


                var result = await _infrastructure.GetBaseTemplateConsolidatedReport( request.KeyLanguage);

                return new ApiResponse<BaseTemplateConsolidatedReport>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "",
                    Structure = result
                };

            }
            catch (Exception ex)
            {

                return new ApiResponse<BaseTemplateConsolidatedReport>()
                {
                    Code = (int)(ResponsesID.fallido),
                    Description = ex.Message,
                    Structure = null
                };

            }
        }
    }

        #endregion
    
}

