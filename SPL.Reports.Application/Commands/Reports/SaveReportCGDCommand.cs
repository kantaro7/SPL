namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;

    using System.Collections.Generic;

    public class SaveReportCGDCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportCGDCommand(CGDTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public CGDTestsGeneral Data { get; }
        #endregion
    }
}
