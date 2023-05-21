namespace SPL.Masters.Application.Handlers.Artifactdesign
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Masters;
    using SPL.Masters.Application.Queries.Artifactdesign;

    public class GetThirdWindingHandler : IRequestHandler<GetThirdWindingQuery, ApiResponse<List<GeneralProperties>>>
    {

        private readonly IMastersInfrastructure _infrastructure;

        public GetThirdWindingHandler(IMastersInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<GeneralProperties>>> Handle(GetThirdWindingQuery request, CancellationToken cancellationToken)
        {
            List<GeneralProperties> result = await this._infrastructure.GetThirdWinding();

            return new ApiResponse<List<GeneralProperties>>()
            {
                Code = result is null ? (int)ResponsesID.fallido : result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                Description = result is null ? "No se encontraron resultados" : result.Count <= 0 ? "No se encontraron resultados" : "",
                Structure = result
            };
        }
        #endregion
    }
}
