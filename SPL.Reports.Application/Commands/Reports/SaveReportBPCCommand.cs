namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.BPC;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;

    using System.Collections.Generic;

    public class SaveReportBPCCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportBPCCommand(BPCTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public BPCTestsGeneral Data { get; }
        #endregion
    }
}
