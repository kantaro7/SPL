namespace SPL.WebApp.Domain.Services
{
    using SPL.WebApp.Domain.DTOs;

    using System;
    using System.Collections.Generic;

    using Telerik.Web.Spreadsheet;

    public interface IRanService
    {

        void PrepareTemplate_RAN_AYD(int measuring, string languageKey, SettingsToDisplayRANReportsDTO reportsDTO, ref Workbook workbook);

        void PrepareTemplate_RAN_APD(int measuring, string languageKey, SettingsToDisplayRANReportsDTO reportsDTO, ref Workbook workbook);

        void Prepare_RAN_Test(string testType, int measuring, SettingsToDisplayRANReportsDTO reportsDTO, Workbook workbook, ref RANTestsDetailsDTO _ranTestDTO);

        void PrepareIndexOfRAN(ResultRANTestsDTO resultRANTestsDTO, SettingsToDisplayRANReportsDTO reportsDTO, string keyLenguage, ref Workbook workbook, string ClavePrueba);
        void DeleteValid(string testType, int measuring, SettingsToDisplayRANReportsDTO reportsDTO, ref Workbook workbook);

        void CloneWorkbook(string testType, string ketLanguage, Workbook Workbook, SettingsToDisplayRANReportsDTO RANReportsDTO, int measuring, ref Workbook officialWorkbook);

        List<DateTime> GetDate(Workbook origin, SettingsToDisplayRANReportsDTO reportsDTO, string keyTests);

        //string Validate(string testType, Workbook workbook, SettingsToDisplayRADReportsDTO reportsDTO);

    }
}
