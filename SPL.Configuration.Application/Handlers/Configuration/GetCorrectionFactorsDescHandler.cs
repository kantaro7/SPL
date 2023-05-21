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
  

    public class GetCorrectionFactorsDescHandler : IRequestHandler<GetCorrectionFactorsDescQuery, ApiResponse<CorrectionFactorsDesc>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetCorrectionFactorsDescHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<CorrectionFactorsDesc>> Handle(GetCorrectionFactorsDescQuery request, CancellationToken cancellationToken)
        {
            try
            {
                CorrectionFactorsDesc result = await this._infrastructure.GetCorrectionFactorsDesc(request.Specification, request.KeyLenguage);

                return new ApiResponse<CorrectionFactorsDesc>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : result != null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No se encontraron resultados" : "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CorrectionFactorsDesc>()
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
