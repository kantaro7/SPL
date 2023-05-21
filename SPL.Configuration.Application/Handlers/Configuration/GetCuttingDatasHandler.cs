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
  

    public class GetCuttingDatasHandler : IRequestHandler<GetCuttingDatasQuery, ApiResponse<List<HeaderCuttingData>>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetCuttingDatasHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<HeaderCuttingData>>> Handle(GetCuttingDatasQuery request, CancellationToken cancellationToken)
        {
            try
            {


                List<HeaderCuttingData> result = await this._infrastructure.GetCuttingDatas(request.NroSerie);

                return new ApiResponse<List<HeaderCuttingData>>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : result == null  ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No se encontraron resultados"  : "Datos obtenidos con exito",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<HeaderCuttingData>>()
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
