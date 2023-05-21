namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IFpbService
    {
        void PrepareTemplate_FPB(SettingsToDisplayFPBReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba, string tangtDelta);

        void Prepare_FPB_Test( Workbook workbook, ref List<FPBTestsDTO> _fpbTestDTOs , SettingsToDisplayFPBReportsDTO fpbReport,string clavePrueba);

        void PrepareIndexOfFPB(ResultFPBTestsDTO resultFPBTestsDTO, SettingsToDisplayFPBReportsDTO reportsDTO, string idioma, ref Workbook workbook, bool resultReport, string CLavePrueba,string TanDelta);

        void CloneWorkbook(Workbook origin, SettingsToDisplayFPCReportsDTO reportsDTO, ref Workbook official, out List<DateTime> dates);

        bool Verify_FPB_Columns( Workbook workbook , SettingsToDisplayFPBReportsDTO info, string ClavePrueba);

    }
}
