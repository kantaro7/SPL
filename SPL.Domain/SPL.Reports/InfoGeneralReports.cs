namespace SPL.Domain.SPL.Reports
{
    using System;

    public  class InfoGeneralReports
    {
        public decimal IdCarga { get; set; }
        public DateTime? FechaCarga { get; set; }
        public string NoSerie { get; set; }
        public decimal? NoPrueba { get; set; }
        public string ClaveIdioma { get; set; }
        public string Cliente { get; set; }
        public string Capacidad { get; set; }
        public bool? Resultado { get; set; }
        public string NombreArchivo { get; set; }
        public string Archivo { get; set; }
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string DesplazamientoAngular { get; set; }
        public string Norma { get; set; }
        public decimal? ConexionSp { get; set; }
        public string PostPruebaBt { get; set; }
        public string Ter { get; set; }
        public string Creadopor { get; set; }
        public string Comentario { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public DateTime? Fecha { get; set; }
        public string PosAt { get; set; }
    }
}
