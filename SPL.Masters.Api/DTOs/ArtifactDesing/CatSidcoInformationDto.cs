namespace SPL.Masters.Api.DTOs.Artifactdesign
{
    using System;

    public class CatSidcoInformationDto
    {
        public decimal Id { get; set; }
        public decimal? AttributeId { get; set; }
        public string ClaveSpl { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
