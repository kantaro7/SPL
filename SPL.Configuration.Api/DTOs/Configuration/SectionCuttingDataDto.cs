namespace SPL.Configuration.Api.DTOs.Configuration
{
    using System.Collections.Generic;

    public class SectionCuttingDataDto
    {
        public decimal IdCorte { get; set; }
        public decimal Seccion { get; set; }
        public string CapturaEn { get; set; }
        public string Terminal { get; set; }
        public decimal Resistencia { get; set; }
        public string UmResistencia { get; set; }
        public decimal TempResistencia { get; set; }
        public decimal FactorK { get; set; }
        public decimal TempAceiteProm { get; set; }
        public decimal LimiteEst { get; set; }
        public decimal ResistZeroC { get; set; }
        public decimal TempDevC { get; set; }
        public decimal GradienteCaC { get; set; }
        public decimal AwrC { get; set; }
        public decimal HsrC { get; set; }
        public decimal HstC { get; set; }
        public decimal WindT { get; set; }
        public decimal ResistZeroE { get; set; }
        public decimal TempDevE { get; set; }
        public decimal GradienteCaE { get; set; }
        public decimal AwrE { get; set; }
        public decimal HsrE { get; set; }
        public decimal HstE { get; set; }

        public List<DetailCuttingDataDto> DetailCuttingData { get; set; }
    }
}
