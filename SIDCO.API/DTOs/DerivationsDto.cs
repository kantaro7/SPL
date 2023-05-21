using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.API.DTOs
{
   public class DerivationsDto
    {

        public string TipoDerivacionAltaTension { get; set; }
        public decimal? ValorDerivacionUpAltaTension { get; set; }
        public decimal? ValorDerivacionDownAltaTension { get; set; }
        public string TipoDerivacionAltaTension_2 { get; set; }
        public int? ValorDerivacionUpAltaTension_2 { get; set; }
        public int? ValorDerivacionDownAltaTension_2 { get; set; }
        public string TipoDerivacionBajaTension { get; set; }
        public decimal? ValorDerivacionUpBajaTension { get; set; }
        public decimal? ValorDerivacionDownBajaTension { get; set; }
        public string TipoDerivacionSegundaTension { get; set; }
        public decimal? ValorDerivacionUpSegundaTension { get; set; }
        public decimal? ValorDerivacionDownSegundaTension { get; set; }
        public string TipoDerivacionTerceraTension { get; set; }
        public decimal? ValorDerivacionUpTerceraTension { get; set; }
        public decimal? ValorDerivacionDownTerceraTension { get; set; }
        public string ConexionEquivalente { get; set; }
        public string ConexionEquivalente_2 { get; set; }
        public string ConexionEquivalente_3 { get; set; }
        public string ConexionEquivalente_4 { get; set; }
        public int IdConexionEquivalente { get; set; }
        public int IdConexionEquivalente2 { get; set; }
        public int IdConexionEquivalente3 { get; set; }
        public int IdConexionEquivalente4 { get; set; }
    }
}
