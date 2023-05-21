namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportPCECommand : IRequest<ApiResponse<long>>
    {
        public SaveReportPCECommand(PCETestsGeneral pData) => this.Data = pData;
        #region Constructor
        public PCETestsGeneral Data { get; }
        #endregion
    }
}
