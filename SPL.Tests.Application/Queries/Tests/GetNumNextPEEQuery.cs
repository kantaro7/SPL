namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextPEEQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextPEEQuery(string pNroSerie, string pKeyTest, string pLenguage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
      
   
 

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }

   

   

        #endregion
    }
}
