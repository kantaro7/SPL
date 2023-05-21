namespace SPL.WebApp.ViewModels
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using SPL.WebApp.Domain.DTOs;

    public class CorrectionFactorViewModel
    {
        [Required(ErrorMessage = "Requerido")]
        [DisplayName("Especificaciones")]
        public string EspecificacionId { get; set; }

        [DisplayName("Temperatura")]
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(3)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La temperatura debe ser numérica mayor o igual a cero considerando 3 enteros sin decimales")]
        [Range(0, 999, ErrorMessage = "La temperatura debe ser numérica mayor o igual a cero considerando 3 enteros sin decimales")]
        public string TemperatureId { get; set; }
        [DisplayName("Factor de Corrección")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$",ErrorMessage = "El factor de corrección debe ser numérico mayor o igual a cero considerando 3 enteros con 2 decimales")]
        [Range(0, 999.99,ErrorMessage = "El factor de corrección debe ser numérico mayor o igual a cero considerando 3 enteros con 2 decimales")]
        [Required(ErrorMessage = "Requerido")]
        public string FactorCorreccionId { get; set; }
        public List<CorrectionFactorDTO> DataFactorCorrecion { get; set; }
        public int? OperationType { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [Range(1, 100000, ErrorMessage = "Requerido")]
        [DisplayName("Marca")]
        public int MarcaId { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [Range(1, 100000, ErrorMessage = "Requerido")]
        [DisplayName("Tipo")]
        public int TipoId { get; set; }

        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
