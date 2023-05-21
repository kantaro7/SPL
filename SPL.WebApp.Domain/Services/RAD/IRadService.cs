namespace SPL.WebApp.Domain.Services
{
    using SPL.WebApp.Domain.DTOs;
    using Telerik.Web.Spreadsheet;

    public interface IRadService
    {
        void PrepareTemplate_RAD_CAYDES(SettingsToDisplayRADReportsDTO reportsDTO, ref Workbook workbook);

        void PrepareTemplate_RAD_SA(SettingsToDisplayRADReportsDTO reportsDTO, ref Workbook workbook);

        void Prepare_RAD_Test(string testType, SettingsToDisplayRADReportsDTO reportsDTO, Workbook workbook, ref RADTestsDTO _radTestDTO);

        void PrepareIndexOfRAD(ResultRADTestsDTO resultRADTestsDTO, SettingsToDisplayRADReportsDTO reportsDTO, string keyLenguage, ref Workbook workbook);

        void CloneWorkbook(string testType, string ketLanguage, Workbook Workbook, SettingsToDisplayRADReportsDTO RADReportsDTO, ResultRADTestsDTO resultRADTestsDTO, ref Workbook officialWorkbook);

        string Validate(string testType, Workbook workbook, SettingsToDisplayRADReportsDTO reportsDTO);
    }
}
