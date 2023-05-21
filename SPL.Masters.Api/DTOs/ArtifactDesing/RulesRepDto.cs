namespace SPL.Masters.Api.DTOs.Artifactdesign
{
    using System;

    public class RulesRepDto
    {
        public string ClaveNorma { get; set; }
        public string ClaveIdioma { get; set; }
        public decimal Secuencia { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
