namespace SPL.Security.Application.Queries.Security
{
    using MediatR;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Security;

    using System.Collections.Generic;

    public class GetProfilesQuery : IRequest<ApiResponse<List<UserProfiles>>>
    {
        public GetProfilesQuery(string pKey)
        {
         
            this.Key = pKey;


        }
        #region Constructor

     
        public string Key { get; }
     
        #endregion

    }
}
