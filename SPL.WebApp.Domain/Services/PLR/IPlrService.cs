namespace SPL.WebApp.Domain.Services
{

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IPlrService
    {
        void PrepareTemplate_PLR (SettingsToDisplayPLRReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba , int? cantidadTension = 0, int? cantidadTiempo = 0 , decimal? reactanciaLineal = 0);

    }
}
