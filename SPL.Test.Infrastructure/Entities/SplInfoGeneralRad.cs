using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoGeneralRad
    {
        public decimal IdCarga { get; set; }
        public DateTime? FechaCarga { get; set; }
        public string NoSerie { get; set; }
        public decimal? NoPrueba { get; set; }
        public string ClaveIdioma { get; set; }
        public string Cliente { get; set; }
        public string Capacidad { get; set; }
        public bool? Resultado { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] Archivo { get; set; }
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string TipoUnidad { get; set; }
        public string TercerDevanadoTipo { get; set; }
        public decimal? ValorMinimo { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
