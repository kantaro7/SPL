namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using Telerik.Web.Spreadsheet;

    public interface IBpcService
    {
        void PrepareTemplate_BPC(SettingsToDisplayBPCReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma);

        void Prepare_BPC_Test(SettingsToDisplayBPCReportsDTO reportsDTO, Workbook workbook, ref BPCTestsGeneralDTO _bpcTestDTO);

        void PrepareIndexOfBPC(SettingsToDisplayBPCReportsDTO reportsDTO, ref Workbook workbook, string idioma);

        DateTime GetDate(Workbook origin, SettingsToDisplayBPCReportsDTO reportsDTO);

        string Verify_BPC_ColumnsToCalculate(SettingsToDisplayBPCReportsDTO reportsDTO, Workbook workbook);
        public bool GetResult(Workbook origin, SettingsToDisplayBPCReportsDTO reportsDTO);

        }
}
