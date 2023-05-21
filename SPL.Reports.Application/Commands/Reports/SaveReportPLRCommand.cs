namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportPLRCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportPLRCommand(PLRTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public PLRTestsGeneral Data { get; }
        #endregion
    }
}
