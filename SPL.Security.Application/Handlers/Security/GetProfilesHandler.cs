namespace SPL.Security.Application.Handlers.Security
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Security;
    using SPL.Security.Application.Queries.Security;

    public class GetProfilesHandler : IRequestHandler<GetProfilesQuery, ApiResponse<List<UserProfiles>>>
    {

        private readonly ISecurityInfrastructure _infrastructure;

        public GetProfilesHandler(ISecurityInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<UserProfiles>>> Handle(GetProfilesQuery request, CancellationToken cancellationToken)
        {
            try
            {

                List<UserProfiles> result = await this._infrastructure.GetProfiles(request.Key);

                return new ApiResponse<List<UserProfiles>>()
                {
                    Code = result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<UserProfiles>>()
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
