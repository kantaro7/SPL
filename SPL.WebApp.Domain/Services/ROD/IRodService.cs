namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IRodService
    {
        void PrepareTemplate_ROD(SettingsToDisplayRODReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma , ref List<string> celdas,string unidad);

        bool VerifyPrepare_ROD_Test(SettingsToDisplayRODReportsDTO reportsDTO, Workbook workbook, ref List<RODTestsDTO> _rodTestDTOs);

        void Prepare_ROD_Test(SettingsToDisplayRODReportsDTO reportsDTO, Workbook workbook, ref List<RODTestsDTO> _fpcTestDTOs);

        void PrepareIndexOfROD(ResultRODTestsDTO resultRODTestsDTO, SettingsToDisplayRODReportsDTO reportsDTO, string idioma, ref Workbook workbook);

        DateTime GetDate(Workbook origin, SettingsToDisplayRODReportsDTO reportsDTO);

        bool Verify_ROD_Columns(SettingsToDisplayRODReportsDTO reportsDTO, Workbook workbook, int rows1, int rows2, int rows3);
    }
}
