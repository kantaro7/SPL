namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextFPCQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextFPCQuery(string pNroSerie, string pKeyTest, string pTypeUnit, string pSpecification, string pFrequency, string pLenguage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.TypeUnit = pTypeUnit;
            this.Specification = pSpecification;
            this.Frequency = pFrequency;
            this.Lenguage = pLenguage;

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string TypeUnit { get; }
        public string Specification { get; }
        public string Frequency { get; }
        public string Lenguage { get; }
   

        #endregion
    }
}
