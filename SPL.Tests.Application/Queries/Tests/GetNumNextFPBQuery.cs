namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextFPBQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextFPBQuery(string pNroSerie, string pKeyTest, string pLenguage, string pTangentDelta)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.TangentDelta = pTangentDelta;

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string TangentDelta { get; }
   

        #endregion
    }
}
