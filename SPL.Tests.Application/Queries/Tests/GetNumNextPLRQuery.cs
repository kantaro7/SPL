namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextPLRQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextPLRQuery(string pNroSerie, string pKeyTest, string pLenguage, decimal pRldnt, decimal pNominalVoltage, int pAmountOfTensions)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.Rldnt = pRldnt;
            this.NominalVoltage = pNominalVoltage;
            this.AmountOfTensions = pAmountOfTensions;
   
 

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public decimal Rldnt { get; }
        public decimal NominalVoltage { get; }
        public int AmountOfTensions { get; }
   

   

        #endregion
    }
}
