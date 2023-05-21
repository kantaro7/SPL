namespace SPL.Configuration.Application.Handlers.Configuration
{
    using MediatR;

    using SPL.Configuration.Application.Commands.Configuration;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateInformationOctavesHandler : IRequestHandler<UpdateInformationOctavesCommand, ApiResponse<long>>
    {
        #region Fields

        private readonly IConfigurationInfrastructure _infrastructure;

        #endregion

        #region Constructor

        public UpdateInformationOctavesHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #endregion

        #region Methods

        public async Task<ApiResponse<long>> Handle(UpdateInformationOctavesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _infrastructure.UpdateInformationOctaves(request.Data);

                return new ApiResponse<long>()
                {
                    Code = result <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result <= 0 ? "Hubo un error actualizando los datos" : "Información actualizada con éxito",
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