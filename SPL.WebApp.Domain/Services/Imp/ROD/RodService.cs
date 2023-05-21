namespace SPL.WebApp.Domain.Services.Imp.ROD
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class RodService : IRodService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        private string GetCellAddress(int row, int col)
        {
            string[] arr = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

            string r = arr[col] + (row + 1).ToString();

            return r;
        }
        public void PrepareTemplate_ROD(SettingsToDisplayRODReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma, ref List<string> celdas, string unidad)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion

                #region Head

                int[] _positionWB;

                for (int i = 1; i <= (int)reportsDTO.BaseTemplate.ColumnasConfigurables; i++)
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente") && c.Seccion == i).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie") && c.Seccion == i).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad") && c.Seccion == i).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTensionPrueba") && c.Seccion == i).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.TestVoltage;

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha") && c.Seccion == i).Celda);
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
                }

                #endregion

                #region Tops
                IEnumerable<ConfigurationReportsDTO> configTemp = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura"));
                foreach (ConfigurationReportsDTO item in configTemp)
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#0.0";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "99.9",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };
                }
                #endregion

                #region TittleColumns
                int section = 0;
                configTemp = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temp_SE"));
                foreach (ConfigurationReportsDTO item in configTemp)
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"{(claveIdioma.Equals("ES") ? "a" : "to")} {reportsDTO.TitleOfColumns[section].TempSE} °C";
                    section++;
                }

                section = 0;
                IEnumerable<ConfigurationReportsDTO> titTerms = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Tit_Term_1"));
                foreach (ConfigurationReportsDTO item in titTerms)
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.TitleOfColumns[section].Terminal1;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = reportsDTO.TitleOfColumns[section].Terminal2;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = reportsDTO.TitleOfColumns[section].Terminal3;
                    section++;
                }
                #endregion

                #region DataFill
                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };
                BorderStyle boderW = new()
                {
                    Color = "White",
                    Size = 1
                };
                section = 0;
                IEnumerable<ConfigurationReportsDTO> startPos = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Posicion"));

                int row = 0;
                int celda = 0;
                foreach (ConfigurationReportsDTO cell in startPos)
                {

                    _positionWB = GetRowColOfWorbook(cell.Celda);

                    if (reportsDTO.TitleOfColumns[section].Positions.Count == 1)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.TitleOfColumns[section].Positions[0];

                        for (int j = 0; j < 8; j++)
                        {
                            try
                            {
                                row = _positionWB[0];
                                celda = _positionWB[1] + j;
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + j].Enable = j is > 0 and < 4;
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + j].BorderTop = boder;
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + j].BorderRight = boder;
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + j].BorderBottom = boder;
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + j].BorderLeft = boder;
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + j].FontFamily = "Arial Unicode MS";
                                workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + j].BorderRight = boderW;
                                workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + j].BorderBottom = boderW;
                                workbook.Sheets[0].Rows[_positionWB[0] + 2].Cells[_positionWB[1] + j].BorderTop = boderW;
                                workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + j].BorderLeft = boderW;
                                if (j is > 0 and < 4)
                                {
                                    string aa = GetCellAddress(row, celda);
                                    celdas.Add(aa);
                                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + j].Format = unidad.ToUpper() == "OHMS" ? "#,##0.000000" : "#,##0.000000";

                                }
                            }
                            catch (Exception)
                            {
                                string msg = $"Fila {row} Columna {celda} i 0 j {j}";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < reportsDTO.TitleOfColumns[section].Positions.Count; i++)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = reportsDTO.TitleOfColumns[section].Positions[i];

                            for (int j = 0; j < 8; j++)
                            {
                                try
                                {
                                    row = _positionWB[0] + i;
                                    celda = _positionWB[1] + j;
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Enable = j is > 0 and < 4;
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderTop = boder;
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderRight = boder;
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderBottom = boder;
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderLeft = boder;
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].FontFamily = "Arial Unicode MS";
                                    if (j is > 0 and < 4)
                                    {
                                        string aa = GetCellAddress(row, celda);
                                        celdas.Add(aa);
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Format = unidad.ToUpper() == "OHMS" ? "#,##0.000000" : "#,##0.000000";

                                    }
                                }
                                catch (Exception)
                                {
                                    string msg = $"Fila {row} Columna {celda} i {i} j {j}";
                                }
                            }
                        }
                    }

                    section++;
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool VerifyPrepare_ROD_Test(SettingsToDisplayRODReportsDTO reportsDTO, Workbook workbook, ref List<RODTestsDTO> _rodTestDTOs)
        {
            #region Reading Columns
            int[] _positionWB;
            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Posicion"));

            int section = 0;
            foreach (ConfigurationReportsDTO report in celdas)
            {
                _positionWB = GetRowColOfWorbook(report.Celda);
                int cantPositions = reportsDTO.TitleOfColumns[section].Positions.Count();
                for (int i = 0; i < cantPositions; i++)
                {
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString()) ||
                        string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value?.ToString()) ||
                        string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Value?.ToString()) ||
                        string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value?.ToString()))
                    {
                        return false;
                    }
                }
                section++;
            }
            #endregion

            // Obteniendo temperaturas
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura"));
            section = 0;
            foreach (ConfigurationReportsDTO item in celdas)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
                    return false;
                section++;
            }
            return true;
        }

        public void Prepare_ROD_Test(SettingsToDisplayRODReportsDTO reportsDTO, Workbook workbook, ref List<RODTestsDTO> _rodTestDTOs)
        {
            #region Reading Columns
            int[] _positionWB;
            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Posicion"));

            int section = 0;
            foreach (ConfigurationReportsDTO report in celdas)
            {
                _positionWB = GetRowColOfWorbook(report.Celda);
                int cantPositions = reportsDTO.TitleOfColumns[section].Positions.Count();
                for (int i = 0; i < cantPositions; i++)
                {
                    _rodTestDTOs[section].RODTestsDetails.Add(new RODTestsDetailsDTO()
                    {
                        Position = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value.ToString(),
                        TerminalH1 = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value is null ? 0 : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value.ToString()),
                        TerminalH2 = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value is null ? 0 : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Value.ToString()),
                        TerminalH3 = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value is null ? 0 : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value.ToString())
                    });
                }
                section++;
            }
            #endregion

            // Obteniendo temperaturas
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura"));
            section = 0;
            foreach (ConfigurationReportsDTO item in celdas)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                _rodTestDTOs[section].Temperature = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

                section++;
            }
        }

        public void PrepareIndexOfROD(ResultRODTestsDTO resultRODTestsDTO, SettingsToDisplayRODReportsDTO reportsDTO, string idioma, ref Workbook workbook)
        {
            int section = 0;
            IEnumerable<ConfigurationReportsDTO> columna = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Resist_Prom"));
            foreach (ConfigurationReportsDTO item in columna)
            {
                int[] init = GetRowColOfWorbook(item.Celda);
                int row = 0;
                foreach (RODTestsDetailsDTO rodTestsDetailsDTO in resultRODTestsDTO.RODTests[section].RODTestsDetails)
                {
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1]].Value = decimal.Round(rodTestsDetailsDTO.AverageResistance, 4).ToString();
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1]].Format = "#,##0.0000";
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1] + 1].Value = decimal.Round(rodTestsDetailsDTO.PercentageA, 4).ToString();
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1] + 1].Format = "#,##0.0000";
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1] + 2].Value = decimal.Round(rodTestsDetailsDTO.PercentageB, 4).ToString();
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1] + 2].Format = "#,##0.0000";
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1] + 3].Value = decimal.Round(rodTestsDetailsDTO.Desv, 2).ToString();
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1] + 3].Format = "##0.00";
                    row++;
                }
                section++;
            }

            for (int i = 1; i <= (int)reportsDTO.BaseTemplate.ColumnasConfigurables; i++)
            {
                int[] resultLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado") && c.Seccion == i).Celda);
                bool resultReport = !resultRODTestsDTO.Results.Any();

                workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?
                    reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                    reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
            }
        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayRODReportsDTO reportsDTO)
        {
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = GetRowColOfWorbook(fechaCelda.Celda);
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            if (DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
            {
                return objDT;
            }
            else
            {
                double value = double.Parse(fecha);
                DateTime date1 = DateTime.FromOADate(value);
                return date1;
            }
        }

        public bool Verify_ROD_Columns(SettingsToDisplayRODReportsDTO reportsDTO, Workbook workbook, int rows1, int rows2, int rows3)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Posicion"));
            int[] _positionWB;
            int section = 0;
            foreach (ConfigurationReportsDTO item in starts)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                for (int i = 0; i < 8; i++)
                {
                    int rowsReaded = 0;
                    string cell = "NOM";
                    int count = _positionWB[0];
                    while (cell is not "" and not null)
                    {
                        cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1] + i].Value?.ToString();
                        if (cell is not "" and not null)
                            rowsReaded++;
                        count++;
                    }
                    if (rowsReaded != (section == 0 ? rows1 : section == 1 ? rows2 : rows3))
                        return false;
                }
                section++;
            }

            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura") || c.Dato.Equals("Fecha") || c.Dato.Equals("Resultado"));

            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
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
