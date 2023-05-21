using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.DTOs.Artifactdesign
{
   public class VoltageKVDto
    {

        public decimal? TensionKvAltaTension1 { get; set; }
        public decimal? TensionKvAltaTension2 { get; set; }
        public decimal? TensionKvAltaTension3 { get; set; }
        public decimal? TensionKvAltaTension4 { get; set; }
        public decimal? TensionKvBajaTension1 { get; set; }
        public decimal? TensionKvBajaTension2 { get; set; }
        public decimal? TensionKvBajaTension3 { get; set; }
        public decimal? TensionKvBajaTension4 { get; set; }
        public decimal? TensionKvSegundaBaja1 { get; set; }
        public decimal? TensionKvSegundaBaja2 { get; set; }
        public decimal? TensionKvSegundaBaja3 { get; set; }
        public decimal? TensionKvSegundaBaja4 { get; set; }
        public decimal? TensionKvTerciario1 { get; set; }
        public decimal? TensionKvTerciario2 { get; set; }
        public decimal? TensionKvTerciario3 { get; set; }
        public decimal? TensionKvTerciario4 { get; set; }
        public string NoSerie { get; set; }
    }
}
