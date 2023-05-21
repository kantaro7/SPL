namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HeaderCuttingDataDTO
    {
        public decimal IdCorte { get; set; }
        public string NoSerie { get; set; }
        public decimal IdReg { get; set; }
        public decimal KwPrueba { get; set; }

        public int LimitEst { get; set; }
        public int TipoRegresion { get; set; }
        public decimal Constante { get; set; }
        public DateTime UltimaHora { get; set; }
        public DateTime PrimerCorte { get; set; }
        public DateTime? SegundoCorte { get; set; }
        public DateTime? TercerCorte { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public StabilizationDataDTO StabilizationData { get; set; }
        public List<SectionCuttingDataDTO> SectionCuttingData { get; set; }
    }
}
