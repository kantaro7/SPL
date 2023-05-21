namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextPCEQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextPCEQuery(string pNroSerie, string pKeyTest, string pLenguage, string pEnergizedWinding)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;

            this.EnergizedWinding = pEnergizedWinding;
 

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string EnergizedWinding { get; }

   

        #endregion
    }
}
