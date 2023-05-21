namespace SPL.Domain.SPL.Reports
{
    using System.Collections.Generic;

    public class InfoGeneralTypesReports
    {
        public string TipoReporte { get; set; }

        public List<InfoGeneralReports> ListDetails { get; set; }

        public InfoGeneralTypesReports()
        {
           

        }

        public InfoGeneralTypesReports(string pTipoReporte, List<InfoGeneralReports> Details)
        {
            this.TipoReporte = pTipoReporte;
            this.ListDetails = Details;

        }
    }
}
