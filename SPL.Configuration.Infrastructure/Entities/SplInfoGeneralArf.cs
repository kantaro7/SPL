﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoGeneralArf
    {
        public decimal IdRep { get; set; }
        public DateTime FechaRep { get; set; }
        public string NoSerie { get; set; }
        public decimal NoPrueba { get; set; }
        public string ClaveIdioma { get; set; }
        public string Cliente { get; set; }
        public string Capacidad { get; set; }
        public bool Resultado { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] Archivo { get; set; }
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string Equipo { get; set; }
        public string Terciario2dabaja { get; set; }
        public string TerciarioDisp { get; set; }
        public string NivelesTension { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal TotalPags { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
