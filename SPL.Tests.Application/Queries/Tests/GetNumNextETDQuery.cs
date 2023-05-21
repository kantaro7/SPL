namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextETDQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextETDQuery(string pNroSerie, string pKeyTest, string pLanguage, bool pTypeRegression, bool pBtDifCap, decimal pCapacityBt, string pTertiary2B, bool pTerCapRed, decimal pCapacityTer, string pWindingSplit)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Language = pLanguage;
            this.TypeRegression = pTypeRegression;
            this.BtDifCap = pBtDifCap;
            this.CapacityBt = pCapacityBt;
            this.Tertiary2B = pTertiary2B;
            this.TerCapRed = pTerCapRed;
            this.CapacityTer = pCapacityTer;
            this.WindingSplit = pWindingSplit;
  

        }
        #region Constructor

        public string NroSerie { get; } 
        public string KeyTests { get; }
        public string Language { get; }
        public bool TypeRegression { get; }
        public bool BtDifCap { get; }
        public decimal CapacityBt { get; }
        public string Tertiary2B { get; }
        public bool TerCapRed { get; }
        public decimal CapacityTer { get; }
        public string WindingSplit { get; }
     
     
    
        #endregion
    }
}
