namespace SPL.WebApp.Domain.Services
{
    using System;
    using SPL.WebApp.Domain.DTOs;
    using Telerik.Web.Spreadsheet;

    public interface ICemService
    {
        void PrepareTemplate_CEM (SettingsToDisplayCEMReportsDTO reportsDTO, ref Workbook workbook, string tensionPrimaria,string tensionSecundaria, string posicionPrimaria, string posicionesSecundarias, string voltage, string idioma);

        DateTime GetDate(Workbook origin, SettingsToDisplayRCTReportsDTO reportsDTO);

        bool Verify_CEM_Columns(SettingsToDisplayCEMReportsDTO reportsDTO, Workbook workbook, int cols);

        void Prepare_CEM_Test(SettingsToDisplayCEMReportsDTO reportsDTO, Workbook workbook, ref CEMTestsGeneralDTO _cemTestDTOs, int cols, string idioma, string tensionSecundaria);
    }
}
