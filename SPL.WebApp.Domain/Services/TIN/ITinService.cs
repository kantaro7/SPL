namespace SPL.WebApp.Domain.Services
{

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface ITinService
    {
        void PrepareTemplate_Tin(SettingsToDisplayTINReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje, string connection, string voltaje ,ref CeldasValidate celdas);

    }
}
