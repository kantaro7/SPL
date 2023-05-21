namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.CEM;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.ROD;
    using SPL.Domain.SPL.Reports.TAP;
    using SPL.Domain.SPL.Reports.TDP;
    using SPL.Domain.SPL.Reports.TIN;

    public class SaveReportTDPCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportTDPCommand(TDPTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public TDPTestsGeneral Data { get; }
        #endregion
    }
}
