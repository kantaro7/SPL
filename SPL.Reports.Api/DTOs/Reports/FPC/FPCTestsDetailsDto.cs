using System.ComponentModel.DataAnnotations;

namespace SPL.Reports.Api.DTOs.Reports.FPC
{
    public class FPCTestsDetailsDto
    {
        [Range(1, 99, ErrorMessage = "El campo Renglón debe ser numérico mayor a cero considerando hasta 2 enteros")]
        public int Row { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "El campo Dev E solo puede tener {0} caracteres")]
        public string WindingA { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "El campo Dev T solo puede tener {0} caracteres")]
        public string WindingB { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "El campo Dev G solo puede tener {0} caracteres")]
        public string WindingC { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "El campo Dev UST solo puede tener {0} caracteres")]
        public string WindingD { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Corriente debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal Current { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Potencia debe ser numérico mayor a cero considerando hasta 3 enteros y 3 decimales")]
        public decimal Power { get; set; }



        [Range(0.001, 999.999, ErrorMessage = "El campo PorcFp debe ser numérico mayor a cero considerando hasta 3 enteros y 3 decimales")]
        public decimal PercentageA { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo TangPorcFp debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal PercentageB { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo CorrPorcFp debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal PercentageC { get; set; }

        [Range(1, 999999999999999999, ErrorMessage = "El campo Capacitancia debe ser numérico mayor a cero considerando hasta 18 enteros")]
        public decimal Capacitance { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "El campo ID solo puede tener {0} caracteres")]
        public string Id { get; set; }
    }
}