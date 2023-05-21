namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.ISZ;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.ROD;
    using SPL.Domain.SPL.Reports.RYE;

    public class SaveReportRYECommand : IRequest<ApiResponse<long>>
    {
        public SaveReportRYECommand(RYETestsGeneral pData) => this.Data = pData;
        #region Constructor
        public RYETestsGeneral Data { get; }
        #endregion
    }
}
