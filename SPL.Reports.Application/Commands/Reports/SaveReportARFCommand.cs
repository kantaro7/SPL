namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;

    using System.Collections.Generic;

    public class SaveReportARFCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportARFCommand(ARFTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public ARFTestsGeneral Data { get; }
        #endregion
    }
}
