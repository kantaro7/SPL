namespace SPL.WebApp.Domain.Services
{
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IIszService
    {
        void PrepareTemplate_Isz(SettingsToDisplayISZReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba, decimal degreesCor, List<PlateTensionDTO> tension, ref int filas,ref string posiMayor,
            string idioma, string[] AtList = null, string[] BTList = null, string[] TerList = null, string devanadoEnergizado=null,string seleccionadoTodosABT =null);

    }
}
