namespace SPL.Security.Application.Queries.Security
{
    using MediatR;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Security;

    using System.Collections.Generic;

    public class GetUsersQuery : IRequest<ApiResponse<List<Users>>>
    {
        public GetUsersQuery(string pName)
        {
         
            this.Name = pName;


        }
        #region Constructor

     
        public string Name { get; }
     
        #endregion

    }
}
