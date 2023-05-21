namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DetaCuttingDataDTO
    {

        public decimal FactorK { get; set; }  
        public DateTime FechaHora { get; set; }
        public decimal Capacidad { get; set; }
        public decimal OverElevation { get; set; }
        public decimal Perdidas { get; set; }
        public int PorCarga { get; set; }
        public string CoolingType { get; set; }

    }
}
