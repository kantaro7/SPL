namespace SPL.Reports.Application.Queries.Reports
{
    using MediatR;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.RCT;

    public class GetInfoReportRCTQuery : IRequest<ApiResponse<RCTTestsGeneral>>
    {
        public GetInfoReportRCTQuery(string pNroSerie, string pKeyTest, bool pResult)
        {
         
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Result = pResult;


        }
        #region Constructor

     
        public string NroSerie { get; }
        public string KeyTests { get; }
        public bool Result { get; }

        #endregion

    }
}
