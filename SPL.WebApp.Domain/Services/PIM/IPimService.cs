namespace SPL.WebApp.Domain.Services
{
    using System;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IPimService
    {
        void PrepareTemplate_PIM(SettingsToDisplayPIMReportsDTO reportsDTO, ref Workbook workbook);

        void Prepare_PIM_Test(SettingsToDisplayPIMReportsDTO reportsDTO, Workbook workbook, ref PIMTestsGeneralDTO _prdTestDTO);

        DateTime GetDate(Workbook origin, SettingsToDisplayPIMReportsDTO reportsDTO);

        bool Verify_PIM_Columns(SettingsToDisplayPIMReportsDTO reportsDTO, Workbook workbook);
    }
}
