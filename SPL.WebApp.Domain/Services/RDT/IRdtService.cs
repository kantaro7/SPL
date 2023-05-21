namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IRdtService
    {
        void PrepareTemplate_RDT(SettingsToDisplayRDTReportsDTO reportsDTO, ref Workbook workbook, string angularDisplacement, string testType, string posAT, string posBT, string posTer, int ConexionSp);

        void Prepare_RDT_Test(string testType, string aT, string bT, string ter, GeneralPropertiesDTO angular, SettingsToDisplayRDTReportsDTO reportsDTO, Workbook workbook, List<PlateTensionDTO> plateTensions, ref RDTTestsDTO _rdtTestDTO);

        void PrepareIndexOfRDT(ResultRDTTestsDetailsDTO resultRDTTestsDetailsDTO, SettingsToDisplayRDTReportsDTO reportsDTO, string idioma, ref Workbook workbook);

        void CloneWorkbook(Workbook origin, SettingsToDisplayRDTReportsDTO reportsDTO, ref Workbook official, out DateTime date);

        bool Verify_RDT_Columns(SettingsToDisplayRDTReportsDTO reportsDTO, Workbook workbook);
    }
}
