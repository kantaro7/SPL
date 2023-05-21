using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoGeneralDpr
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
        public decimal? NumeroCiclo { get; set; }
        public decimal? OtroCiclo { get; set; }
        public decimal TiempoTotal { get; set; }
        public decimal Intervalo { get; set; }
        public decimal NivelHora { get; set; }
        public decimal NivelRealce { get; set; }
        public decimal DescMayPc { get; set; }
        public decimal DescMayMv { get; set; }
        public decimal IncMaxPc { get; set; }
        public string TipoMedicion { get; set; }
        public string TerminalesPrueba { get; set; }
        public string TensionPrueba { get; set; }
        public decimal Frecuencia { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        public DateTime FechaPrueba { get; set; }
        public string Notas { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
