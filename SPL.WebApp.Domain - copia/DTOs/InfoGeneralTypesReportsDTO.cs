namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class InfoGeneralTypesReportsDTO
    {
        public InfoGeneralTypesReportsDTO()
        {
            ListDetails = new List<InfoGeneralReportsDTO>();
        }

        public string TipoReporte { get; set; }

        public List<InfoGeneralReportsDTO> ListDetails { get; set; }
    }
}
