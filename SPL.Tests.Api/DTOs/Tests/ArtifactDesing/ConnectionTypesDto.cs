using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.Artifactdesign
{
   public  class ConnectionTypesDto
    {
        public int IdConexionAltaTension { get; set; }
        public string ConexionAltaTension { get; set; }
        public string OtraConexionAltaTension { get; set; }

        public int IdConexionBajaTension { get; set; }
        public string ConexionBajaTension { get; set; }
        public string OtraConexionBajaTension { get; set; }

        public int IdConexionSegundaBaja { get; set; }
        public string ConexionSegundaBaja { get; set; }
        public string OtraConexionSegundaBaja { get; set; }

        public int IdConexionTercera { get; set; }
        public string ConexionTercera { get; set; }
        public string OtraConexionTercera { get; set; }

    }
}
