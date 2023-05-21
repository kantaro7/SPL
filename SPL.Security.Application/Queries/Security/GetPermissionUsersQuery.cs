namespace SPL.Security.Application.Queries.Security
{
    using MediatR;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Security;

    using System.Collections.Generic;

    public class GetPermissionUsersQuery : IRequest<ApiResponse<List<UserPermissions>>>
    {
        public GetPermissionUsersQuery(string pIdUser)
        {
         
            this.IdUser = pIdUser;


        }
        #region Constructor

     
        public string IdUser { get; }
     
        #endregion

    }
}
