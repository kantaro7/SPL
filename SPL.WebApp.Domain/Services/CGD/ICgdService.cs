namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface ICgdService
    {
        void PrepareTemplate_CGD(SettingsToDisplayCGDReportsDTO reportsDTO, string typeTest, int hour1, int hour2, int hour3, ref Workbook workbook);

        void Prepare_CGD_Test(SettingsToDisplayCGDReportsDTO reportsDTO, Workbook workbook, string typeTest, string lenguage, ref List<CGDTestsDTO> _cgdTestsDTOs);

        void PrepareIndexOfCGD(ResultCGDTestsDTO resultRYDTestsDTO, SettingsToDisplayCGDReportsDTO reportsDTO, ref Workbook workbook, string idioma);

        DateTime[] GetDate(Workbook origin, SettingsToDisplayCGDReportsDTO reportsDTO);

        string[] GetResults(Workbook origin, SettingsToDisplayCGDReportsDTO reportsDTO);

        bool Verify_CGD_ColumnsToCalculate(SettingsToDisplayCGDReportsDTO reportsDTO, Workbook workbook, string typeTest);

        bool Verify_CGD_Columns(SettingsToDisplayCGDReportsDTO reportsDTO, Workbook workbook);
    }
}
