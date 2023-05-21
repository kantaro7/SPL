namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextRADQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextRADQuery(string pNroSerie, string pKeyTest, string pTypeUnit, string pThirdWinding, string pLenguage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.TypeUnit = pTypeUnit;
            this.ThirdWinding = pThirdWinding;
            this.Lenguage = pLenguage;

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string TypeUnit { get; }
        public string ThirdWinding { get; }
        public string Lenguage { get; }
        #endregion
    }
}
