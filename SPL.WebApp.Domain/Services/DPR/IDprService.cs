namespace SPL.WebApp.Domain.Services
{

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.DPR;

    using Telerik.Web.Spreadsheet;

    public interface IDprService
    {
        void PrepareTemplate(SettingsToDisplayDPRReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje);
        string ValidateTemplateDPR(SettingsToDisplayDPRReportsDTO reportsDTO, Workbook workbook);

    }
}
