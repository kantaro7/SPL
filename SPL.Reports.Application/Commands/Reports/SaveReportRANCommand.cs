namespace SPL.Artifact.Application.Commands.Reports
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;

    using System.Collections.Generic;

    public class SaveReportRANCommand : IRequest<ApiResponse<long>>
    {
        public SaveReportRANCommand(RANReport pData) => this.Data = pData;
        #region Constructor
        public RANReport Data { get; }
        #endregion
    }
}
