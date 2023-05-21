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
  

    public class GetInfoHeaderCuttingDataHandler : IRequestHandler<GetInfoHeaderCuttingDataQuery, ApiResponse<HeaderCuttingData>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetInfoHeaderCuttingDataHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<HeaderCuttingData>> Handle(GetInfoHeaderCuttingDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                HeaderCuttingData result = await this._infrastructure.GetInfoHeaderCuttingData(request.IdCorte);

                return new ApiResponse<HeaderCuttingData>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : result == null  ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No se encontraron resultados"  : "Datos obtenidos con exito",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HeaderCuttingData>()
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
