namespace SPL.Masters.Application.Handlers.Artifactdesign
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain.SPL.Masters;
    using SPL.Masters.Application.Queries.Artifactdesign;

    public class GetCatSidcoHandler : IRequestHandler<GetCatSidcoQuery, List<CatSidcoInformation>>
    {

        private readonly IMastersInfrastructure _infrastructure;

        public GetCatSidcoHandler(IMastersInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<List<CatSidcoInformation>> Handle(GetCatSidcoQuery request, CancellationToken cancellationToken) => await this._infrastructure.GetCatSidcoInformation();
        #endregion
    }
}
