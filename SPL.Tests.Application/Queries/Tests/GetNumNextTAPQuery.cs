namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextTAPQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextTAPQuery(string pNroSerie, string pKeyTest, string pLenguage,
      string pUnitType, int pNoConnectionAT, int pNoConnectionBT, int pNoConnectionTER, string pIdCapAT, string pIdCapBT, string pIdCapTer)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.UnitType = pUnitType;
            this.NoConnectionAT = pNoConnectionAT;
            this.NoConnectionBT = pNoConnectionBT;
            this.NoConnectionTER = pNoConnectionTER;
            this.IdCapAT = pIdCapAT;
            this.IdCapBT = pIdCapBT;
            this.IdCapTER = pIdCapTer;
                

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string UnitType { get; }
        public int NoConnectionAT { get; }
        public int NoConnectionBT { get; }
        public int NoConnectionTER { get; }
        public string IdCapAT { get; }
        public string IdCapBT { get; }
        public string IdCapTER { get; }

        #endregion
    }
}
