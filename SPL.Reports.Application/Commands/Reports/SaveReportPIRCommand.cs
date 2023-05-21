namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PIM;
    using SPL.Domain.SPL.Reports.PIR;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportPIRCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportPIRCommand(PIRTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public PIRTestsGeneral Data { get; }
        #endregion
    }
}
