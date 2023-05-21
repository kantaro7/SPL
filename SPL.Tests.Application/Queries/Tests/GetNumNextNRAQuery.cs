namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextNRAQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextNRAQuery(string pNroSerie, string pKeyTest, string pLenguage, string pLaboratory, string pRule, string pFeeding, string pCoolingType)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Language = pLenguage;
            this.Laboratory = pLaboratory;
            this.Feeding = pFeeding;
            this.Rule = pRule;
            this.Feeding = pFeeding;
            this.CoolingType = pCoolingType;
  

        }
        #region Constructor

        public string NroSerie { get; } 
        public string KeyTests { get; }
        public string Language { get; }
        public string Laboratory { get; }
        public string Rule { get; }
        public string Feeding { get; }
        public string CoolingType { get; }
     
    
        #endregion
    }
}
