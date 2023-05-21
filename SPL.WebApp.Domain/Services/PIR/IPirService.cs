namespace SPL.WebApp.Domain.Services
{
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IPirService
    {
        void PrepareTemplate_Pir (SettingsToDisplayPIRReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba, string idioma, DerivationsDTO derivationsDTO, string connectionAt, string connectionBt, string connectionTer, ref int count);

    }
}
