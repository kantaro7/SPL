namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.ROD;
    using SPL.Domain.SPL.Reports.TIN;

    public class SaveReportTINCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportTINCommand(TINTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public TINTestsGeneral Data { get; }
        #endregion
    }
}
