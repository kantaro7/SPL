namespace SPL.WebApp.Domain.Services.Imp.TAP
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Helpers;

    using Telerik.Web.Spreadsheet;

    public class TapService : ITapService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_TAP(SettingsToDisplayTAPReportsDTO reportsDTO, List<EstructuraReporte> reporte, ref Workbook workbook)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));

                #endregion

                #region Head

                int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "MM/dd/yyyy";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "date",
                    AllowNulls = false,
                    MessageTemplate = $"Fecha es requerida y debe estar dentro del rango 1/1/1900 - {DateTime.Now:MM/dd/yyyy}",
                    ComparerType = "between",
                    From = "DATEVALUE(\"1/1/1900\")",
                    To = $"DATEVALUE(\"{DateTime.Now:MM/dd/yyyy}\")",
                    Type = "reject",
                    TitleTemplate = "Error",
                    ShowButton = true
                };
                #endregion

                #region DataFill
                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };
                // FrequencyTest
                _positionWB = GetRowColOfWorbook("D10");
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.FrequencyTest;

                // Tabla
                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEnergizado")).Celda);

                reporte.RemoveAll(x => x.DevanadoEnergizado == null || x.DevanadoAterrizado==null);

                for (int i = 0; i < reporte.Count(); i++)
                {
                    // Devanado Energizado
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = reporte[i].DevanadoEnergizado;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontSize = 8;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        ComparerType = "lessThan",
                        From = "AND(LEN(C" + (_positionWB[0] + i + 1) + ") <=10)",
                        AllowNulls = false,
                        MessageTemplate = "Devanado evergizado no puede exceder 10 caracteres",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };

                    // Devanado Aterrizado
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value = reporte[i].DevanadoAterrizado;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].FontSize = 8;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Validation = new Validation()
                    {
                        DataType = "custom",
                        ComparerType = "lessThan",
                        From = "AND(LEN(D" + (_positionWB[0] + i + 1) + ") <=10)",
                        AllowNulls = false,
                        MessageTemplate = "Devanado aterrizado no puede exceder 10 caracteres",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };

                    // Nivel de Tension KV
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Format = "###,##0.000";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].FontSize = 8;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999999",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };

                    // Tension Aplicada KV
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Format = "###,##0.000";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].FontSize = 8;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999999",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };

                    // Corriente
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Format = "###,##0.000";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].FontSize = 8;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999999",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };

                    // Tiempo
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Format = "0";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].FontSize = 8;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Tiempo  debe ser mayor a cero considerando 2 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };
                }

                string finals = reportsDTO.BaseTemplate.ClaveIdioma.ToUpper().Equals("ES") ?
                       "Aceptado,Rechazado" : "Accepted,Rejected";
                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "Seleccione...";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = "\"" + finals + "\"",
                    AllowNulls = false,
                    Type = "reject"
                };

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Verify_TAP_ColumnsToCalculate(SettingsToDisplayTAPReportsDTO reportsDTO, Workbook workbook)
        {
            ConfigurationReportsDTO starts = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEnergizado"));
            int[] _positionWB = GetRowColOfWorbook(starts.Celda);
            for (int i = -1; i < reportsDTO.EstructuraReportes.Count() - 1; i++)
            {
                string cell = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString();
                string cell2 = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value?.ToString();
                string cell3 = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Value?.ToString();
                string cell4 = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value?.ToString();
                string cell5 = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Value?.ToString();
                string cell6 = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Value?.ToString();
                if (cell is null or "" || cell2 is null or "" || cell3 is null or "" || cell4 is null or "" || cell5 is null or "" || cell6 is null or "")
                    return false;
            }

            return true;
        }

        public void Prepare_TAP_Test(SettingsToDisplayTAPReportsDTO reportsDTO, Workbook workbook, ref TAPTestsDTO _tapTestDTO)
        {
            ConfigurationReportsDTO starts = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEnergizado"));
            int[] _positionWB = GetRowColOfWorbook(starts.Celda);
            int count = 0;
            foreach (var item in reportsDTO.EstructuraReportes)
            {
                _tapTestDTO.TAPTestsDetails.Add(new TAPTestsDetailsDTO
                {
                    WindingEnergized = workbook.Sheets[0].Rows[_positionWB[0] + count].Cells[_positionWB[1]].Value?.ToString(),
                    WindingGrounded = workbook.Sheets[0].Rows[_positionWB[0] + count].Cells[_positionWB[1] + 1].Value?.ToString(),
                    VoltageLevel = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + count].Cells[_positionWB[1] + 2].Value?.ToString()),
                    AppliedkV = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + count].Cells[_positionWB[1] + 3].Value?.ToString()),
                    CurrentkV = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + count].Cells[_positionWB[1] + 4].Value?.ToString()),
                    Time = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + count].Cells[_positionWB[1] + 5].Value?.ToString())
                });
                count++;
            }
        }

        public void PrepareIndexOfTAP(ResultTAPTestsDTO resultTAPTestsDTO, SettingsToDisplayTAPReportsDTO reportsDTO, ref Workbook workbook, string idioma)
        {
            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{reportsDTO.BaseTemplate.ClaveIdioma}")).Formato;
        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayTAPReportsDTO reportsDTO)
        {
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = GetRowColOfWorbook(fechaCelda.Celda);
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            return DateUtils.getDateFromSheet(fecha);
        }

        public bool Verify_TAP_Columns(SettingsToDisplayTAPReportsDTO reportsDTO, Workbook workbook)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("PorcZ") || c.Dato.Equals("PerdidasVacio") || c.Dato.Equals("PerdidasCarga") || c.Dato.Equals("PerdidasEnf") || c.Dato.Equals("PerdidasTotales") || c.Dato.Equals("Porc_Z") || c.Dato.Equals("PorcX") || c.Dato.Equals("PorcR") || c.Dato.Equals("XentreR"));
            int[] _positionWB;
            foreach (ConfigurationReportsDTO item in starts)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                string cell = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                if (cell is null or "")
                    return false;
            }

            return true;
        }

        #region Private Methods
        private void CopyColumn(Workbook origin, int[] position, ref Workbook official)
        {
            string cell = "NOM";
            int count = position[0];
            while (cell is not "" and not null)
            {
                cell = origin.Sheets[0].Rows[count].Cells[position[1]].Value?.ToString();
                if (cell is not "" and not null)
                    official.Sheets[0].Rows[count].Cells[position[1]].Value = cell;
                count++;
            }
        }
        private int[] GetRowColOfWorbook(string cell)
        {
            int[] position = new int[2];
            string row = string.Empty, col = string.Empty;

            for (int i = 0; i < cell.Length; i++)
            {
                if (char.IsDigit(cell[i]))
                {
                    col += cell[i];
                }
                else
                {
                    row += cell[i];
                }
            }

            position[0] = Convert.ToInt32(col);

            for (int i = 0; i < row.Length; i++)
            {
                position[1] += char.ToUpper(row[i]) - 64;
            }

            position[0] += -1;
            position[1] += -1;

            return position;
        }
        #endregion

    }
}
