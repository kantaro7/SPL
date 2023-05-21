namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextRCTQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextRCTQuery(string pNroSerie, string pKeyTest, string pLenguage, string pUnitOfMeasurement, string pTertiary, decimal pTestvoltage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.UnitOfMeasurement = pUnitOfMeasurement;
            this.Tertiary = pTertiary;
            this.Testvoltage = pTestvoltage;

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string UnitOfMeasurement { get; }
        public string Tertiary { get; }
        public decimal Testvoltage { get; }
   

        #endregion
    }
}
