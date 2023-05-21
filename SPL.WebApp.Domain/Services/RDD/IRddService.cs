namespace SPL.WebApp.Domain.Services
{
    using System;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IRddService
    {
        void PrepareTemplate_RDD(SettingsToDisplayRDDReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma, PositionsDTO positions);

        void Prepare_RDD_Test(SettingsToDisplayRDDReportsDTO reportsDTO, Workbook workbook, ref RDDTestsGeneralDTO rddTestsGeneralDTO);

        void PrepareIndexOfRDD(ResultRDDTestsDTO resultRDDTestsDTO, SettingsToDisplayRDDReportsDTO reportsDTO, string idioma, ref Workbook workbook);

        string Verify_RDD_Columns(SettingsToDisplayRDDReportsDTO reportsDTO, Workbook workbook);

        DateTime GetDate(SettingsToDisplayRDDReportsDTO reportsDTO, Workbook workbook);
    }
}
