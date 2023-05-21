namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextPIRQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextPIRQuery(string pNroSerie, string pKeyTest, string pLenguage,
          string pConnection, string IncludesTertiary, string VoltageLevel)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.Connection = pConnection;
            this.IncludesTertiary = IncludesTertiary;
            this.VoltageLevel = VoltageLevel;


      
   
 

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string Connection { get; }
        public string IncludesTertiary { get; }
        public string VoltageLevel { get; }


   

   

        #endregion
    }
}
