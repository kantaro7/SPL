namespace SPL.Masters.Application.Handlers.Artifactdesign
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain.SPL.Masters;
    using SPL.Masters.Application.Queries.Artifactdesign;

    public class GetRulesRepHandler : IRequestHandler<GetRulesRepQuery, List<RulesRep>>
    {

        private readonly IMastersInfrastructure _infrastructure;

        public GetRulesRepHandler(IMastersInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<List<RulesRep>> Handle(GetRulesRepQuery request, CancellationToken cancellationToken) => await this._infrastructure.GetRulesRep(request.Idioma, request.Norma);
        #endregion
    }
}
