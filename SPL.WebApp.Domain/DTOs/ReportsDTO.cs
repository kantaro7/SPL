namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class ReportsDTO
    {
        public string TipoReporte { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
