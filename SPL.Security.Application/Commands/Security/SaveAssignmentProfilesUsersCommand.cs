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

    public class SaveAssignmentProfilesUsersCommand : IRequest<ApiResponse<long>>
    {
        public SaveAssignmentProfilesUsersCommand(AssignmentUsers pData) => this.Data = pData;
        #region Constructor
        public AssignmentUsers Data { get; }
        #endregion
    }
}
