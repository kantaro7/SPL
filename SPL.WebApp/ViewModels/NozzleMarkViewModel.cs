namespace SPL.WebApp.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class NozzleMarkViewModel
    {
        [Required(ErrorMessage = "Requerido")]
        [DisplayName("Marca")]
        [Range(0, 99999, ErrorMessage = "El identificador de la marca debe ser numérico mayor a cero considerando 5 enteros sin decimales")]
        public long IdMarca { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [DisplayName("Descripción")]
        [MaxLength(128, ErrorMessage = "La descripción de la marca no puede excederse de 128 caracteres")]
        [MinLength(1)]
        [RegularExpression(@"([a-zA-ZÁÉÍÓÚáéíóúü0-9_\s]+)", ErrorMessage = "La descripción debe ser alfanumerica")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [DisplayName("Estatus")]
        public bool Estatus { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
