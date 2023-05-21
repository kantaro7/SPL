using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplInfoGeneralEtd
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

		public int IdCorte { get; set; }
		public int IdReg { get; set; }
		public bool BtDifCap { get; set; }
		public decimal CapacidadBt { get; set; }
		public string Terciario2b { get; set; }
		public bool TerCapRed { get; set; }
		public decimal CapacidadTer { get; set; }
		public string DevanadoSplit { get; set; }
		public DateTime UltimaHora { get; set; }
		public decimal FactorKw { get; set; }
		public decimal FactorAltitud { get; set; }
		public bool TipoRegresion { get; set; }

		public string Comentario { get; set; }
		public string Creadopor { get; set; }
		public DateTime Fechacreacion { get; set; }
		public string Modificadopor { get; set; }
		public DateTime? Fechamodificacion { get; set; }

	}
}
