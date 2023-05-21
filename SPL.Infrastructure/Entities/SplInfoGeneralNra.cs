using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplInfoGeneralNra
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
        public string Laboratorio { get; set; }
        public string ClaveNorma { get; set; }
        public string Alimentacion { get; set; }
        public decimal ValorAlimentacion { get; set; }
        public string UmAlimentacion { get; set; }
        public string CoolingType { get; set; }
        public decimal CantMediciones { get; set; }
        public bool CargarInfo { get; set; }
        public DateTime? FechaDatos { get; set; }
        public decimal MedAyd { get; set; }
        public DateTime FechaPrueba { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        public decimal Altura { get; set; }
        public string UmAltura { get; set; }
        public decimal Perimetro { get; set; }
        public string UmPerimetro { get; set; }
        public decimal Area { get; set; }
        public string UmArea { get; set; }
        public string Notas { get; set; }
        public decimal FactorK { get; set; }
        public decimal Garantia { get; set; }
        public decimal Npplp { get; set; }
        public decimal Prlw { get; set; }
        public decimal Sv { get; set; }
        public decimal? Alfa { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
