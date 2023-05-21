namespace SPL.Configuration.Application.Queries.Configuration
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetNozzleTypesByBrandQuery : IRequest<ApiResponse<List<TypesNozzleMarks>>>
    {
        public GetNozzleTypesByBrandQuery(long pIdMark)
        {
            this.IdMark = pIdMark;
      

        }
        #region Constructor
        public long IdMark { get; }

        #endregion
    }
}
