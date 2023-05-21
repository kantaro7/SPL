namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.FPC;

    using System.Collections.Generic;

    public class SaveReportFPCCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportFPCCommand(FPCTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public FPCTestsGeneral Data { get; }
        #endregion
    }
}
