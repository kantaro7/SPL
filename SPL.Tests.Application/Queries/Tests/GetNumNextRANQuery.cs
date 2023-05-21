namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextRANQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextRANQuery(string pNroSerie, string pKeyTest, string pLenguage, int pNumberMeasurements)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.NumberMeasurements = pNumberMeasurements;
            this.Lenguage = pLenguage;

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public int NumberMeasurements { get; }

        #endregion
    }
}
