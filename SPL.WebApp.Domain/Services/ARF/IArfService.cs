namespace SPL.WebApp.Domain.Services
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using Telerik.Web.Spreadsheet;

    public interface IArfService
    {
        void PrepareTemplate(ref SettingsToDisplayARFReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje, string voltageLevels, string team, int columnas, string ter2da, 
            ref string nivelAceiteLab, ref string nivelAceitePla, ref string boquillasLab, ref string boquillasPla, ref string nucleoLab, ref string nucleoPla, ref string terciarioLab, ref string terciarioPla);
        string ValidateTemplateTDP(SettingsToDisplayTDPReportsDTO reportsDTO, Workbook workbook);

    }
}
