namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextISZQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextISZQuery(string pNroSerie, string pKeyTest, string pLenguage,
            decimal pDegreesCor, string pPosAT, string pPosBT, string pPosTER, string pWindingEnergized, int pQTYNeutral, decimal pImpedanceGar, string pMaterialWinding)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.DegreesCor = pDegreesCor;
            this.PosAT = pPosAT;
            this.PosBT = pPosBT;
            this.PosTER = pPosTER;
            this.WindingEnergized = pWindingEnergized;
            this.QTYNeutral = pQTYNeutral;
            this.ImpedanceGar = pImpedanceGar;
            this.MaterialWinding = pMaterialWinding;

      
   
 

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public decimal DegreesCor { get; }
        public string PosAT { get; }
        public string PosBT { get; }
        public string PosTER { get; }
        public string WindingEnergized { get; }
        public int QTYNeutral { get; }
        public decimal ImpedanceGar { get; }
        public string MaterialWinding { get; }

   

   

        #endregion
    }
}
