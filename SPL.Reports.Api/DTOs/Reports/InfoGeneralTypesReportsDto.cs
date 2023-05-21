namespace SPL.Reports.Api.DTOs.Reports
{
    using System.Collections.Generic;

    public class InfoGeneralTypesReportsDto
    {
        public string TipoReporte { get; set; }

        public List<InfoGeneralReportsDto> ListDetails { get; set; }

        public InfoGeneralTypesReportsDto()
        {
        }

        public InfoGeneralTypesReportsDto(string pTipoReporte, List<InfoGeneralReportsDto> Details)
        {
            this.TipoReporte = pTipoReporte;
            this.ListDetails = Details;
        }
    }
}
