namespace SPL.Security.Application.Commands.Security
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Security;

    using System.Collections.Generic;

    public class DeleteProfilesCommand : IRequest<ApiResponse<long>>
    {
        public DeleteProfilesCommand(UserProfiles pData) => this.Data = pData;
        #region Constructor
        public UserProfiles Data { get; }
        #endregion
    }
}
