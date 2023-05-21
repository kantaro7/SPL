namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IPceService
    {
        void PrepareTemplate_PCE(SettingsToDisplayPCEReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma, int inicio, int fin, int intervalo, string keyTest);

        void Prepare_PCE_Test(SettingsToDisplayPCEReportsDTO reportsDTO, Workbook workbook, ref List<PCETestsDTO> _fpcTestDTOs);

        void PrepareIndexOfPCE(ResultPCETestsDTO resultPCETestsDTO, SettingsToDisplayPCEReportsDTO reportsDTO, string idioma, ref Workbook workbook);

        List<DateTime> GetDate(Workbook origin, SettingsToDisplayPCEReportsDTO reportsDTO);

        bool Verify_PCE_Columns(SettingsToDisplayPCEReportsDTO reportsDTO, Workbook workbook, int renglones);
    }
}
