namespace SPL.Masters.Application.Handlers.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Masters;
    using SPL.Masters.Application.Queries.Configuration;

    public class GetConfigurationFilesHandler : IRequestHandler<GetConfigurationFilesQuery, ApiResponse<List<FileWeight>>>
    {

        private readonly IMastersInfrastructure _infrastructure;

        public GetConfigurationFilesHandler(IMastersInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<FileWeight>>> Handle(GetConfigurationFilesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Module < 0)
                {
                    return new ApiResponse<List<FileWeight>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El módulo es requerido",
                        Structure = null
                    };
                }

                List<FileWeight> result = await this._infrastructure.GetConfigurationFiles(request.Module);

                return new ApiResponse<List<FileWeight>>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No se encontraron resultados" : result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<FileWeight>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }
        #endregion
    }
}
