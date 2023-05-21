namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextPCIQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextPCIQuery(string pNroSerie, string pKeyTest, string pLenguage, string pWindingMaterial, bool pCapRedBaja, bool pAutotransformer, bool pMonofasico, decimal pOverElevation, string pPosPi, string pPosSec)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.WindingMaterial = pWindingMaterial;
            this.CapRedBaja = pCapRedBaja;
            this.Autotransformer = pAutotransformer;
            this.Monofasico = pMonofasico;
            this.OverElevation = pOverElevation;
            this.PosPi = pPosPi;
            this.PosSec = pPosSec;
           

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }

        public string WindingMaterial { get; }
        public bool CapRedBaja { get; }
        public bool Autotransformer { get; }
        public bool Monofasico { get; }
        public decimal OverElevation { get; }
        public string PosPi { get; }  
        public string PosSec { get; }
  
   

        #endregion
    }
}
