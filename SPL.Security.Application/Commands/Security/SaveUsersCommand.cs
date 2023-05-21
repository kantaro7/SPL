namespace SPL.Security.Application.Commands.Security
{
    using MediatR;

    using SPL.Domain;

    using SPL.Domain.SPL.Security;

    using System.Collections.Generic;

    public class SaveUsersCommand : IRequest<ApiResponse<long>>
    {
        public SaveUsersCommand(List<Users> pData) => this.Data = pData;
        #region Constructor
        public List<Users>  Data { get; }
        #endregion
    }
}
