namespace SPL.Reports.Application.Queries.Reports
{
    using MediatR;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports.FPC;

    public class GetInfoReportFPCQuery : IRequest<ApiResponse<FPCTestsGeneral>>
    {
        public GetInfoReportFPCQuery(string pNroSerie, string pKeyTest, string pLenguage, string pUnitType, decimal pFrecuency, bool pResult)
        {
         
            this.NroSerie = pNroSerie;
            this.KeyTest = pKeyTest;
            this.Lenguage = pLenguage;
            this.UnitType = pUnitType;
            this.Frecuency = pFrecuency;
            this.Result = pResult;

        }
        #region Constructor

     
        public string NroSerie { get; }
        public string KeyTest { get; }
        public string Lenguage { get; }
        public string UnitType { get; }
        public decimal Frecuency { get; }
        public bool Result { get; }
        #endregion

    }
}
