namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DetailCuttingDataDTO
    {
        public decimal IdCorte { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public decimal Tiempo { get; set; }
        public decimal Resistencia { get; set; }
        public decimal TempR { get; set; }
    }
}
