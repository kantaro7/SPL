namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextRDTQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextRDTQuery(string pNroSerie, string pKeyTest, string pDAngular, string pRule, string pLenguage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.DAngular = pDAngular;
            this.Rule = pRule;
            this.Lenguage = pLenguage;

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string DAngular { get; }
        public string Rule { get; }
        public string Lenguage { get; }
        #endregion
    }
}
