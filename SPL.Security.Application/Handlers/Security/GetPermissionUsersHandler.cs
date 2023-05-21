﻿namespace SPL.Security.Application.Handlers.Security
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

    public class GetPermissionUsersHandler : IRequestHandler<GetPermissionUsersQuery, ApiResponse<List<UserPermissions>>>
    {

        private readonly ISecurityInfrastructure _infrastructure;

        public GetPermissionUsersHandler(ISecurityInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<UserPermissions>>> Handle(GetPermissionUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {

                List<UserPermissions> result = await this._infrastructure.GetPermissionUsers(request.IdUser);

                return new ApiResponse<List<UserPermissions>>()
                {
                    Code = result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<UserPermissions>>()
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
