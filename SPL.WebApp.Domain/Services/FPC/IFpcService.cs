namespace SPL.WebApp.Domain.Services.FPC
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IFpcService
    {
        void PrepareTemplate_FPC(SettingsToDisplayFPCReportsDTO reportsDTO, ref Workbook workbook, string specification, string idioma);

        void Prepare_FPC_Test(SettingsToDisplayFPCReportsDTO reportsDTO, Workbook workbook, ref List<FPCTestsDTO> _fpcTestDTOs);

        void PrepareIndexOfFPC(ResultFPCTestsDTO resultFPCTestsDTO, SettingsToDisplayFPCReportsDTO reportsDTO, string idioma, ref Workbook workbook);

        void CloneWorkbook(Workbook origin, SettingsToDisplayFPCReportsDTO reportsDTO, ref Workbook official, out List<DateTime> dates);

        DateTime[] GetDate(Workbook origin, SettingsToDisplayFPCReportsDTO reportsDTO);

        bool Verify_FPC_Columns(SettingsToDisplayFPCReportsDTO reportsDTO, Workbook workbook, int rows);
    }
}
