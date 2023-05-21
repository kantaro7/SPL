namespace SPL.WebApp.Domain.DTOs
{

    public class ConnectionTypesDTO
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
