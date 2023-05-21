namespace SPL.WebApp.Domain.Services
{

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface ITdpService
    {
        void PrepareTemplate(SettingsToDisplayTDPReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje, string voltageLevels, int noCycles);
        string ValidateTemplateTDP(SettingsToDisplayTDPReportsDTO reportsDTO, Workbook workbook);

    }
}
