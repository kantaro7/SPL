namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextCEMQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextCEMQuery(string pNroSerie, string pKeyTest, string pLenguage,
       string pIdPosPrimary, string pPosPrimary, string pIdPosSecundary, string pPosSecundary, decimal pTestsVoltage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.IdPosPrimary = pIdPosPrimary;
            this.PosPrimary = pPosPrimary;
            this.IdPosSecundary = pIdPosSecundary;
            this.PosSecundary = pPosSecundary;
            this.TestsVoltage = pTestsVoltage;

                

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string IdPosPrimary { get; }
        public string PosPrimary { get; }
        public string IdPosSecundary { get; }
        public string PosSecundary { get; }
        public decimal TestsVoltage { get; }



        #endregion
    }
}
