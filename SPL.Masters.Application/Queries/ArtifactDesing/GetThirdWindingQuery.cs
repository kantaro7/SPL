namespace SPL.Masters.Application.Queries.Artifactdesign
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Masters;

    public class GetThirdWindingQuery : IRequest<ApiResponse<List<GeneralProperties>>>
    {
        public GetThirdWindingQuery()
        {
        }
    }
}
