namespace SPL.Masters.Application.Queries.Artifactdesign
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain.SPL.Masters;

    public class GetCatSidcoOtherQuery : IRequest<List<CatSidcoOtherInformation>>
    {
        public GetCatSidcoOtherQuery()
        {
        }
    }
}
