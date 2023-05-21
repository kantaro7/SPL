﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class CorrectionFactorkWTypeCoolingDto
    {
        public string CoolingType { get; set; }

        public decimal FactorCorr { get; set; }
 

        
        public string Creadopor { get; set; }


        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
