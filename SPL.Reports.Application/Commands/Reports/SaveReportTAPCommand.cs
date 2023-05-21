namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.ROD;
    using SPL.Domain.SPL.Reports.TAP;
    using SPL.Domain.SPL.Reports.TIN;

    public class SaveReportTAPCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportTAPCommand(TAPTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public TAPTestsGeneral Data { get; }
        #endregion
    }
}
