namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.PRD;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportPRDCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportPRDCommand(PRDTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public PRDTestsGeneral Data { get; }
        #endregion
    }
}
