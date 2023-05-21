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

    public class GetAssignmentProfilesHandler : IRequestHandler<GetAssignmentProfilesQuery, ApiResponse<List<AssignmentUsers>>>
    {

        private readonly ISecurityInfrastructure _infrastructure;

        public GetAssignmentProfilesHandler(ISecurityInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<AssignmentUsers>>> Handle(GetAssignmentProfilesQuery request, CancellationToken cancellationToken)
        {
            try
            {

                List<AssignmentUsers> result = await this._infrastructure.GetAssignmentProfiles(request.Profile);

                return new ApiResponse<List<AssignmentUsers>>()
                {
                    Code = result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AssignmentUsers>>()
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
