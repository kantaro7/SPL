namespace SPL.Configuration.Application.Queries.Configuration
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetInfoContGasCGDQuery : IRequest<ApiResponse<List<ContGasCGD>>>
    {
        public GetInfoContGasCGDQuery(string pIdReport, string pKeyTests, string pTypeOil)
        {
            this.IdReport = pIdReport;
            this.KeyTests = pKeyTests;
            this.TypeOil = pTypeOil;

        }
        #region Constructor
        public string IdReport { get; }
        public string KeyTests { get; }
        public string TypeOil { get; }

        #endregion
    }
}
