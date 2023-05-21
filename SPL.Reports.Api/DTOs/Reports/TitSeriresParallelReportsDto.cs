namespace SPL.Reports.Api.DTOs.Reports
{
    using System;

    public class TitSeriresParallelReportsDto
    {
        public decimal ClaveSepa { get; set; }
        public string ClaveIdioma { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
