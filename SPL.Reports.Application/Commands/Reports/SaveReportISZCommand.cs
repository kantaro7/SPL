namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.ISZ;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportISZCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportISZCommand(ISZTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public ISZTestsGeneral Data { get; }
        #endregion
    }
}
