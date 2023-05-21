namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextINDQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextINDQuery(string pNroSerie, string pKeyTest, string pLenguage,
string pTcPurchased)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.TcPurchased = pTcPurchased;
       
  

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string TcPurchased { get; }
  
    
        #endregion
    }
}
