namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextARFQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextARFQuery(string pNroSerie, string pKeyTest, string pLenguage,
string pTeam, string pTertiary2Low, string pTertiaryDisp, string pLevelsVoltage)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.Team = pTeam;
            this.Tertiary2Low = pTertiary2Low;
            this.TertiaryDisp = pTertiaryDisp;
            this.LevelsVoltage = pLevelsVoltage;
  

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public string Team { get; }
        public string Tertiary2Low { get; }
        public string TertiaryDisp { get; }
        public string LevelsVoltage { get; }
    
        #endregion
    }
}
