namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PIM;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportPIMCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportPIMCommand(PIMTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public PIMTestsGeneral Data { get; }
        #endregion
    }
}
