namespace SPL.WebApp.Domain.Services
{
    using System;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IPrdService
    {
        void PrepareTemplate_PRD(SettingsToDisplayPRDReportsDTO reportsDTO, ref Workbook workbook);

        void Prepare_PRD_Test(SettingsToDisplayPRDReportsDTO reportsDTO, Workbook workbook, ref PRDTestsDTO _prdTestDTO);

        void PrepareIndexOfPRD(ResultPRDTestsDTO resultPRDTestsDTO, SettingsToDisplayPRDReportsDTO reportsDTO, ref Workbook workbook);

        DateTime GetDate(Workbook origin, SettingsToDisplayPRDReportsDTO reportsDTO);

        bool Verify_PRD_Columns(SettingsToDisplayPRDReportsDTO reportsDTO, Workbook workbook);

        bool Verify_PRD_ColumnsToCalculate(SettingsToDisplayPRDReportsDTO reportsDTO, Workbook workbook);
    }
}
