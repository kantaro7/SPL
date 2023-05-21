﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.API.DTOs
{
    public class TapBaanDto
    {
        public TapBaanDto() { }
        public string OrderCode { get; set; }
        public decimal? ComboNumericSc { get; set; }
        public decimal? CantidadSupSc { get; set; }
        public decimal? PorcentajeSupSc { get; set; }
        public decimal? CantidadInfSc { get; set; }
        public decimal? PorcentajeInfSc { get; set; }
        public decimal? NominalSc { get; set; }
        public decimal? IdentificacionSc { get; set; }
        public decimal? InvertidoSc { get; set; }
        public decimal? ComboNumericBc { get; set; }
        public decimal? CantidadSupBc { get; set; }
        public decimal? PorcentajeSupBc { get; set; }
        public decimal? CantidadInfBc { get; set; }
        public decimal? PorcentajeInfBc { get; set; }
        public decimal? NominalBc { get; set; }
        public decimal? IdentificacionBc { get; set; }
        public decimal? InvertidoBc { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
