namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class NozzleMarksDTO
    {
        public long IdMarca { get; set; }
        public string Marca { get; set; }
        public decimal IdTipo { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }
        public int EstatusId { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
