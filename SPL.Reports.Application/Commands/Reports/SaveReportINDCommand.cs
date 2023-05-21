namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPA;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.IND;

    using System.Collections.Generic;

    public class SaveReportINDCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportINDCommand(INDTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public INDTestsGeneral Data { get; }
        #endregion
    }
}
