using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplInfoGeneralTap
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
        public string TipoUnidad { get; set; }
        public decimal? NoConexionesAt { get; set; }
        public decimal? NoConexionesBt { get; set; }
        public decimal? NoConexionesTer { get; set; }
        public string IdCapAt { get; set; }
        public string IdCapBt { get; set; }
        public string IdCapTer { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal Frecuencia { get; set; }
        public decimal ValAcept { get; set; }
        public decimal? IdRepFpc { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
