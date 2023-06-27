namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;

    using Telerik.Web.Spreadsheet;

    public interface IEtdService
    {
        void PrepareTemplate_ETD(SettingsToDisplayETDReportsDTO reportsDTO, bool lVDifferentCapacity, decimal capacity1, bool terReducedCapacity, decimal capacity2, string claveIdioma, string overload, string typeTest, ref Workbook workbook);

        void PrepareIndexOfETD(ResultETDTestsDTO resultETDTestsDTO, SettingsToDisplayETDReportsDTO reportsDTO, ref Workbook workbook, string idioma);

        DateTime GetDate(Workbook origin, SettingsToDisplayETDReportsDTO reportsDTO);

        bool Verify_ETD_ColumnsToCalculate(SettingsToDisplayETDReportsDTO reportsDTO, Workbook workbook);

        bool Verify_ETD_Columns(SettingsToDisplayETDReportsDTO reportsDTO, Workbook workbook);

        string PrepareDownloadTemplate_ETD(SettingsToDisplayETDReportsDTO reportsDTO, Dictionary<string, ParamETD> parameters, ref Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook);

        ETDUploadResultDTO PrepareUploadConfiguration_ETD(SettingsToDisplayETDReportsDTO reportsDTO, List<bool> listHojas, Workbook workbook, string claveIdioma);
    }
}
