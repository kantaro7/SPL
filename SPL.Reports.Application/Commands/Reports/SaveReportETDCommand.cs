namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.ETD;

    public class SaveReportETDCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportETDCommand(ETDReport pData) => Data = pData;
        #region Constructor
        public ETDReport Data { get; }
        #endregion
    }
}
