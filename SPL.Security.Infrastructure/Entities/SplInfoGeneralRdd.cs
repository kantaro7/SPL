using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplInfoGeneralRdd
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
        public string ConfigDevanado { get; set; }
        public string Conexion { get; set; }
        public decimal PorcZ { get; set; }
        public decimal PorcJx { get; set; }
        public decimal CapacidadPrueba { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string DevEnergizado { get; set; }
        public decimal TensionEnerg { get; set; }
        public string DevCorto { get; set; }
        public decimal TensionCorto { get; set; }
        public decimal S3fV2 { get; set; }
        public decimal PorcX { get; set; }
        public decimal DporcX { get; set; }
        public decimal ValorAceptacion { get; set; }
        public DateTime FechaPrueba { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
