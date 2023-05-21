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
  

    public class GetNozzleTypesByBrandHandler : IRequestHandler<GetNozzleTypesByBrandQuery, ApiResponse<List<TypesNozzleMarks>>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetNozzleTypesByBrandHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<TypesNozzleMarks>>> Handle(GetNozzleTypesByBrandQuery request, CancellationToken cancellationToken)
        {
            try
            {
              

                List<TypesNozzleMarks> result = await this._infrastructure.GetNozzleTypesByBrand(request.IdMark);

                return new ApiResponse<List<TypesNozzleMarks>>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No se encontraron resultados" : result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TypesNozzleMarks>>()
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
