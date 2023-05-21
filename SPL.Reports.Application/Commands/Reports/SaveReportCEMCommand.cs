namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.CEM;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.ROD;
    using SPL.Domain.SPL.Reports.TAP;
    using SPL.Domain.SPL.Reports.TIN;

    public class SaveReportCEMCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportCEMCommand(CEMTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public CEMTestsGeneral Data { get; }
        #endregion
    }
}
