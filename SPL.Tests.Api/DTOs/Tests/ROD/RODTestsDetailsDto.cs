using SPL.Tests.Api.DTOs.Tests.Artifactdesign;

namespace SPL.Tests.Api.DTOs.Tests.ROD
{
    public class RODTestsDetailsDto
    {
 
        public string Position { get; set; }
        public decimal TerminalH1 { get; set; }
        public decimal TerminalH2 { get; set; }
        public decimal TerminalH3 { get; set; }
        public decimal AverageResistance { get; set; }
        public decimal PercentageA { get; set; }
        public decimal PercentageB { get; set; }
        public decimal Desv { get; set; }
        public decimal DesvDesign { get; set; }

        public ResistDesignDto ResistDesigns { get; set; }
    }
}
