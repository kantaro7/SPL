namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.RDD;

    using System.Collections.Generic;

    public class SaveReportRDDCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportRDDCommand(RDDTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public RDDTestsGeneral Data { get; }
        #endregion
    }
}
