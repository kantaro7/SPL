namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextTDPQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextTDPQuery(string pNroSerie, string pKeyTest, string pLenguage,
int pNoCycles, int pTotalTime, int pInterval, decimal pTimeLevel, decimal pOutputLevel, int pDescMayPc, int pDescMayMv, int pIncMaxPc, string pVoltageLevels, string pMeasurementType, string pTerminalsTest)
        {
            this.NroSerie = pNroSerie;
            this.KeyTests = pKeyTest;
            this.Lenguage = pLenguage;
            this.NoCycles = pNoCycles;
            this.TotalTime = pTotalTime;
            this.Interval = pInterval;
            this.TimeLevel = pTimeLevel;
            this.OutputLevel = pOutputLevel;
            this.DescMayMv = pDescMayMv;
            this.DescMayPc = pDescMayPc;
            this.IncMaxPc = pIncMaxPc;
            this.VoltageLevels = pVoltageLevels;
            this.MeasurementType = pMeasurementType;
            this.TerminalsTest = pTerminalsTest;
  
                

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string Lenguage { get; }
        public int NoCycles { get; }
        public int TotalTime { get; }
        public int Interval { get; }
        public decimal TimeLevel { get; }
        public decimal OutputLevel { get; }
        public int DescMayPc { get; }
        public int DescMayMv { get; }
        public int IncMaxPc { get; }
        public string VoltageLevels { get; }
        public string MeasurementType { get; }
        public string TerminalsTest { get; }
 



        #endregion
    }
}
