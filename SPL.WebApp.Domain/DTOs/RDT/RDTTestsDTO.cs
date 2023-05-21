namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class RDTTestsDTO
    {
        public decimal AcceptanceValue { get; set; }
        public List<ColumnDTO> Columns { get; set; }
        public List<string> Positions { get; set; }
        public List<decimal> Tensions { get; set; }
        public List<int> OrderPositions { get; set; }
        public string AllTension { get; set; }
        public string UnitTension { get; set; }
        public decimal UnitValue { get; set; }
        public GeneralPropertiesDTO AngularDisplacement { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
