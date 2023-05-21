namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class CatSidcoDTO 
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public string ClaveSPL { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
