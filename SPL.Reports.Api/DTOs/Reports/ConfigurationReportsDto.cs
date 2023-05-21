namespace SPL.Reports.Api.DTOs.Reports
{
    using System;

    public class ConfigurationReportsDto
    {
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string Apartado { get; set; }
        public decimal Seccion { get; set; }
        public string Dato { get; set; }
        public string Celda { get; set; }
        public string TipoDato { get; set; }
        public string Formato { get; set; }
        public string Obtencion { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
