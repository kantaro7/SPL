namespace SPL.Tests.Api.DTOs.Tests
{
    using System;
    using System.Collections.Generic;

    using SPL.Domain.SPL.Masters;

    public class RDTTestsDto
    {
        public decimal AcceptanceValue { get; set; }
        public List<ColumnDto> Columns { get; set; }
        public List<string> Positions { get; set; }
        public List<decimal> Tensions { get; set; }
   
        public string AllTension { get; set; }
        public string UnitTension { get; set; }
        public decimal UnitValue { get; set; }
        public GeneralProperties AngularDisplacement { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
