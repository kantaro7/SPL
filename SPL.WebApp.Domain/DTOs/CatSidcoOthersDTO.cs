namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class CatSidcoOthersDTO
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
