namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextPIMQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextPIMQuery(string pNroSerie, string pKeyTest, string pLenguage,
          string pConnection, string ApplyLow, string VoltageLevel)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.Connection = pConnection;
            this.ApplyLow = ApplyLow;
            this.VoltageLevel = VoltageLevel;
      

      
   
 

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string Connection { get; }
        public string ApplyLow { get; }
        public string VoltageLevel { get; }
 

   

   

        #endregion
    }
}
