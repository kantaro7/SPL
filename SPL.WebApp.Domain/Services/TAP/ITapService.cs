namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface ITapService
    {
        void PrepareTemplate_TAP(SettingsToDisplayTAPReportsDTO reportsDTO,List<EstructuraReporte> reporte, ref Workbook workbook);

        void Prepare_TAP_Test(SettingsToDisplayTAPReportsDTO reportsDTO, Workbook workbook, ref TAPTestsDTO _tapTestDTO);

        void PrepareIndexOfTAP(ResultTAPTestsDTO resultRYDTestsDTO, SettingsToDisplayTAPReportsDTO reportsDTO, ref Workbook workbook, string idioma);

        DateTime GetDate(Workbook origin, SettingsToDisplayTAPReportsDTO reportsDTO);

        bool Verify_TAP_ColumnsToCalculate(SettingsToDisplayTAPReportsDTO reportsDTO, Workbook workbook);

        bool Verify_TAP_Columns(SettingsToDisplayTAPReportsDTO reportsDTO, Workbook workbook);
    }
}
