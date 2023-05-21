namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;

    using Telerik.Web.Spreadsheet;

    public class RctService : IRctService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_RCT(SettingsToDisplayRCTReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma, int cols, string unit, decimal? testVoltage, PositionsDTO positionsDTO, string clavePrueba)
        {
            try
            {
                int pos = clavePrueba switch
                {
                    "ATI" => 1,
                    "ABS" => 2,
                    "ABI" => 2,
                    "TSI" => 3,
                    "TOS" => 3,
                    "TOI" => 3,
                    "TIS" => 3,
                    _ => 0,
                };

                string positionString = pos == 1 ? string.Join(',', positionsDTO.AltaTension) : pos == 2 ? string.Join(',', positionsDTO.AltaTension.Concat(positionsDTO.BajaTension).Distinct()) : string.Join(',', positionsDTO.AltaTension.Concat(positionsDTO.BajaTension.Concat(positionsDTO.Terciario)).Distinct());

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

                // Tension de prueba
                _positionWB = GetRowColOfWorbook("B11");
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = testVoltage is null ? "" : claveIdioma.Equals("ES") ? $"Tensión de Prueba {testVoltage} KV" : $"Test Voltage {testVoltage} KV";

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

                #region Tops
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                List<string> measures = new List<string>();
                foreach (int item in Enum.GetValues(typeof(MeasuringResistance)))
                {
                    measures.Add($"{myTI.ToTitleCase(Enum.GetName(typeof(MeasuringResistance), item).ToLower())}");
                }
                IEnumerable<ConfigurationReportsDTO> configTemp = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Unidades"));
                foreach (ConfigurationReportsDTO item in configTemp)
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);

                    for (int i = 0; i < cols; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Value = unit;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Validation = new Validation()
                        {
                            DataType = "list",
                            ShowButton = true,
                            ComparerType = "list",
                            From = "\"" + String.Join(",", measures) + "\"",
                            AllowNulls = false,
                            Type = "reject"
                        };
                    }
                }
                #endregion

                #region DataFill
                

                // Terminales
                IEnumerable<ConfigurationReportsDTO> terminals = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Terminales"));
                foreach (ConfigurationReportsDTO cell in terminals)
                {
                    _positionWB = GetRowColOfWorbook(cell.Celda);
                    for (int i = 0; i < cols; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Enable = true;
                        
                    }
                }

                // Posiciones
                IEnumerable<ConfigurationReportsDTO> positions = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Posicion"));
                foreach (ConfigurationReportsDTO cell in positions)
                {
                    _positionWB = GetRowColOfWorbook(cell.Celda);
                    for (int i = 0; i < cols; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Value = "Seleccione...";
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Validation = new Validation()
                        {
                            DataType = "list",
                            ShowButton = true,
                            ComparerType = "list",
                            From = "\""+positionString+"\"",
                            AllowNulls = false,
                            Type = "reject"
                        };
                    }
                }

                // Resistencias
                IEnumerable<ConfigurationReportsDTO> resists = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Resistencia"));
                foreach (ConfigurationReportsDTO cell in resists)
                {
                    _positionWB = GetRowColOfWorbook(cell.Celda);
                    for (int i = 0; i < cols; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Format = "##,##0.000000";
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "99999.999999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };
                    }
                }

                // Tempreratura
                IEnumerable<ConfigurationReportsDTO> temps = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura"));
                foreach (ConfigurationReportsDTO cell in temps)
                {
                    _positionWB = GetRowColOfWorbook(cell.Celda);
                    for (int i = 0; i < cols; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Format = "##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999.9",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };
                    }
                }
                #endregion
            }
            catch (Exception ex) {
                throw;
            }
        }

        public void Prepare_RCT_Test(SettingsToDisplayRCTReportsDTO reportsDTO, Workbook workbook, ref List<RCTTestsDTO> _rctTestDTOs, int cols)
        {
            #region Reading Columns
            int[] _positionWB;
            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fase"));

            int section = 0;
            foreach (ConfigurationReportsDTO report in celdas)
            {
                _positionWB = GetRowColOfWorbook(report.Celda);
                for (int i = 0; i < cols; i++)
                {
                    _rctTestDTOs[section].RCTTestsDetails.Add(new RCTTestsDetailsDTO()
                    {
                        Phase = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Value.ToString(),
                        Terminal = workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + i].Value.ToString(),
                        Position = workbook.Sheets[0].Rows[_positionWB[0] + 2].Cells[_positionWB[1] + i].Value.ToString(),
                        Resistencia = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 3].Cells[_positionWB[1] + i].Value.ToString()),
                        Unit = workbook.Sheets[0].Rows[_positionWB[0] + 4].Cells[_positionWB[1] + i].Value.ToString(),
                        Temperature = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 5].Cells[_positionWB[1] + i].Value.ToString())
                    });
                }
                section++;
            }
            #endregion
        }

        public void PrepareIndexOfRCT(ResultRCTTestsDTO resultRODTestsDTO, SettingsToDisplayRCTReportsDTO reportsDTO, string idioma, ref Workbook workbook)
        {
            int[] resultLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            bool resultReport = !resultRODTestsDTO.Results.Any();

            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayRCTReportsDTO reportsDTO)
        {
            DateTime date;
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = GetRowColOfWorbook(fechaCelda.Celda);
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

            if (DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
            {
                date = objDT;
              
            }
            else
            {
                double value = double.Parse(fecha);
                DateTime date1 = DateTime.FromOADate(value);
                date = date1;
            }
           
            return date;
        }

        public string GetMeasurementType(Workbook origin, SettingsToDisplayRCTReportsDTO reportsDTO)
        {
            int[] _positionWB = GetRowColOfWorbook("D37");
            return origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
        }

        public bool Verify_RCT_Columns(SettingsToDisplayRCTReportsDTO reportsDTO, Workbook workbook, int cols)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Terminales"));
            int[] _positionWB;
            int section = 0;
            foreach (ConfigurationReportsDTO item in starts)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                for (int i = 0; i < cols; i++)
                {
                    int rowsReaded = 0;
                    string cell = "NOM";
                    int count = _positionWB[0];
                    while (cell is not "" and not null)
                    {
                        cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1] + i].Value?.ToString();
                        if (cell is not "" and not null)
                        {
                            rowsReaded++;
                        }
                        count++;
                    }
                    if(rowsReaded != 5)
                    {
                        return false;
                    }
                }
                section++;
            }

            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura") || c.Dato.Equals("Fecha") || c.Dato.Equals("Resultado"));

            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                if(string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        #region Private Methods
        private static void CopyColumn(Workbook origin, int[] position, ref Workbook official)
        {
            string cell = "NOM";
            int count = position[0];
            while (cell is not "" and not null)
            {
                cell = origin.Sheets[0].Rows[count].Cells[position[1]].Value?.ToString();
                if (cell is not "" and not null)
                {
                    official.Sheets[0].Rows[count].Cells[position[1]].Value = cell;
                }
                count++;
            }
        }
        private static int[] GetRowColOfWorbook(string cell)
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
