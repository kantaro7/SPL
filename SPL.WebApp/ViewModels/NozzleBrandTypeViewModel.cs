namespace SPL.WebApp.ViewModels
{
    using SPL.WebApp.Domain.Enums;

    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class NozzleBrandTypeViewModel
    {
        [DisplayName("Marca")]
        [Required(ErrorMessage = "Requerido")]
        public string BrandId { get; set; }

        [DisplayName("Tipo")]
        [Required(ErrorMessage = "Requerido")]
        public int Type { get; set; }

        [DisplayName("Descripción")]
        [StringLength(128, ErrorMessage = "La Descripción debe tener un máximo de 128 caracteres")]
        [Required(ErrorMessage = "Requerido")]
        public string Description { get; set; }

        [DisplayName("Estatus")]
        [Required(ErrorMessage = "Requerido")]
        public Status Status { get; set; }
        public int? OperationType { get; set; }

        public string CreadoPor { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
