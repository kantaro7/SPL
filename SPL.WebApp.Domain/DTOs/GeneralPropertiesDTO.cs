namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class GeneralPropertiesDTO
    {
        public decimal Id { get; set; }
        public string Clave { get; set; }
        public string Description { get; set; }
        public string Descripcion { get; set; }
        public decimal H_wye { get; set; }
        public decimal X_wye { get; set; }
        public decimal T_wye { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
