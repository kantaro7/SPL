namespace SPL.Configuration.Application.Queries.Configuration
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetNozzleMarksQuery : IRequest<ApiResponse<List<NozzleMarks>>>
    {
        public GetNozzleMarksQuery(long pIdMark, bool pStatus)
        {
            this.IdMark = pIdMark;
            this.Status = pStatus;
  
        }
        #region Constructor
        public long IdMark { get; }
        public bool Status { get; }

        #endregion
    }
}
