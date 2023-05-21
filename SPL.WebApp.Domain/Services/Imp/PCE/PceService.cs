namespace SPL.WebApp.Domain.Services.Imp.PCE
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class PceService : IPceService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_PCE(SettingsToDisplayPCEReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma, int inicio, int fin, int intervalo, string keyTest)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion
                BorderStyle boderW = new()
                {
                    Color = "White",
                    Size = 1
                };
                #region Head

                int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";
                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("CapMinima")))
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.CapMinima;
                }

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

                if (keyTest == "AYD")
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha") && c.Seccion == 2).Celda);
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

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Pos_AT")))
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Pos_AT;
                }

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Pos_BT")))
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Pos_BT;
                }

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Pos_Ter")))
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    if (string.IsNullOrEmpty(reportsDTO.Pos_TER) || string.IsNullOrWhiteSpace(reportsDTO.Pos_TER))
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.Empty;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].BorderBottom = boderW;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Value = string.Empty;
                    }
                    else
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Pos_TER;
                    }
                }
                string[] voltagesBase = new string[5];
                int count = 0;
                foreach (string item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("VoltajeBase")).Select(x => x.Celda))
                {
                    voltagesBase[count] = item;
                    _positionWB = GetRowColOfWorbook(item);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.VoltajeBase;
                    count++;
                }

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Frecuencia")))
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Frecuencia;
                }

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Gar_Perdidas")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Gar_Perdidas;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Gar_Cexcitacion")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Gar_Cexcitacion;
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

                #region DataFill
                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };
                int section = 0;
                IEnumerable<ConfigurationReportsDTO> startPos = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Porc_Vn"));
                int row = 0;
                int renglones = (fin - inicio) / intervalo;
                foreach (ConfigurationReportsDTO cell in startPos)
                {
                    row = 0;
                    _positionWB = GetRowColOfWorbook(cell.Celda);
                    for (int vn = inicio; vn <= fin; vn += intervalo)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1]].Value = vn + "%";
                        workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 1].Formula = $"=ROUNDDOWN({GetCellOfWorbook(_positionWB[0] + row, _positionWB[1])}*{voltagesBase[section]}, 3)";
                        workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1]].Enable = row + 2 >= renglones;

                        for (int j = 0; j < 9; j++)
                        {
                            try
                            {
                                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + j].Enable = j is 0 ? row + 4 >= renglones : j is > 1 and < 6;
                                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + j].BorderTop = boder;
                                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + j].BorderRight = boder;
                                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + j].BorderBottom = boder;
                                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + j].BorderLeft = boder;
                                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + j].Format = "#,##0.000";
                                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + j].FontFamily = "Arial Unicode MS";
                                if (j is > 1 and < 6)
                                {
                                    workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + j].Validation = new Validation()
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
                            catch (Exception)
                            {
                            }
                        }
                        row++;
                    }
                    section++;

                }

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CorrienteIrms")).Celda);
                for (int i = 0; i <= renglones; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "#,##0.000";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                }

                if (keyTest == "AYD")
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CorrienteIrms") && c.Seccion == 2).Celda);
                    for (int i = 0; i <= renglones; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderTop = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "#,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Prepare_PCE_Test(SettingsToDisplayPCEReportsDTO reportsDTO, Workbook workbook, ref List<PCETestsDTO> _pceTestDTOs)
        {
            #region Reading Columns
            int[] _positionWB;
            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Porc_Vn"));

            int section = 0;
            foreach (ConfigurationReportsDTO report in celdas)
            {
                _positionWB = GetRowColOfWorbook(report.Celda);
                long cantPositions = ((_pceTestDTOs[section].IFin - _pceTestDTOs[section].IInicio) / _pceTestDTOs[section].Intervalo) + 1;
                for (int i = 0; i < cantPositions; i++)
                {
                    _pceTestDTOs[section].PCETestsDetails.Add(new PCETestsDetailsDTO()
                    {
                        PorcentajeVN = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value.ToString().Replace("%", string.Empty)),
                        NominalKV = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value.ToString()),
                        PerdidasKW = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Value.ToString()),
                        CorrienteIRMS = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value.ToString()),
                        TensionKVRMS = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Value.ToString()),
                        TensionKVAVG = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Value.ToString()),

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
                _pceTestDTOs[section].Temperatura = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());
                _pceTestDTOs[section].UmTemperatura = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value?.ToString();

                section++;
            }

            // Unidad Medida Capacidad Minima
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("CapMinima"));
            section = 0;
            foreach (ConfigurationReportsDTO item in celdas)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                _pceTestDTOs[section].Capacidad = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());
                section++;
            }

            // Unidad Medida Capacidad Minima
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMCapMin"));
            section = 0;
            foreach (ConfigurationReportsDTO item in celdas)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                _pceTestDTOs[section].UmCapacidad = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                section++;
            }

            // Unidad Medida Voltaje Base
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMVoltajeBase"));
            section = 0;
            foreach (ConfigurationReportsDTO item in celdas)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                _pceTestDTOs[section].UmVoltajeBase = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                section++;
            }

            // Unidad Medida Frecuencia
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMFrec"));
            section = 0;
            foreach (ConfigurationReportsDTO item in celdas)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                _pceTestDTOs[section].UmFrecuencia = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                section++;
            }

            // Unidad Medida Garatntia perdida
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMGarPerdidas"));
            section = 0;
            foreach (ConfigurationReportsDTO item in celdas)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                _pceTestDTOs[section].UmGarPerdidas = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                section++;
            }

            // Unidad Medida Garantia excitacion
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMGarCExcitacion"));
            section = 0;
            foreach (ConfigurationReportsDTO item in celdas)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                _pceTestDTOs[section].UmGarCExcitacion = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                section++;
            }
        }

        public void PrepareIndexOfPCE(ResultPCETestsDTO resultPCETestsDTO, SettingsToDisplayPCEReportsDTO reportsDTO, string idioma, ref Workbook workbook)
        {
            int section = 0;
            IEnumerable<ConfigurationReportsDTO> columna = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("PerdidasOnda"));
            foreach (ConfigurationReportsDTO item in columna)
            {
                int[] init = GetRowColOfWorbook(item.Celda);
                int row = 0;
                foreach (PCETestsDetailsDTO pceTestsDetailsDTO in resultPCETestsDTO.PCETests[section].PCETestsDetails)
                {
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1]].Value = decimal.Round(pceTestsDetailsDTO.PerdidasOndaKW, 3).ToString();
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1] + 1].Value = decimal.Round(pceTestsDetailsDTO.Corregidas20KW, 3).ToString();
                    workbook.Sheets[0].Rows[init[0] + row].Cells[init[1] + 2].Value = decimal.Round(pceTestsDetailsDTO.PorcentajeIexc, 3).ToString();
                    row++;
                }
                section++;
            }

            int[] resultLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            bool resultReport = !resultPCETestsDTO.Results.Any(x => x.Column != 77 && x.Fila != 77);

            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
        }

        public List<DateTime> GetDate(Workbook origin, SettingsToDisplayPCEReportsDTO reportsDTO)
        {
            List<DateTime> dates = new();
            IEnumerable<ConfigurationReportsDTO> fechaCelda = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));
            foreach (ConfigurationReportsDTO item in fechaCelda)
            {
                int[] _positionWB = GetRowColOfWorbook(item.Celda);
                string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

                if (DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
                {
                    dates.Add(objDT);
                }
                else
                {
                    double value = double.Parse(fecha);
                    DateTime date1 = DateTime.FromOADate(value);
                    dates.Add(date1);
                }
            }

            return dates;
        }

        public bool Verify_PCE_Columns(SettingsToDisplayPCEReportsDTO reportsDTO, Workbook workbook, int renglones)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("PerdidasKW"));
            int[] _positionWB;
            int section = 0;
            foreach (ConfigurationReportsDTO item in starts)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                for (int i = 0; i < 4; i++)
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
                    if (rowsReaded != renglones)
                        return false;
                }
                section++;
            }

            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura") || c.Dato.Equals("Fecha"));

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

        private string GetCellOfWorbook(int row, int col) => $"{Convert.ToChar(col + 65)}{row + 1}";
        #endregion

    }
}
