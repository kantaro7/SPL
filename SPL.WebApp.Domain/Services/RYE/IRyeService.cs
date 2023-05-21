namespace SPL.WebApp.Domain.Services
{
    using System;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IRyeService
    {
        void PrepareTemplate_RYE(SettingsToDisplayRYEReportsDTO reportsDTO, ref Workbook workbook, decimal perdidasVacio, decimal perdidasEnf, decimal porcZ, decimal perdidasCarga);

        void Prepare_RYE_Test(SettingsToDisplayRYEReportsDTO reportsDTO, Workbook workbook, ref OutRYETestsDTO _ryeTestDTO);

        void PrepareIndexOfRYE(ResultRYETestsDTO resultRYDTestsDTO, SettingsToDisplayRYEReportsDTO reportsDTO, ref Workbook workbook);

        DateTime GetDate(Workbook origin, SettingsToDisplayRYEReportsDTO reportsDTO);

        bool Verify_RYE_ColumnsToCalculate(SettingsToDisplayRYEReportsDTO reportsDTO, Workbook workbook);

        bool Verify_RYE_Columns(SettingsToDisplayRYEReportsDTO reportsDTO, Workbook workbook);
    }
}
