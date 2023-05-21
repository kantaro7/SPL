namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextPRDQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextPRDQuery(string pNroSerie, string pKeyTest, string pLenguage, decimal pNominalVoltage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.NominalVoltage = pNominalVoltage;
 

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public decimal NominalVoltage { get; }

   

        #endregion
    }
}
