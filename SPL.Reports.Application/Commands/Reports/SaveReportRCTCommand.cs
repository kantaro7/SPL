namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.RCT;

    using System.Collections.Generic;

    public class SaveReportRCTCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportRCTCommand(RCTTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public RCTTestsGeneral Data { get; }
        #endregion
    }
}
