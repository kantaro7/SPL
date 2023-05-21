namespace SPL.WebApp.Domain.Services
{
    using System;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IPeeService
    {
        void PrepareTemplate_PEE(SettingsToDisplayPEEReportsDTO reportsDTO, ref Workbook workbook);

        void Prepare_PEE_Test(SettingsToDisplayPEEReportsDTO reportsDTO, Workbook workbook, ref PEETestsDTO _prdTestDTO);

        void PrepareIndexOfPEE(ResultPEETestsDTO resultPRDTestsDTO, SettingsToDisplayPEEReportsDTO reportsDTO, ref Workbook workbook, string idioma);

        DateTime GetDate(Workbook origin, SettingsToDisplayPEEReportsDTO reportsDTO);

        bool Verify_PEE_ColumnsToCalculate(SettingsToDisplayPEEReportsDTO reportsDTO, Workbook workbook);

        bool Verify_PEE_Columns(SettingsToDisplayPEEReportsDTO reportsDTO, Workbook workbook);
    }
}
