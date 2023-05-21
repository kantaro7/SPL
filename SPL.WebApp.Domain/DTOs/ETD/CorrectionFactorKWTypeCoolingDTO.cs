namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CorrectionFactorKWTypeCoolingDTO
    {
            public string CoolingType { get; set; }
            public decimal FactorCorr { get; set; }
            public string Creadopor { get; set; }
            public DateTime Fechacreacion { get; set; }
            public string Modificadopor { get; set; }
            public DateTime? Fechamodificacion { get; set; }
    }
}
