namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class ColumnTitleRODReportsDTO
    {
        public string ClaveIdioma { get; set; }

        public bool Load { get; set; }

        public int Sup { get; set; }

        public int Inf { get; set; }

        public int Nom { get; set; }

        public bool Inv { get; set; }

        public int Iden { get; set; }

        public string Terminal1 { get; set; }

        public string Terminal2 { get; set; }

        public string Terminal3 { get; set; }

        public int TempSE { get; set; }

        public string Connection { get; set; }

        public List<string> Positions { get; set; }

        public int TotalPositions { get; set; }

        public string Position { get; set; }

        public string Creadopor { get; set; }

        public DateTime Fechacreacion { get; set; }

        public string Modificadopor { get; set; }
        
        public DateTime? Fechamodificacion { get; set; }
    }
}