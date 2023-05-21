namespace SPL.Masters.Application.Handlers.Artifactdesign
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain.SPL.Masters;
    using SPL.Masters.Application.Queries.Artifactdesign;

    public class GetEquivalentsAngularDisplacementHandler : IRequestHandler<GetEquivalentsAngularDisplacementQuery, List<GeneralProperties>>
    {

        private readonly IMastersInfrastructure _infrastructure;

        public GetEquivalentsAngularDisplacementHandler(IMastersInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<List<GeneralProperties>> Handle(GetEquivalentsAngularDisplacementQuery request, CancellationToken cancellationToken) => await this._infrastructure.GetEquivalentsAngularDisplacement();
        #endregion
    }
}
