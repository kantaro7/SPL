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
  

    public class GetStabilizationDesignHandler : IRequestHandler<GetStabilizationDesignQuery, ApiResponse<StabilizationDesignData>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetStabilizationDesignHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<StabilizationDesignData>> Handle(GetStabilizationDesignQuery request, CancellationToken cancellationToken)
        {
            try
            {


                StabilizationDesignData result = await this._infrastructure.GetStabilizationDesignData(request.NroSerie);

                return new ApiResponse<StabilizationDesignData>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No existe información cargada para el aparato, tipo de información y fecha de datos" : result == null  ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<StabilizationDesignData>()
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
