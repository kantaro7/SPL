using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.DTOs.Artifactdesign
{
   public  class TapsDto
    {
        public TapsDto() { }
        public decimal? TapsAltaTension { get; set; }
        public decimal? TapsAltaTension_2 { get; set; }
        public decimal? TapsBajaTension { get; set; }
        public decimal? TapsSegundaBaja { get; set; }
        public decimal? TapsTerciario { get; set; }

    }
}
