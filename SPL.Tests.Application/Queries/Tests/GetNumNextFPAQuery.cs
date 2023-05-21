namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextFPAQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextFPAQuery(string pNroSerie, string pKeyTest, string pLenguage, string pTypeOil)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.TypeOil = pTypeOil;
       
  

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string TypeOil { get; }
  
    
        #endregion
    }
}
