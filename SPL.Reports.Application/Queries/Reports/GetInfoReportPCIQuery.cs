using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Reports.PCI;

namespace SPL.Reports.Application.Queries.Reports
{
    public class GetInfoReportPCIQuery : IRequest<ApiResponse<PCITestGeneral>>
    {
        public GetInfoReportPCIQuery(string pNroSerie, string pKeyTest, bool pResult)
        {
            this.NroSerie = pNroSerie;
            this.KeyTest = pKeyTest;
            this.Result = pResult;
        }

        #region Constructor

        public string NroSerie { get; }

        public string KeyTest { get; }

        public bool Result { get; }

        #endregion
    }
}