namespace SPL.Masters.Application.Queries.Artifactdesign
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain.SPL.Masters;

    public class GetEquivalentsAngularDisplacementQuery : IRequest<List<GeneralProperties>>
    {
        public GetEquivalentsAngularDisplacementQuery()
        {
        }
    }
}
