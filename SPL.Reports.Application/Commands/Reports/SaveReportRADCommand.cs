namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;

    public class SaveReportRADCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportRADCommand(RADReport pData) => this.Data = pData;
        #region Constructor
        public RADReport Data { get; }
        #endregion
    }
}
