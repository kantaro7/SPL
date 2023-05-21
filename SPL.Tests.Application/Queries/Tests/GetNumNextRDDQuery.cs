namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextRDDQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextRDDQuery(string pNroSerie, string pKeyTest, string pLenguage,
string pConfig_Winding, string pConnection, decimal pPorc_Z, decimal pPorc_Jx)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.Config_Winding = pConfig_Winding;
            this.Connection = pConnection;
            this.Porc_Z = pPorc_Z;
            this.Porc_Jx = pPorc_Jx;
  

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string Config_Winding { get; }
        public string Connection { get; }
        public decimal Porc_Z { get; }
        public decimal Porc_Jx { get; }
    
        #endregion
    }
}
