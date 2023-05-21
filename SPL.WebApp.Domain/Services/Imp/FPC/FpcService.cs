namespace SPL.WebApp.Domain.Services.Imp.FPC
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Services.FPC;

    using Telerik.Web.Spreadsheet;

    public class FpcService : IFpcService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_FPC(SettingsToDisplayFPCReportsDTO reportsDTO, ref Workbook workbook, string specification, string idioma)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion

                #region Head

                int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);
                // [3, 2]
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";

                #endregion

                #region Tops

                int section = 0;
                IEnumerable<ConfigurationReportsDTO> configDate = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));
                foreach (ConfigurationReportsDTO item in configDate)
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);

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
                    section++;
                }

                IEnumerable<ConfigurationReportsDTO> configTemp = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempAceiteInf"));

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

                configTemp = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempAceiteSup"));

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

                IEnumerable<ConfigurationReportsDTO> configTension = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TensionPrueba"));

                foreach (ConfigurationReportsDTO item in configTension)
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = 10;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;
                    /* workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                     workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0.0##";
                     workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                     {
                         DataType = "number",
                         ComparerType = "greaterThan",
                         From = "0",
                         To = "999.9",
                         AllowNulls = false,
                         MessageTemplate = $"{messageErrorNumeric}",
                         Type = "reject",
                         TitleTemplate = "Error"
                     };*/
                }

                //IEnumerable<ConfigurationReportsDTO> configUM = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMTension"));

                //foreach (ConfigurationReportsDTO item in configUM)
                //{
                //    _positionWB = this.GetRowColOfWorbook(item.Celda);

                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Validation = new Validation()
                //    {
                //        AllowNulls = false,
                //        MessageTemplate = $"El valor de unidad de medida es requerido",
                //        Type = "reject",
                //        TitleTemplate = "Error"
                //    };
                //}

                //configUM = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMTempSup"));

                //foreach (ConfigurationReportsDTO item in configUM)
                //{
                //    _positionWB = this.GetRowColOfWorbook(item.Celda);

                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Validation = new Validation()
                //    {
                //        AllowNulls = false,
                //        MessageTemplate = $"El valor de unidad de medida es requerido",
                //        Type = "reject",
                //        TitleTemplate = "Error"
                //    };
                //}

                //configUM = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMTempInf"));

                //foreach (ConfigurationReportsDTO item in configUM)
                //{
                //    _positionWB = this.GetRowColOfWorbook(item.Celda);

                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 3].Enable = true;
                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 3].Validation = new Validation()
                //    {
                //        AllowNulls = false,
                //        MessageTemplate = $"El valor de unidad de medida es requerido",
                //        Type = "reject",
                //        TitleTemplate = "Error"
                //    };
                //}
                #endregion

                #region TittleColumns
                IEnumerable<ConfigurationReportsDTO> titColumna9 = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna9"));

                foreach (ConfigurationReportsDTO item in titColumna9)
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = idioma is "ES" ? specification.Equals("Doble") ? "%FP" : "%Tan d" : (object)(specification.Equals("Doble") ? "%PF" : "%Tan d");

                }
                #endregion

                #region DataFill
                List<ColumnTitleFPCReportsDTO> columnas = reportsDTO.TitleOfColumns.OrderBy(x => x.Renglon).ToList();
                IEnumerable<ConfigurationReportsDTO> startE = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna1"));
                IEnumerable<ConfigurationReportsDTO> startT = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna2"));
                IEnumerable<ConfigurationReportsDTO> startG = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna3"));
                IEnumerable<ConfigurationReportsDTO> startUST = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna4"));
                IEnumerable<ConfigurationReportsDTO> startID = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna5"));
                for (int i = 0; i < columnas.Count(); i++)
                {
                    foreach (ConfigurationReportsDTO cell in startE)
                    {
                        _positionWB = GetRowColOfWorbook(cell.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = columnas[i].Columna1;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Bold = true;
                    }

                    foreach (ConfigurationReportsDTO cell in startT)
                    {
                        _positionWB = GetRowColOfWorbook(cell.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = columnas[i].Columna2;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Bold = true;
                    }

                    foreach (ConfigurationReportsDTO cell in startG)
                    {
                        _positionWB = GetRowColOfWorbook(cell.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = columnas[i].Columna3;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Bold = true;
                    }

                    foreach (ConfigurationReportsDTO cell in startUST)
                    {
                        _positionWB = GetRowColOfWorbook(cell.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = columnas[i].Columna4;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Bold = true;
                    }

                    foreach (ConfigurationReportsDTO cell in startID)
                    {
                        _positionWB = GetRowColOfWorbook(cell.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = columnas[i].Columna5;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Bold = true;
                    }
                }
                #endregion

                #region Columns
                IEnumerable<ConfigurationReportsDTO> startsNumbers = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Corriente"));

                foreach (ConfigurationReportsDTO report in startsNumbers)
                {
                    _positionWB = GetRowColOfWorbook(report.Celda);
                    for (int i = 0; i < columnas.Count(); i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Enable = j is 0 or 1;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Format = j == 4 ? "#,##0.#" : "#,##0.000";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Validation = new Validation()
                            {
                                DataType = "number",
                                ComparerType = "greaterThan",
                                From = "0",
                                To = "999999999",
                                AllowNulls = j is 2 or 3 or 4,
                                MessageTemplate = $"{messageErrorNumeric}",
                                Type = "reject",
                                TitleTemplate = "Error"
                            };
                        }
                    }
                }

                foreach (ConfigurationReportsDTO item in configTension)
                {
                    _positionWB = GetRowColOfWorbook(item.Celda);

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                }
                #endregion

                #region Footer
                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NotaFC")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.NoteFc) ? string.Empty : $"{reportsDTO.HeadboardReport.NoteFc}";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Prepare_FPC_Test(SettingsToDisplayFPCReportsDTO reportsDTO, Workbook workbook, ref List<FPCTestsDTO> _fpcTestDTOs)
        {
            #region Reading Columns
            List<FPCTestsDetailsDTO> details = new();
            int[] _positionWB;
            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Renglon"));

            int section = 0;
            foreach (ConfigurationReportsDTO report in celdas)
            {
                _positionWB = GetRowColOfWorbook(report.Celda);

                string init = "NOM";
                int row = _positionWB[0];
                // inicia 16 pero es 13
                while (init is not "" and not null)
                {
                    init = workbook.Sheets[0].Rows[row].Cells[_positionWB[1]].Value?.ToString();
                    if (init is not "" and not null)
                    {
                        _fpcTestDTOs[section].FPCTestsDetails.Add(new FPCTestsDetailsDTO()
                        {
                            Row = Convert.ToInt32(workbook.Sheets[0].Rows[row].Cells[_positionWB[1]].Value?.ToString()),
                            WindingA = workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 1].Value?.ToString(),
                            WindingB = workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 2].Value?.ToString(),
                            WindingC = workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 3].Value?.ToString(),
                            WindingD = workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 4].Value?.ToString(),
                            Current = Convert.ToDecimal(string.IsNullOrEmpty(workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 5].Value?.ToString()) ? -1 : workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 5].Value?.ToString()),
                            Power = Convert.ToDecimal(string.IsNullOrEmpty(workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 6].Value?.ToString()) ? -1 : workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 6].Value?.ToString()),
                            Id = workbook.Sheets[0].Rows[row].Cells[_positionWB[1] + 10].Value?.ToString()
                        });
                    }
                    row++;
                }
                section++;
            }

            #endregion

            // Obteniendo tensiones
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TensionPrueba"));
            section = 0;
            foreach (ConfigurationReportsDTO tension in celdas)
            {
                _positionWB = GetRowColOfWorbook(tension.Celda);
                _fpcTestDTOs[section].Tension = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());
                section++;
            }

            // Obteniendo Unidad Tensiones
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMTension"));
            section = 0;
            foreach (ConfigurationReportsDTO tension in celdas)
            {
                _positionWB = GetRowColOfWorbook(tension.Celda);
                _fpcTestDTOs[section].UmTension = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                section++;
            }

            // Obteniendo Temperatura Sup
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempAceiteSup"));
            section = 0;
            foreach (ConfigurationReportsDTO tension in celdas)
            {
                _positionWB = GetRowColOfWorbook(tension.Celda);
                _fpcTestDTOs[section].UpperOilTemperature = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());
                section++;
            }

            // Obteniendo Unidad Temperatura Sup
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMTempSup"));
            section = 0;
            foreach (ConfigurationReportsDTO tension in celdas)
            {
                _positionWB = GetRowColOfWorbook(tension.Celda);
                _fpcTestDTOs[section].UmTempacSup = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                section++;
            }

            // Obteniendo Temperatura Inf
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempAceiteInf"));
            section = 0;
            foreach (ConfigurationReportsDTO tension in celdas)
            {
                _positionWB = GetRowColOfWorbook(tension.Celda);
                _fpcTestDTOs[section].LowerOilTemperature = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());
                section++;
            }

            // Obteniendo Unidad Temperatura Inf
            celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMTempInf"));
            section = 0;
            foreach (ConfigurationReportsDTO tension in celdas)
            {
                _positionWB = GetRowColOfWorbook(tension.Celda);
                _fpcTestDTOs[section].UmTempacInf = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                section++;
            }
        }

        public DateTime[] GetDate(Workbook origin, SettingsToDisplayFPCReportsDTO reportsDTO)
        {
            IEnumerable<ConfigurationReportsDTO> fechaCelda = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));
            int count = 0;
            DateTime[] result = new DateTime[2];
            foreach (ConfigurationReportsDTO item in fechaCelda)
            {
                int[] _positionWB = GetRowColOfWorbook(item.Celda);
                string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                result[count] = DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT)
                    ? objDT
                    : DateTime.Now.Date;
            }

            return result;
        }

        public void PrepareIndexOfFPC(ResultFPCTestsDTO resultFPCTestsDTO, SettingsToDisplayFPCReportsDTO reportsDTO, string idioma, ref Workbook workbook)
        {
            IEnumerable<ConfigurationReportsDTO> porcFP = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("PorcFP"));
            int section = 0;
            foreach (ConfigurationReportsDTO item in porcFP)
            {
                int[] init = GetRowColOfWorbook(item.Celda);
                for (int i = 0; i < resultFPCTestsDTO.FPCTests[section].FPCTestsDetails.Count; i++)
                {
                    workbook.Sheets[0].Rows[init[0] + i].Cells[init[1]].Value = resultFPCTestsDTO.FPCTests[section].FPCTestsDetails[i].PercentageA;
                    workbook.Sheets[0].Rows[init[0] + i].Cells[init[1] + 1].Value = resultFPCTestsDTO.FPCTests[section].Specification.Equals("Doble") ? resultFPCTestsDTO.FPCTests[section].FPCTestsDetails[i].PercentageC : resultFPCTestsDTO.FPCTests[section].FPCTestsDetails[i].PercentageB;
                    workbook.Sheets[0].Rows[init[0] + i].Cells[init[1] + 2].Value = decimal.Round(resultFPCTestsDTO.FPCTests[section].FPCTestsDetails[i].Capacitance, 0).ToString();
                }
                section++;
            }

            IEnumerable<ConfigurationReportsDTO> tempFP = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempFP"));
            section = 0;
            foreach (ConfigurationReportsDTO item in tempFP)
            {
                int[] init = GetRowColOfWorbook(item.Celda);
                workbook.Sheets[0].Rows[init[0]].Cells[init[1]].Value = decimal.Round(resultFPCTestsDTO.FPCTests[section].TempFP, 0).ToString() + " °C";
                section++;
            }

            IEnumerable<ConfigurationReportsDTO> tempTanD = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempTanD"));
            section = 0;
            foreach (ConfigurationReportsDTO item in tempTanD)
            {
                int[] init = GetRowColOfWorbook(item.Celda);
                workbook.Sheets[0].Rows[init[0]].Cells[init[1]].Value = decimal.Round(resultFPCTestsDTO.FPCTests[section].TempTanD, 0).ToString() + " °C";
                section++;
            }

            int[] resultLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            bool resultReport = !resultFPCTestsDTO.Results.Any();

            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
        }

        public void CloneWorkbook(Workbook origin, SettingsToDisplayFPCReportsDTO reportsDTO, ref Workbook official, out List<DateTime> dates)
        {
            dates = new List<DateTime>();
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Corriente"));
            int[] _positionWB;

            // Copiando Todas las columnas
            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                for (int i = 0; i < 5; i++)
                {
                    CopyColumn(origin, _positionWB, ref official);
                    _positionWB[1]++;
                }
            }

            // Copiando fecha
            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));
            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value;
                string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

                if (DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
                {
                    dates.Add(objDT);
                    official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = objDT.ToString("MM/dd/yyyy");
                }
                else
                {
                    double value = double.Parse(fecha);
                    DateTime date = DateTime.FromOADate(value);
                    dates.Add(date);
                    official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = date.ToString("MM/dd/yyyy");

                }
            }

            //Copiando el resto
            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempAceiteInf") || c.Dato.Equals("TempAceiteSup") || c.Dato.Equals("TempFP") || c.Dato.Equals("TempTanD") || c.Dato.Equals("TitColumna9") || c.Dato.Equals("TensionPrueba") || c.Dato.Equals("UMTempInf") || c.Dato.Equals("UMTempSup") || c.Dato.Equals("UMTension") || c.Dato.Equals("Resultado"));

            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value;
            }
        }

        public bool Verify_FPC_Columns(SettingsToDisplayFPCReportsDTO reportsDTO, Workbook workbook, int rowsV)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Corriente"));
            int[] _positionWB;
            foreach (ConfigurationReportsDTO item in starts)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);
                for (int i = 0; i < 5; i++)
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
                    if (rowsReaded != rowsV)
                        return false;
                }
            }

            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempAceiteInf") || c.Dato.Equals("TempAceiteSup") || c.Dato.Equals("TempFP") || c.Dato.Equals("TempTanD") || c.Dato.Equals("TitColumna9") || c.Dato.Equals("TensionPrueba") || c.Dato.Equals("UMTempInf") || c.Dato.Equals("UMTempSup") || c.Dato.Equals("UMTension") || c.Dato.Equals("Fecha") || c.Dato.Equals("Resultado"));

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

        public void PrepareTemplate_FPC(SettingsToDisplayFPCReportsDTO reportsDTO, ref Workbook workbook, string specification) => throw new NotImplementedException();
        #endregion

    }
}
