namespace SPL.Configuration.Application.Handlers.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Configuration.Application.Queries.Configuration;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Masters;
  

    public class GetCorrectionFactorkWTypeCoolingHandler : IRequestHandler<GetCorrectionFactorkWTypeCoolingQuery, ApiResponse<List<CorrectionFactorkWTypeCooling>>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetCorrectionFactorkWTypeCoolingHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<CorrectionFactorkWTypeCooling>>> Handle(GetCorrectionFactorkWTypeCoolingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<CorrectionFactorkWTypeCooling> result = await this._infrastructure.GetCorrectionFactorkWTypeCooling();

                return new ApiResponse<List<CorrectionFactorkWTypeCooling>>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CorrectionFactorkWTypeCooling>>()
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
