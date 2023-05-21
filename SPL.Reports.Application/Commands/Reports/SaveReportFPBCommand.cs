namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;

    using System.Collections.Generic;

    public class SaveReportFPBCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportFPBCommand(FPBTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public FPBTestsGeneral Data { get; }
        #endregion
    }
}
