namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;

    public class SaveReportRDTCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportRDTCommand(RDTReport pData) => this.Data = pData;
        #region Constructor
        public RDTReport Data { get; }
        #endregion
    }
}
