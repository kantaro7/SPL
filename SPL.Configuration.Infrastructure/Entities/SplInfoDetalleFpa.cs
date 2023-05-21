using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoDetalleFpa
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public string Descripcion { get; set; }
        public string Asmt { get; set; }
        public decimal? Medicion { get; set; }
        public decimal? Escala { get; set; }
        public decimal? FactorCorr { get; set; }
        public decimal? FactorPotencia { get; set; }
        public decimal? Apertura { get; set; }
        public decimal? Valor1 { get; set; }
        public decimal? Valor2 { get; set; }
        public decimal? Valor3 { get; set; }
        public decimal? Valor4 { get; set; }
        public decimal? Valor5 { get; set; }
        public decimal? Promedio { get; set; }
        public decimal LimiteMax { get; set; }
        public string Validacion { get; set; }
    }
}
