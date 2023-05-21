namespace SPL.Masters.Application.Handlers.Artifactdesign
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain.SPL.Masters;
    using SPL.Masters.Application.Queries.Artifactdesign;

    public class GetCatSidcoOtherHandler : IRequestHandler<GetCatSidcoOtherQuery, List<CatSidcoOtherInformation>>
    {

        private readonly IMastersInfrastructure _infrastructure;

        public GetCatSidcoOtherHandler(IMastersInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<List<CatSidcoOtherInformation>> Handle(GetCatSidcoOtherQuery request, CancellationToken cancellationToken) => await this._infrastructure.GetCatSidcoOtherInformation();
        #endregion
    }
}
