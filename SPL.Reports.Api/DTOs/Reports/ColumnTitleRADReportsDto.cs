namespace SPL.Reports.Api.DTOs.Reports
{
    using System;

    public class ColumnTitleRADReportsDto
    {
        public string TipoUnidad { get; set; }
        public string TercerDevanadoTipo { get; set; }
        public decimal PosColumna { get; set; }
        public string ClaveIdioma { get; set; }
        public string Titulo { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
