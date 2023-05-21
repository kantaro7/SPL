namespace SPL.Reports.Api.DTOs.Reports
{
    using System;
    using System.Collections.Generic;

    public class RADTestsDto
    {
        public decimal AcceptanceValue { get; set; }
        public List<HeaderRADTestsDto> Headers { get; set; }
        public List<ColumnDto> Columns { get; set; }
        public List<ColumnDto> Columns2 { get; set; }
        public List<string> Times { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
