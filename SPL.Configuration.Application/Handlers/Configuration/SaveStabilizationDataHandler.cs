namespace SPL.Configuration.Application.Handlers.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Configuration.Application.Commands.Configuration;
    using SPL.Configuration.Application.Queries.Configuration;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Masters;
  

    public class SaveStabilizationDataHandler : IRequestHandler<SaveStabilizationDataCommand, ApiResponse<long>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public SaveStabilizationDataHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveStabilizationDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
             

                var result = await _infrastructure.saveStabilizationData(request.Data);

                return new ApiResponse<long>()
                {
                    Code = result <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result <= 0 ? "Hubo un error guardando los datos" : "Guardado con éxito",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<long>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = -1
                };
            }
        }
        #endregion
    }
}
