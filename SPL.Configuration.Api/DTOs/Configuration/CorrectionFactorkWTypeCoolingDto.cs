using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Configuration.Api.DTOs.Configuration
{
    public class CorrectionFactorkWTypeCoolingDto
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El campo CoolingType es requerido")]
        [MaxLength(200, ErrorMessage = "El campo CoolingType solo puede tener {0} caracteres")]
        public string CoolingType { get; set; }

        [Required(ErrorMessage = "El campo FactorCorr es requerido")]
        [Range(0.01, 999.99, ErrorMessage = "El factor de corrección debe ser numérico mayor a cero considerando 3 enteros con 2 decimales")]
        public decimal FactorCorr { get; set; }
 

        [Required(ErrorMessage = "El campo Creadopor es requerido")]
        public string Creadopor { get; set; }

        [Required(ErrorMessage = "El campo Fechacreacion es requerido")]
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
