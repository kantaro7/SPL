namespace SPL.Security.Application.Queries.Security
{
    using MediatR;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Security;

    using System.Collections.Generic;

    public class GetPermissionsProfileQuery : IRequest<ApiResponse<List<UserPermissions>>>
    {
        public GetPermissionsProfileQuery(string pProfile)
        {

            this.Profile = pProfile;


        }
        #region Constructor


        public string Profile { get; }

        #endregion

    }
}
