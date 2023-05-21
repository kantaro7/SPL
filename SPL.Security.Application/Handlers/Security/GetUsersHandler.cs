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

    public class GetUsersHandler : IRequestHandler<GetUsersQuery, ApiResponse<List<Users>>>
    {

        private readonly ISecurityInfrastructure _infrastructure;

        public GetUsersHandler(ISecurityInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<Users>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {

                List<Users> result = await this._infrastructure.GetUsers(request.Name);

                return new ApiResponse<List<Users>>()
                {
                    Code = result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Users>>()
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
