namespace SPL.WebApp.Domain.Services.Imp.PEE
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class PeeService : IPeeService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_PEE(SettingsToDisplayPEEReportsDTO reportsDTO, ref Workbook workbook)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));

                #endregion

                #region Head

                IEnumerable<string> aux = reportsDTO.InfotmationArtifact.CharacteristicsArtifact.Where(x => !x.CoolingType.ToUpper().Contains("ONAN") && !x.CoolingType.ToUpper().Contains("OKAN")).Select(x => x.CoolingType.ToUpper());
                List<string> coolingTypes = new();
                if (aux.Where(x => x.Contains("ONAF")).Count() > 1)
                    coolingTypes.AddRange(aux.Where(x => x.Contains("ONAF")));

                if (aux.Where(x => x.Contains("OFAF")).Count() > 1)
                    coolingTypes.AddRange(aux.Where(x => x.Contains("OFAF")));

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
                BorderStyle boderW = new()
                {
                    Color = "White",
                    Size = 1
                };
                // Cooling Type
                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TipoEnf")).Celda);
                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };
                coolingTypes = coolingTypes.Distinct().ToList();

                if (coolingTypes.Count() == 1)
                {
                    // Cooling Type
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = coolingTypes.ElementAt(0);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].BorderRight = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].BorderBottom = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].BorderLeft = boderW;

                    // Voltage
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Format = "###,##0.000";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 1].BorderRight = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 1].BorderBottom = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 1].BorderLeft = boderW;

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Validation = new Validation()
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

                    // Current
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Format = "###,##0.000";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 2].BorderRight = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 2].BorderBottom = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 2].BorderLeft = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Validation = new Validation()
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

                    // Power
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].FontFamily = "Arial Unicode MS";

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Format = "##0.000";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 3].BorderRight = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 3].BorderBottom = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 3].BorderLeft = boderW;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };
                }
                else
                {
                    for (int i = 0; i < coolingTypes.Count(); i++)
                    {
                        // Cooling Type
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = coolingTypes.ElementAt(i);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderTop = boder;

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderLeft = boder;

                        // Voltage
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderTop = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderRight = boder;

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Validation = new Validation()
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

                        // Current
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].BorderTop = boder;

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Enable = true;
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

                        // Power
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].FontFamily = "Arial Unicode MS";

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Format = "##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].BorderTop = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                    }
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

        public bool Verify_PEE_ColumnsToCalculate(SettingsToDisplayPEEReportsDTO reportsDTO, Workbook workbook)
        {
            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TipoEnf")).Celda);
            string coolingType = "Frio";
            int row = 0;
            while (!string.IsNullOrEmpty(coolingType))
            {
                string voltage = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 1].Value?.ToString();
                string current = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 2].Value?.ToString();
                string power = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 3].Value?.ToString();

                if (string.IsNullOrEmpty(voltage) || string.IsNullOrEmpty(current) || string.IsNullOrEmpty(power))
                    return false;

                coolingType = workbook.Sheets[0].Rows[_positionWB[0] + row + 1].Cells[_positionWB[1]].Value?.ToString();
                row++;
            }

            return true;
        }

        public void Prepare_PEE_Test(SettingsToDisplayPEEReportsDTO reportsDTO, Workbook workbook, ref PEETestsDTO _pceTestDTO)
        {
            int[] _positionWB;
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TipoEnf")).Celda);
            string coolingType = "Frio";
            int row = 0;
            while (!string.IsNullOrEmpty(coolingType))
            {
                _pceTestDTO.PEETestsDetails.Add(new PEETestsDetailsDTO()
                {
                    CoolingType = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1]].Value.ToString(),
                    VoltageRMS = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 1].Value.ToString()),
                    CurrentRMS = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 2].Value.ToString()),
                    PowerKW = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 3].Value.ToString()),
                });
                coolingType = workbook.Sheets[0].Rows[_positionWB[0] + row + 1].Cells[_positionWB[1]].Value?.ToString();
                row++;
            }
        }

        public void PrepareIndexOfPEE(ResultPEETestsDTO resultPEETestsDTO, SettingsToDisplayPEEReportsDTO reportsDTO, ref Workbook workbook, string idioma)
        {
            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{reportsDTO.BaseTemplate.ClaveIdioma}")).Formato;

        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayPEEReportsDTO reportsDTO)
        {
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = GetRowColOfWorbook(fechaCelda.Celda);
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            double fechaD = returnDouble(fecha);
            if (fechaD > 0)
            {
                return DateTime.FromOADate(fechaD);
            }
            DateTime date = DateTime.ParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            return date;
        }

        private double returnDouble(string value)
        {
            double valueD = 0;
            try
            {
                valueD = Double.Parse(value);
            }
            catch (Exception e)
            {

            }
            return valueD;
        }

        public bool Verify_PEE_Columns(SettingsToDisplayPEEReportsDTO reportsDTO, Workbook workbook)
        {
            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
            string cell = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            if (cell is null or "")
                return false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            cell = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            if (cell is null or "")
                return false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TipoEnf")).Celda);
            string coolingType = "Frio";
            int row = 0;
            while (!string.IsNullOrEmpty(coolingType))
            {
                string voltage = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 1].Value?.ToString();
                string current = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 2].Value?.ToString();
                string power = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 3].Value?.ToString();

                if (string.IsNullOrEmpty(voltage) || string.IsNullOrEmpty(current) || string.IsNullOrEmpty(power))
                    return false;

                coolingType = workbook.Sheets[0].Rows[_positionWB[0] + row + 1].Cells[_positionWB[1]].Value?.ToString();
                row++;
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
