using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplInfoGeneralCem
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
        public string IdPosPrimaria { get; set; }
        public string PosPrimaria { get; set; }
        public string IdPosSecundaria { get; set; }
        public string PosSecundaria { get; set; }
        public decimal VoltajePrueba { get; set; }
        public DateTime FechaPrueba { get; set; }
        public string TituloTerm1 { get; set; }
        public string TituloTerm2 { get; set; }
        public string TituloTerm3 { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
