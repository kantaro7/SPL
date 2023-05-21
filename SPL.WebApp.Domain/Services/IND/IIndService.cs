namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using Telerik.Web.Spreadsheet;

    public interface IIndService
    {
        void PrepareTemplate( SettingsToDisplayINDReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje, string tcBuyers);
        string ValidateTemplateIND(SettingsToDisplayINDReportsDTO reportsDTO, Workbook workbook, string keyTest , string tieneTC, ref List<INDTestsDetailsDTO> arreglo,ref int totalPag, ref string anexo);

    }
}
