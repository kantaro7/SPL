namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PEE;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportPEECommand : IRequest<ApiResponse<long>>
    {
        public SaveReportPEECommand(PEETestsGeneral pData) => this.Data = pData;
        #region Constructor
        public PEETestsGeneral Data { get; }
        #endregion
    }
}
