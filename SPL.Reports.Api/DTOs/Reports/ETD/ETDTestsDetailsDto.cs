namespace SPL.Reports.Api.DTOs.Reports.ETD
{
    using System;
    using System.Collections.Generic;

    public class ETDTestsDetailsDto
    {

        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public DateTime? FechaHora { get; set; }
        public decimal? PromRadSup { get; set; }
        public decimal? PromRadInf { get; set; }
        public decimal? AmbienteProm { get; set; }
        public decimal? TempTapa { get; set; }
        public decimal? Tor { get; set; }
        public decimal? Aor { get; set; }
        public decimal? Bor { get; set; }
        public decimal? ElevAceiteSup { get; set; }
        public decimal? ElevAceiteProm { get; set; }
        public decimal? ElevAceiteInf { get; set; }
        public decimal? Tiempo { get; set; }
        public decimal? Resistencia { get; set; }
    }
}
