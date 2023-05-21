namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.NRA;

    using System.Collections.Generic;

    public class SaveReportNRACommand : IRequest<ApiResponse<long>>
    {
        public SaveReportNRACommand(NRATestsGeneral pData) => this.Data = pData;
        #region Constructor
        public NRATestsGeneral Data { get; }
        #endregion
    }
}
