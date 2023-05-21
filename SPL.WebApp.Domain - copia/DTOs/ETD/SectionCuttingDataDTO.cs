namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SectionCuttingDataDTO
    {
        public decimal IdCorte { get; set; }
        public decimal Seccion { get; set; }
        public string CapturaEn { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(10, ErrorMessage = "Límite máximo de Caracteres.")]
        public string Terminal { get; set; }
        [RegularExpression(@"^(([0-9]{0,3})|([0-9]{0,3}\.[0-9]{1,4}))$", ErrorMessage = "Resistencia de corte debe ser numérico mayor a cero considerando 3 enteros con 4 decimales")]
        public decimal Resistencia { get; set; }
        public string UmResistencia { get; set; }
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "Temperatura de la resistencia de corte debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal TempResistencia { get; set; }
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1,2}))$", ErrorMessage = "Factor K debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal FactorK { get; set; }
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "Temperatura promedio del aceite debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
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

        public List<DetailCuttingDataDTO> DetailCuttingData { get; set; }
    }
}
