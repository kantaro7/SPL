namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IRctService
    {
        void PrepareTemplate_RCT(SettingsToDisplayRCTReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma, int cols, string unit, decimal? testVoltage, PositionsDTO positionsDTO, string clavePrueba);

        void Prepare_RCT_Test(SettingsToDisplayRCTReportsDTO reportsDTO, Workbook workbook, ref List<RCTTestsDTO> _rctTestDTOs, int cols);

        void PrepareIndexOfRCT(ResultRCTTestsDTO resultRODTestsDTO, SettingsToDisplayRCTReportsDTO reportsDTO, string idioma, ref Workbook workbook);

        DateTime GetDate(Workbook origin, SettingsToDisplayRCTReportsDTO reportsDTO); 
        string GetMeasurementType(Workbook origin, SettingsToDisplayRCTReportsDTO reportsDTO); 

        bool Verify_RCT_Columns(SettingsToDisplayRCTReportsDTO reportsDTO, Workbook workbook, int cols);
    }
}
