namespace SPL.WebApp.Domain.Services
{
    using System;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IFpaService
    {
        void PrepareTemplate_FPA(SettingsToDisplayFPAReportsDTO reportsDTO, ref Workbook workbook, bool incluirSegundaFila, string keyTests, string oilType);

        void Prepare_FPA_Test(SettingsToDisplayFPAReportsDTO reportsDTO, Workbook workbook, ref FPATestsDTO fpaTestsDTO, bool incluirSegundaFila);

        void PrepareIndexOfFPA(ResultFPATestsDTO resultFPATestsDTO, SettingsToDisplayFPAReportsDTO reportsDTO, string idioma, ref Workbook workbook, bool incluirSegundaFila, string keyTests);

        bool Verify_FPA_Columns(SettingsToDisplayFPAReportsDTO reportsDTO, Workbook workbook, bool incluirSegundaFila, string keyTests);

        DateTime GetDate(SettingsToDisplayFPAReportsDTO reportsDTO, Workbook workbook);

        string GetGrades(SettingsToDisplayFPAReportsDTO reportsDTO, Workbook workbook);
    }
}
