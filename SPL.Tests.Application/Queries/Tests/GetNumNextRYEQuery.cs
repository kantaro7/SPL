namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextRYEQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextRYEQuery(string pNroSerie, string pKeyTest, string pLenguage,
            int pNoConnectionsAT, int pNoConnectionsBT, int pNoConnectionsTER, decimal pVoltageAT, decimal pVoltageBT, decimal pVoltageTER, string pCoolingType)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.NoConnectionsAT = pNoConnectionsAT;
            this.NoConnectionsBT = pNoConnectionsBT;
            this.NoConnectionsTER = pNoConnectionsTER;
            this.VoltageAT = pVoltageAT;
            this.VoltageBT = pVoltageBT;
            this.VoltageTER = pVoltageTER;
            this.CoolingType = pCoolingType;
      
   
 

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public int NoConnectionsAT { get; }
        public int NoConnectionsBT { get; }
        public int NoConnectionsTER { get; }
        public decimal VoltageAT { get; }
        public decimal VoltageBT { get; }
        public decimal VoltageTER { get; }
        public string CoolingType { get; }

   

   

        #endregion
    }
}
