namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class InfoGeneralReportsDTO
    {
        public decimal IdCarga { get; set; }
        public DateTime? FechaCarga { get; set; }
        public string NoSerie { get; set; }
        public decimal? NoPrueba { get; set; }
        public string ClaveIdioma { get; set; }
        public string Cliente { get; set; }
        public bool? Resultado { get; set; }
        public string NombreArchivo { get; set; }
        public string Archivo { get; set; }
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string Comentario { get; set; }
    }
}
