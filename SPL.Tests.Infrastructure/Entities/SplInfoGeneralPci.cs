using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoGeneralPci
    {
        #region Properties

        public decimal IdRep { get; set; }

        public string TipoReporte { get; set; }

        public string ClavePrueba { get; set; }

        public string ClaveIdioma { get; set; }

        public DateTime FechaRep { get; set; }

        public decimal NoPrueba { get; set; }

        public DateTime FechaPrueba { get; set; }

        public string Cliente { get; set; }

        public string Capacidad { get; set; }

        public string NoSerie { get; set; }

        public string CapRedBaja { get; set; }

        public string Autotransformador { get; set; }

        public string Monofasico { get; set; }

        public string MaterialDevanado { get; set; }

        public string PosPri { get; set; }

        public string PosSec { get; set; }

        public decimal? Kwcu { get; set; }

        public decimal? KwcuMva { get; set; }

        public decimal Kwtot100 { get; set; }

        public decimal Kwtot100M { get; set; }

        public string Comentario { get; set; }

        public bool Resultado { get; set; }

        public string NombreArchivo { get; set; }

        public byte[] Archivo { get; set; }

        public string Creadopor { get; set; }

        public DateTime Fechacreacion { get; set; }

        public string Modificadopor { get; set; }

        public DateTime? Fechamodificacion { get; set; }

        #endregion
    }
}
