namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextTINQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextTINQuery(string pNroSerie, string pKeyTest, string pLenguage,
        string pConnection, decimal pVoltage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.Connection = pConnection;
            this.Voltage = pVoltage;

                

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string Connection { get; }
        public decimal Voltage { get; }



        #endregion
    }
}
