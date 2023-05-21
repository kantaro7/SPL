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
  

    public class GetStabilizationHandler : IRequestHandler<GetStabilizationQuery, ApiResponse<List<StabilizationData>>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetStabilizationHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<StabilizationData>>> Handle(GetStabilizationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<StabilizationData> result = await this._infrastructure.GetStabilizationData(request.NroSerie,request.Status, request.Stabilized);

                return new ApiResponse<List<StabilizationData>>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No existe información cargada para las concidencias establecidas" : "Datos obtenidos de forma exitosa",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<StabilizationData>>()
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
