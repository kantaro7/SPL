namespace SPL.Masters.Api.DTOs.Artifactdesign
{
    using System;

    public class CatSidcoOtherInformationDto
    {
        public decimal Dato { get; set; }
        public string Id { get; set; }
        public string ClaveIdioma { get; set; }
        public string Descripcion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
