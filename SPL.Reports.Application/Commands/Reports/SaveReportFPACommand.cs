namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPA;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;

    using System.Collections.Generic;

    public class SaveReportFPACommand : IRequest<ApiResponse<long>>
    {
        public SaveReportFPACommand(FPATestsGeneral pData) => this.Data = pData;
        #region Constructor
        public FPATestsGeneral Data { get; }
        #endregion
    }
}
