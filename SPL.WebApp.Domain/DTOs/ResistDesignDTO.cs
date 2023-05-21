namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class ResistDesignDTO
    {
        public int? Id { get; set; }
        public string NoSerie { get; set; }
        public string UnidadMedida { get; set; }
        public string ConexionPrueba { get; set; }
        public decimal Temperatura { get; set; }
        public string IdSeccion { get; set; }
        public decimal Orden { get; set; }
        public string Posicion { get; set; }
        public decimal Resistencia { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
