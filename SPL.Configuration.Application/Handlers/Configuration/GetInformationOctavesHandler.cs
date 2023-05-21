namespace SPL.Configuration.Application.Handlers.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using SPL.Configuration.Application.Queries.Configuration;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Masters;
  

    public class GetInformationOctavesHandler : IRequestHandler<GetInformationOctavesQuery, ApiResponse<List<InformationOctaves>>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetInformationOctavesHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<InformationOctaves>>> Handle(GetInformationOctavesQuery request, CancellationToken cancellationToken)
        {
            try
            {
              

                List<InformationOctaves> result = await this._infrastructure.GetInformationOctaves(request.NroSerie, request.TypeInformation, request.DateData);

                return new ApiResponse<List<InformationOctaves>>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No existe información cargada para el aparato, tipo de información y fecha de datos" : result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (DbUpdateException e)
            {
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar la obtención " + e.Message + sb.ToString() + e.InnerException.Message);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<InformationOctaves>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message + ex.InnerException,
                    Structure = null
                };
            }
        }
        #endregion
    }
}
