namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportRODCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportRODCommand(RODTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public RODTestsGeneral Data { get; }
        #endregion
    }
}
