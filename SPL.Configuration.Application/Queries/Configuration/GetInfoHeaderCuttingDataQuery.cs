namespace SPL.Configuration.Application.Queries.Configuration
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetInfoHeaderCuttingDataQuery : IRequest<ApiResponse<HeaderCuttingData>>
    {
        public GetInfoHeaderCuttingDataQuery(decimal pIdCorte) => IdCorte = pIdCorte;
        #region Constructor
        public decimal IdCorte { get; }


        #endregion
    }
}
