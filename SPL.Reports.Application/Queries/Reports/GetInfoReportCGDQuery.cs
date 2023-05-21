namespace SPL.Reports.Application.Queries.Reports
{
    using MediatR;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPC;

    public class GetInfoReportCGDQuery : IRequest<ApiResponse<CGDTestsGeneral>>
    {
        public GetInfoReportCGDQuery(string pNroSerie, string pKeyTests, bool pResult)
        {
         
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTests;
            this.Result = pResult;


        }
        #region Constructor

     
        public string NroSerie { get; }
        public string KeyTests { get; }
        public bool Result { get; }

        #endregion

    }
}
