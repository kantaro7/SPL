namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class RADTestsDTO
    {
        public decimal AcceptanceValue { get; set; }
        public List<HeaderRADTestsDTO> Headers { get; set; }

        public List<ColumnDTO> Columns { get; set; }

        public List<ColumnDTO> Columns2 { get; set; }

        public List<string> Times { get; set; }

        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
