using MediatR;
using SPL.Artifact.Application.Queries.BaseTemplate;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.BaseTemplate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.BaseTemplate
{
    public class GetBaseTemplateHandler : IRequestHandler<GetBaseTemplateQuery, ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>>
    {

        private readonly IBaseTemplateInfrastructure _infrastructure;

        public GetBaseTemplateHandler(IBaseTemplateInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>> Handle(GetBaseTemplateQuery request, CancellationToken cancellationToken)
        {

            try
            {

               

                if (string.IsNullOrEmpty(request.TypeReport.Trim()))
                {
                    return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Tipo de reporte es requerido",
                        Structure = null
                    };
                }

                if (request.TypeReport.Trim().Length > 3)
                    {
                        return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El valor Tipo de reporte no puede ser mayor a 3 caracteres",
                            Structure = null
                        };
                    }

                if (string.IsNullOrEmpty(request.KeyTest.Trim()))
                {
                    return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave prueba es requerido",
                        Structure = null
                    };
                }

                if (request.KeyTest.Trim().Length > 3)
                {
                    return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave prueba no puede ser mayor a 3 caracteres",
                        Structure = null
                    };
                }

                if (string.IsNullOrEmpty(request.KeyLanguage.Trim()))
                {
                    return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave idioma es requerido",
                        Structure = null
                    };
                }

                if (request.KeyLanguage.Trim().Length > 2)
                {
                    return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Clave idioma no puede ser mayor a 2 caracteres",
                        Structure = null
                    };
                }

                if(!int.TryParse(request.NroColumns.ToString(), out _))
                {
                    return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Columnas configurables no debe poseer decimales",
                        Structure = null
                    };
                }

                if (request.NroColumns <= 0)
                {
                    return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Columnas configurables es requerido",
                        Structure = null
                    };
                }

                if (request.NroColumns > 99)
                {
                    return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor Columnas configurables no puede ser mayor a 2 caracteres",
                        Structure = null
                    };
                }

                var result = await _infrastructure.GetBaseTemplate(request.TypeReport, request.KeyTest, request.KeyLanguage, request.NroColumns);

                return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "",
                    Structure = result
                };

            }
            catch (Exception ex)
            {

                return new ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
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

