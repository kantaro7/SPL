﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Configuration
{
    public class CorrectionFactorSpecification
    {
        public string Especificacion { get; set; }
        public decimal Temperatura { get; set; }
        public decimal FactorCorr { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
